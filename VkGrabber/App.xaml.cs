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
        public static VkSettings VkSettings { get; private set; }
        public static NavigationService NavigationService { get; set; }
        public static VkApi VkApi { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public App()
        {
            VkSettings = SettingsManager.DeSerializeObject<VkSettings>("settings") ?? new VkSettings();
            VkApi = new VkApi(VkSettings.AccessToken, "");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SettingsManager.SerializeObject(VkSettings, "settings");
        }
    }
}
