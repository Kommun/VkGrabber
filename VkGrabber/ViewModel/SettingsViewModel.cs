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
    public class SettingsViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Обновить список постов
        /// </summary>
        public ICommand RefreshPostsCommand { get; set; } = new CustomCommand((object p) => Messenger.Default.Send<GrabMessage>(null));

        /// <summary>
        /// Выйти из учетной записи
        /// </summary>
        public ICommand LogoutCommand { get; set; } = new CustomCommand((object p) => { App.NavigationService.Navigate(new View.AuthorizationView(true)); });

        private User _currentUser;
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

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
            GetCurrentUser();
        }

        /// <summary>
        /// Получить информацию о текущем пользователе
        /// </summary>
        private async void GetCurrentUser()
        {
            CurrentUser = (await App.VkApi.GetUsersById(VkSettings.UserId)).FirstOrDefault();
        }
    }
}
