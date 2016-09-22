using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using VkGrabber.Utils;
using VkGrabber.Model.Rest;

namespace VkGrabber.ViewModel
{
    public class PostsListViewModel : PropertyChangedBase
    {
        private Post _currentZoomedPost;
        private string _zoomedPhoto;
        private Visibility _zoomedPhotoVisibility = Visibility.Collapsed;

        #region Commands

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
        /// Запостить с помощью планировщика
        /// </summary>
        public ICommand PostWithSchedulerCommand { get; set; }

        /// <summary>
        /// Открыть оригинал
        /// </summary>
        public ICommand OpenOriginalCommand { get; set; }

        /// <summary>
        /// Увеличить фото
        /// </summary>
        public ICommand ZoomCommand { get; set; }

        /// <summary>
        /// Увеличить следующее фото
        /// </summary>
        public ICommand ZoomNextCommand { get; set; }

        /// <summary>
        /// Скрыть увеличенное фото
        /// </summary>
        public ICommand HideZoomedPhotoCommand { get; set; }

        public ICommand CopyUrlCommand { get; set; }

        #endregion

        #region Properties    

        /// <summary>
        /// Ширина поста
        /// </summary>
        public int PostWidth { get; } = 550;

        /// <summary>
        /// Url увеличенного фото
        /// </summary>
        public string ZoomedPhoto
        {
            get { return _zoomedPhoto; }
            set
            {
                _zoomedPhoto = value;
                OnPropertyChanged("ZoomedPhoto");
            }
        }

        /// <summary>
        /// Видимость увеличенного фото
        /// </summary>
        public Visibility ZoomedPhotoVisibility
        {
            get { return _zoomedPhotoVisibility; }
            set
            {
                _zoomedPhotoVisibility = value;
                OnPropertyChanged("ZoomedPhotoVisibility");
            }
        }

        /// <summary>
        /// Отфильтованные по лайкам и репостам записи
        /// </summary>
        public ObservableCollection<Post> FilteredPosts { get; set; } = new ObservableCollection<Post>();

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public PostsListViewModel()
        {
            GrabCommand = new CustomCommand(Grab);
            PostCommand = new CustomCommand(Post);
            PostAtTimeCommand = new CustomCommand(PostAtTime);
            PostWithSchedulerCommand = new CustomCommand(PostWithScheduler);
            OpenOriginalCommand = new CustomCommand(OpenOriginal);
            ZoomCommand = new CustomCommand(Zoom);
            ZoomNextCommand = new CustomCommand(ZoomNext);
            HideZoomedPhotoCommand = new CustomCommand(HideZoomedPhoto);
            CopyUrlCommand = new CustomCommand(CopyUrl);

            Grab();
        }

        /// <summary>
        /// Получить список постов
        /// </summary>
        /// <param name="parameter"></param>
        private void Grab(object parameter = null)
        {
            List<Post> posts = new List<Post>();
            foreach (var group in App.VkSettings.Groups)
            {
                var groupInfo = App.VkApi.GetGroupsById(group.Name).SingleOrDefault();
                var res = App.VkApi.GetPosts(groupInfo.Id, 100, group.Offset);
                res.Items.ForEach(p => p.GroupInfo = groupInfo);

                // Отбираем посты, к которым ничего не прикреплено или прикреплены только фото, а также фильтруем по количеству лайков и репостов
                posts.AddRange(res.Items.Where(p =>
                    p.Attachments?.All(a => a.Type == "photo") != false
                    && p.Likes.Count >= group.LikeCount
                    && p.Reposts.Count >= group.RepostCount));
            }

            posts.ForEach(p =>
            {
                if (p.Attachments?.Count > 0)
                    SetPhotoSize(p.Attachments.Select(a => a.Photo).ToList());

                FilteredPosts.Add(p);
            });
        }

        /// <summary>
        /// Установить размер фото
        /// </summary>
        /// <param name="photo"></param>
        private void SetPhotoSize(List<Photo> photo)
        {
            if (photo.Count == 1)
            {
                photo.Single().Width = double.NaN;
                photo.Single().Height = double.NaN;
            }

            if (photo.Count >= 2 && photo.Count <= 3)
                ResizePhotos(photo);

            if (photo.Count >= 4)
            {
                ResizePhotos(photo.GetRange(0, 2));
                ResizePhotos(photo.GetRange(2, photo.Count - 2));
            }
        }

        /// <summary>
        /// Подстраивает массив фото под ширину поста
        /// </summary>
        /// <param name="photo"></param>
        private void ResizePhotos(List<Photo> photo)
        {
            double minHeight = photo.Min(i => i.Height);
            photo.ForEach(i =>
            {
                i.Width = (int)(i.Width / (i.Height / minHeight));
                i.Height = minHeight;
            });

            double summaryWidth = photo.Sum(i => i.Width);
            double koef = summaryWidth / (PostWidth - 50 - photo.Count * 4);
            photo.ForEach(i =>
            {
                i.Width = (int)(i.Width / koef);
                i.Height = (int)(i.Height / koef);
            });
        }

        /// <summary>
        /// Запостить
        /// </summary>
        /// <param name="parameter"></param>
        private void Post(object parameter = null)
        {
            var post = parameter as Post;
            App.VkApi.Post(App.VkSettings.TargetGroup, true, post.Text, post.Attachments);
        }

        /// <summary>
        /// Запостить в определенное время
        /// </summary>
        /// <param name="parameter"></param>
        private void PostAtTime(object parameter = null)
        {
            var post = parameter as Post;
            var time = new Controls.DateTimeDialog().ShowModal();

            if (time == null)
                return;

            if (time < DateTime.Now.AddMinutes(1))
            {
                MessageBox.Show("Невозможно добавить пост с прошедшей датой");
                return;
            }

            App.VkApi.Post(App.VkSettings.TargetGroup, true, post.Text, post.Attachments, time);
        }

        /// <summary>
        /// Запостить с помощью планировщика
        /// </summary>
        /// <param name="parameter"></param>
        private void PostWithScheduler(object parameter)
        {
            if (App.VkSettings.SchedulerSettings.NextPostDate == null)
            {
                MessageBox.Show("Сначала настройте планировщик");
                return;
            }

            var post = parameter as Post;
            App.VkApi.Post(App.VkSettings.TargetGroup, true, post.Text, post.Attachments, App.VkSettings.SchedulerSettings.NextPostDate);
            App.VkSettings.SchedulerSettings.CalculateNextPostDate();
        }

        /// <summary>
        /// Открыть оригинал
        /// </summary>
        /// <param name="parameter"></param>
        private void OpenOriginal(object parameter = null)
        {
            var post = parameter as Post;
            var url = string.Format("https://vk.com/{0}{1}?w=wall-{1}_{2}", GetGroupTypeUrl(post.GroupInfo), post.GroupInfo.Id, post.Id);
            Process.Start(new ProcessStartInfo(url));
        }

        /// <summary>
        /// Получить тип группы для ссылки
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetGroupTypeUrl(GroupInfo info)
        {
            switch (info.Type)
            {
                case "page":
                    return "public";
                case "group":
                    return "club";
                case "event":
                    return "event";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Увеличить фото
        /// </summary>
        /// <param name="parameter"></param>
        private void Zoom(object parameter = null)
        {
            var multiparameter = parameter as object[];
            ZoomedPhoto = (multiparameter[0] as Attachment).Photo.BiggestPhoto;
            _currentZoomedPost = multiparameter[1] as Post;
            ZoomedPhotoVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Увеличить следующее фото
        /// </summary>
        /// <param name="parameter"></param>
        private void ZoomNext(object parameter = null)
        {
            int nextIndex = _currentZoomedPost.Attachments.IndexOf(_currentZoomedPost.Attachments.Single(a => a.Photo.BiggestPhoto == ZoomedPhoto)) + 1;
            if (nextIndex < _currentZoomedPost.Attachments.Count)
                ZoomedPhoto = _currentZoomedPost.Attachments[nextIndex].Photo.BiggestPhoto;

            else HideZoomedPhoto();
        }

        /// <summary>
        /// Скрыть увеличенное фото
        /// </summary>
        /// <param name="parameter"></param>
        private void HideZoomedPhoto(object parameter = null)
        {
            ZoomedPhotoVisibility = Visibility.Collapsed;
            _currentZoomedPost = null;
        }

        private void CopyUrl(object parameter)
        {
            Clipboard.SetText(ZoomedPhoto);
        }
    }
}
