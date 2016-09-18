using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestSharp;
using VkGrabber.Model.Rest;

namespace VkGrabber.Utils
{
    public class VkApi
    {
        const string BaseUrl = "https://api.vk.com/method";

        private readonly VkSettings _settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings"></param>
        public VkApi(VkSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Выполнить запрос
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            request.AddParameter("access_token", _settings.AccessToken);
            request.AddParameter("v", "5.53");

            var response = client.Execute<ApiResponse<T>>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }

            if (response.Data.Error != null)
                MessageBox.Show(response.Data.Error.Error_Msg);

            return response.Data.Response;
        }

        /// <summary>
        /// Получить информацию о группах
        /// </summary>
        /// <param name="groupIds">Id групп</param>
        /// <returns></returns>
        public List<GroupInfo> GetGroupsById(params string[] groupIds)
        {
            var request = new RestRequest("groups.getById", Method.GET);
            request.AddParameter("group_ids", string.Join(",", groupIds));
            return Execute<List<GroupInfo>>(request);
        }

        /// <summary>
        /// Получить список постов группы
        /// </summary>
        /// <param name="group">Название группы</param>
        /// <param name="count">Количество постов</param>
        /// <returns></returns>
        public ListResponse<Post> GetPosts(long groupId, int count, int offset)
        {
            var request = new RestRequest("wall.get", Method.GET);
            request.AddParameter("owner_id", $"-{groupId}");
            request.AddParameter("count", count);
            request.AddParameter("offset", offset);
            return Execute<ListResponse<Post>>(request);
        }

        /// <summary>
        /// Запостить на стену
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="fromGroup">Добавить запись от лица группы</param>
        /// <param name="message">Текст</param>
        /// <param name="attachments">Прикрепленные документы</param>
        /// <returns></returns>
        public void Post(string groupId, bool fromGroup, string message, List<Attachment> attachments, DateTimeOffset? publishDate = null)
        {
            var request = new RestRequest("wall.post", Method.POST);
            request.AddParameter("owner_id", $"-{groupId}");
            request.AddParameter("from_group", fromGroup);
            request.AddParameter("message", message);

            string attachmentsString = "";
            var client = new WebClient();
            // Получаем сервер для загрузки фото
            var uploadServer = GetWallUploadServer(groupId);

            foreach (var attach in attachments)
            {
                string fileName = $"{attach.Photo.Id}.png";

                // Скачиваем фото из группы
                client.DownloadFile(attach.Photo.BiggestPhoto, fileName);

                // Загружаем фото на сервер
                var res = client.UploadFile(uploadServer.Upload_Url, fileName);
                var uploadResult = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadResult>(Encoding.UTF8.GetString(res));

                // Удаляем локальный файл
                System.IO.File.Delete(fileName);

                // Сохраняем фото на стене
                var photo = SaveWallPhoto(groupId, uploadResult);
                attachmentsString += $"photo{photo.Owner_Id}_{photo.Id},";
            }

            request.AddParameter("attachments", attachmentsString);

            if (publishDate != null)
                request.AddParameter("publish_date", publishDate.Value.ToUnixTimeSeconds());

            Execute<object>(request);
        }

        /// <summary>
        /// Получить сервер для загрузки фотографий
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <returns></returns>
        public WallUploadServer GetWallUploadServer(string groupId)
        {
            var request = new RestRequest("photos.getWallUploadServer", Method.GET);
            request.AddParameter("group_id", groupId);
            return Execute<WallUploadServer>(request);
        }

        /// <summary>
        /// Сохранить фото в альбоме стены
        /// </summary>
        /// <param name="groupId">Id группы</param>
        /// <param name="uploadResult">Результат загрузки фото на сервер</param>
        /// <returns></returns>
        public Photo SaveWallPhoto(string groupId, UploadResult uploadResult)
        {
            var request = new RestRequest("photos.saveWallPhoto", Method.POST);
            request.AddParameter("group_id", groupId);
            request.AddParameter("photo", uploadResult.Photo);
            request.AddParameter("server", uploadResult.Server);
            request.AddParameter("hash", uploadResult.Hash);
            return Execute<List<Photo>>(request).SingleOrDefault();
        }
    }
}
