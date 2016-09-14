using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VkGrabber.Utils;

namespace VkGrabber
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CustomNavigationService NavigationService { get; set; }
        public static VkSettings VkSettings { get; private set; }
        public static VkApi VkApi { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public App()
        {
            VkSettings = SettingsManager.DeSerializeObject<VkSettings>("settings") ?? new VkSettings();
            VkApi = new VkApi(VkSettings);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SettingsManager.SerializeObject(VkSettings, "settings");
        }
    }
}
