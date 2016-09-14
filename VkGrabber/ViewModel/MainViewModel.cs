using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VkGrabber.Utils;

namespace VkGrabber.ViewModel
{
    public class MainViewModel
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
            var groups = VkSettings.GroupsToGrab.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var group in groups)
            {
                var posts = App.VkApi.GetPosts(group, 1);
                try
                {
                    System.Windows.MessageBox.Show(posts.Items[0].Likes.User_Likes.ToString());
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        }
    }
}
