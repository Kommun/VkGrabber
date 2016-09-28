using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkGrabber.Model;
using VkGrabber.Enums;

namespace VkGrabber.Utils
{
    public class VkSettings
    {
        public const string AppId = "5627431";
        public const long Scopes = (long)Scope.Wall + (long)Scope.Photos;
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
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();

        /// <summary>
        /// Название целевой группы
        /// </summary>
        public string TargetGroup { get; set; }

        /// <summary>
        /// Настройки планировщика
        /// </summary>
        public SchedulerSettings SchedulerSettings { get; set; } = new SchedulerSettings();
    }
}
