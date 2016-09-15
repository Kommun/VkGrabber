using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using VkGrabber.Utils;
using VkGrabber.Model.Rest;

namespace VkGrabber.ViewModel
{
    public class MainViewModel : PropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Получить список постов
        /// </summary>
        public ICommand GrabCommand { get; set; }

        /// <summary>
        /// Запостить
        /// </summary>
        public ICommand PostCommand { get; set; }

        /// <summary>
        /// Выложить в определнное время
        /// </summary>
        public ICommand PostAtTimeCommand { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        public VkSettings VkSettings
        {
            get { return App.VkSettings; }
        }

        /// <summary>
        /// Отфильтованные по лайкам и репостам записи
        /// </summary>
        public List<Post> FilteredPosts { get; set; }

        private bool _isSettingsExpanded = true;
        /// <summary>
        /// Развернуты ли настройки
        /// </summary>
        public bool IsSettingsExpanded
        {
            get { return _isSettingsExpanded; }
            set
            {
                _isSettingsExpanded = value;
                OnPropertyChanged("IsSettingsExpanded");
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            GrabCommand = new CustomCommand(Grab);
            PostCommand = new CustomCommand(Post);
            PostAtTimeCommand = new CustomCommand(PostAtTime);
        }

        /// <summary>
        /// Получит список постов
        /// </summary>
        /// <param name="parameter"></param>
        private void Grab(object parameter)
        {
            IsSettingsExpanded = false;
            List<Post> posts = new List<Post>();
            foreach (var group in VkSettings.Groups)
            {
                var res = App.VkApi.GetPosts(group.Name, 100, group.Offset);
                posts.AddRange(res.Items.Where(p => p.Likes.Count >= group.LikeCount && p.Reposts.Count >= group.RepostCount));
            }

            FilteredPosts = posts;
            OnPropertyChanged("FilteredPosts");
        }

        /// <summary>
        /// Запостить
        /// </summary>
        /// <param name="parameter"></param>
        private void Post(object parameter = null)
        {
            var post = parameter as Post;
            App.VkApi.Post(VkSettings.TargetGroup, true, post.Text, post.Attachments);
        }

        /// <summary>
        /// Запостить в определенное время
        /// </summary>
        /// <param name="parameter"></param>
        private void PostAtTime(object parameter = null)
        {
            var post = parameter as Post;
            var time = new Controls.DateTimeDialog().ShowModal();

            if (time != null)
                App.VkApi.Post(VkSettings.TargetGroup, true, post.Text, post.Attachments, time);
        }
    }
}
