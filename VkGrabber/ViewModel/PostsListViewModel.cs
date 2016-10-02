using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;
using VkGrabber.Utils;
using VkGrabber.Model.Rest;
using VkGrabber.Model.Messenger;
using VkGrabber.Controls;

namespace VkGrabber.ViewModel
{
    public class PostsListViewModel : PropertyChangedBase
    {
        private Post _currentZoomedPost;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        #region Commands

        /// <summary>
        /// Получить список постов
        /// </summary>
        public ICommand GrabNextCommand { get; set; }

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

        /// <summary>
        /// Поиск по картинке
        /// </summary>
        public ICommand FindImageCommand { get; set; }

        /// <summary>
        /// Очистить список
        /// </summary>
        public ICommand ClearListCommand { get; set; }

        #endregion

        #region Properties    

        /// <summary>
        /// Ширина поста
        /// </summary>
        public int PostWidth { get; } = 550;

        private string _zoomedPhoto;
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

        private Visibility _zoomedPhotoVisibility = Visibility.Collapsed;
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

        private Visibility _loadingIndicatorVisibility = Visibility.Collapsed;
        /// <summary>
        /// Видимость индикатора загрузки
        /// </summary>
        public Visibility LoadingIndicatorVisibility
        {
            get { return _loadingIndicatorVisibility; }
            set
            {
                _loadingIndicatorVisibility = value;
                OnPropertyChanged("LoadingIndicatorVisibility");
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
            GrabNextCommand = new CustomCommand(GrabNext);
            PostCommand = new CustomCommand(Post);
            PostAtTimeCommand = new CustomCommand(PostAtTime);
            PostWithSchedulerCommand = new CustomCommand(PostWithScheduler);
            OpenOriginalCommand = new CustomCommand(OpenOriginal);
            ZoomCommand = new CustomCommand(Zoom);
            ZoomNextCommand = new CustomCommand(ZoomNext);
            HideZoomedPhotoCommand = new CustomCommand(HideZoomedPhoto);
            FindImageCommand = new CustomCommand(FindImage);
            ClearListCommand = new CustomCommand(ClearList);

            Messenger.Default.Register(this, async (GrabMessage o) =>
             {
                 ClearList();
                 await Grab();
             });

            Grab();
        }

        #region Methods

        /// <summary>
        /// Получить список постов
        /// </summary>
        private async Task Grab()
        {
            var activeGroups = App.VkSettings.Groups.Where(g => g.IsActive).ToList();
            foreach (var group in activeGroups)
            {
                var groupInfo = (await App.VkApi.GetGroupsById(group.Name))?.FirstOrDefault();
                if (groupInfo == null)
                {
                    var dialogResult = new CustomMessageBox().ShowModal($"Группа '{group.Name}' не найдена. Удалить ее из списка?", "Да", "Нет");
                    if (dialogResult == 0)
                        App.VkSettings.Groups.Remove(group);

                    continue;
                }

                var res = await App.VkApi.GetPosts(groupInfo.Id, 100, group.Offset);
                if (res == null)
                    continue;

                // Если с сервера получено 0 постов
                if (res.Items.Count == 0)
                {
                    var dialogResult = new CustomMessageBox().ShowModal($"В группе {group.Name} закончились посты", "Деактивировать", "Обнулить сдвиг", "Удалить");
                    if (dialogResult != null)
                    {
                        switch (dialogResult.Value)
                        {
                            case 0:
                                group.IsActive = false;
                                break;
                            case 1:
                                group.Offset = 0;
                                break;
                            case 2:
                                App.VkSettings.Groups.Remove(group);
                                break;
                        }
                    }
                }

                // Отбираем посты, к которым ничего не прикреплено или прикреплены только фото, а также фильтруем по количеству лайков и репостов
                res.Items
                    .Where(p =>
                        p.Attachments?.All(a => a.Type == "photo") != false
                        && p.Likes.Count >= group.LikeCount
                        && p.Reposts.Count >= group.RepostCount)
                    .ToList()
                    .ForEach(p =>
                    {
                        p.GroupInfo = groupInfo;

                        if (p.Attachments?.Count > 0)
                            SetPhotoSize(p.Attachments.Select(a => a.Photo).ToList());

                        _dispatcher.Invoke(() => FilteredPosts.Add(p));
                    });
            }
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
        /// Добавить пост
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task<bool> Post(Post post, DateTimeOffset? date = null)
        {
            bool success = false;
            LoadingIndicatorVisibility = Visibility.Visible;

            var groupInfo = (await App.VkApi.GetGroupsById(App.VkSettings.TargetGroup))?.FirstOrDefault();
            if (groupInfo == null)
            {
                MessageBox.Show("Целевая группа задана неверно");
                return false;
            }

            success = await App.VkApi.Post(groupInfo.Id.ToString(), true, post.Text, post.Attachments, date);

            LoadingIndicatorVisibility = Visibility.Collapsed;
            return success;
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

        #endregion

        #region CommandMethods

        /// <summary>
        /// Получить следующие записи
        /// </summary>
        /// <param name="parameter"></param>
        private void GrabNext(object parameter)
        {
            foreach (var group in App.VkSettings.Groups)
                group.Offset += 100;

            Grab();
        }

        /// <summary>
        /// Запостить
        /// </summary>
        /// <param name="parameter"></param>
        private async void Post(object parameter = null)
        {
            await Post(parameter as Post);
        }

        /// <summary>
        /// Запостить в определенное время
        /// </summary>
        /// <param name="parameter"></param>
        private async void PostAtTime(object parameter = null)
        {
            var time = new DateTimeDialog().ShowModal();

            if (time == null)
                return;

            if (time < DateTime.Now.AddMinutes(1))
            {
                MessageBox.Show("Невозможно добавить пост с прошедшей датой");
                return;
            }

            await Post(parameter as Post, time);
        }

        /// <summary>
        /// Запостить с помощью планировщика
        /// </summary>
        /// <param name="parameter"></param>
        private async void PostWithScheduler(object parameter)
        {
            if (App.VkSettings.SchedulerSettings.FromTime == null || App.VkSettings.SchedulerSettings.ToTime == null)
                MessageBox.Show("Задайте временные рамки планировщика");
            else if (App.VkSettings.SchedulerSettings.Interval == null)
                MessageBox.Show("Задайте интервал планировщика");
            else if (App.VkSettings.SchedulerSettings.NextPostDate == null)
                MessageBox.Show("Задайте дату следующего поста");
            else if (App.VkSettings.SchedulerSettings.NextPostDate < DateTime.Now.AddMinutes(1))
                MessageBox.Show("Дата следующего поста должна быть больше текущей");
            else
            {
                var success = await Post(parameter as Post, App.VkSettings.SchedulerSettings.NextPostDate);
                if (success)
                    App.VkSettings.SchedulerSettings.CalculateNextPostDate();
            }
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

        /// <summary>
        /// Поиск по картинке
        /// </summary>
        /// <param name="parameter"></param>
        private void FindImage(object parameter)
        {
            var url = string.Format("https://www.google.com/searchbyimage?&image_url={0}", ZoomedPhoto);
            Process.Start(new ProcessStartInfo(url));
        }

        /// <summary>
        /// Очистить список
        /// </summary>
        /// <param name="parameter"></param>
        private void ClearList(object parameter = null)
        {
            FilteredPosts.Clear();
        }

        #endregion CommandMethods
    }
}
