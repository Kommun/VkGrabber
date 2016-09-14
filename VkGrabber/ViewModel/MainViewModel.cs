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
        /// <summary>
        /// Получить список постов
        /// </summary>
        public ICommand GrabCommand { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        public VkSettings VkSettings
        {
            get { return App.VkSettings; }
        }

        public List<Post> FilteredPosts { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            GrabCommand = new CustomCommand(Grab);
        }

        /// <summary>
        /// Получит список постов
        /// </summary>
        /// <param name="parameter"></param>
        private void Grab(object parameter)
        {
            foreach (var group in VkSettings.Groups)
            {
                var posts = App.VkApi.GetPosts(group.Name, 100);
                FilteredPosts = posts.Items.Where(p => p.Likes.Count >= group.LikeCount && p.Reposts.Count >= group.RepostCount).ToList();
                OnPropertyChanged("FilteredPosts");
                try
                {
                    //System.Windows.MessageBox.Show(posts.Items[0].Text.ToString());
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        }
    }
}
