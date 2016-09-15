using System;
using System.Collections.Generic;
using System.Linq;
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
            string attach = string.Join(",", attachments.Select(a => $"photo{a.Photo.Owner_Id}_{a.Photo.Id}"));
            request.AddParameter("attachments", attach);

            if (publishDate != null)
                request.AddParameter("publish_date", publishDate.Value.ToUnixTimeSeconds());

            return Execute<object>(request);
        }
    }
}
