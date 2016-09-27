using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VkGrabber.Utils;
using VkGrabber.Model.Messenger;

namespace VkGrabber.ViewModel
{
    public class SettingsViewModel
    {
        /// <summary>
        /// Обновить список постов
        /// </summary>
        public ICommand RefreshPostsCommand { get; set; } = new CustomCommand((object o) => Messenger.Default.Send<GrabMessage>(null));

        /// <summary>
        /// Параметры
        /// </summary>
        public VkSettings VkSettings
        {
            get { return App.VkSettings; }
        }
    }
}
