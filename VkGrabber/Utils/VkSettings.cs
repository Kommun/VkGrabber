using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Utils
{
    public class VkSettings
    {
        public const string AppId = "5627431";
        public const int Scope = 8192;
        public const string RedirectUri = "http://oauth.vk.com/blank.html";

        /// <summary>
        /// Токен пользователя
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Id авторизованного пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Список групп, из которых будут браться посты
        /// </summary>
        public string GroupsToGrab { get; set; }

        /// <summary>
        /// Название целевой группы
        /// </summary>
        public string TargetGroup { get; set; }
    }
}
