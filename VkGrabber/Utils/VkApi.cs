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
        public ListResponse<Post> GetPosts(string group, int count, int offset)
        {
            var request = new RestRequest("wall.get", Method.GET);
            request.AddParameter("domain", group);
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
        public object Post(string groupId, bool fromGroup, string message, List<Attachment> attachments, DateTimeOffset? publishDate = null)
        {
            var request = new RestRequest("wall.post", Method.POST);
            request.AddParameter("owner_id", $"-{groupId}");
            request.AddParameter("from_group", fromGroup);
            request.AddParameter("message", message);
            string attachmentsString = string.Join(",", attachments.Select(a => $"photo{a.Photo.Owner_Id}_{a.Photo.Id}"));

            var client = new WebClient();
            // Получаем сервер для загрузки фото
            var uploadServer = GetWallUploadServer(groupId);

            foreach (var attach in attachments)
            {

                try
                {
                    // Скачиваем фото из группы
                    var photo = client.DownloadData(attach.Photo.Photo_604);
                    // Загружаем фото на сервер
                    //var client = new RestClient(b.Upload_Url);
                    //var req = new RestRequest();
                    //req.AddFile("", photo, attachments[0].Photo.Id.ToString());
                    //var res = client.qExecute<UploadResult>(req);                    
                    var res = client.UploadData(uploadServer.Upload_Url, photo);
                    MessageBox.Show(res.Length.ToString());
                    // Сохраняем фото на стене
                    SaveWallPhoto(groupId, "", "", "");
                }
                catch (Exception ex) { MessageBox.Show(ex.InnerException.Message); }
            }
            return null;

            request.AddParameter("attachments", attachmentsString);

            if (publishDate != null)
                request.AddParameter("publish_date", publishDate.Value.ToUnixTimeSeconds());

            return Execute<object>(request);
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

        public void SaveWallPhoto(string groupId, string photo, string server, string hash)
        { }
    }
}
