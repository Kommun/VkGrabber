using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VkGrabber.Utils;
using VkGrabber.Model.Messenger;
using VkGrabber.Model.Rest;

namespace VkGrabber.ViewModel
{
    public class SettingsViewModel
    {
        /// <summary>
        /// Обновить список постов
        /// </summary>
        public ICommand RefreshPostsCommand { get; set; } = new CustomCommand((object p) => Messenger.Default.Send<GrabMessage>(null));

        /// <summary>
        /// Выйти из учетной записи
        /// </summary>
        public ICommand LogoutCommand { get; set; } = new CustomCommand((object p) => { App.NavigationService.Navigate(new View.AuthorizationView(true)); });

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        public VkSettings VkSettings
        {
            get { return App.VkSettings; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SettingsViewModel()
        {
            CurrentUser = App.VkApi.GetUsersById(VkSettings.UserId).FirstOrDefault();
        }
    }
}
