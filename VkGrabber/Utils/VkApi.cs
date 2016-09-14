using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using VkGrabber.Model.Rest;

namespace VkGrabber.Utils
{
    public class VkApi
    {
        const string BaseUrl = "https://api.vk.com/method";

        readonly string _access_token;
        readonly string _secretKey;

        public VkApi(string access_token, string secretKey)
        {
            _access_token = access_token;
            _secretKey = secretKey;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            //client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("access_token", _access_token);
            request.AddParameter("v", "5.53");
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }

        /// <summary>
        /// Получить список постов группы
        /// </summary>
        /// <param name="group">Название группы</param>
        /// <param name="count">Количество постов</param>
        /// <returns></returns>
        public ListResponse<Post> GetPosts(string group, int count)
        {
            var request = new RestRequest("wall.get.xml", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("domain", group);
            request.AddParameter("count", count);
            return Execute<ListResponse<Post>>(request);
        }
    }
}
