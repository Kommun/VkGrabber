using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkGrabber.Utils;

namespace VkGrabber.ViewModel
{
    public class SettingsViewModel
    {
        /// <summary>
        /// Параметры
        /// </summary>
        public VkSettings VkSettings
        {
            get { return App.VkSettings; }
        }
    }
}
