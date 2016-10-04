using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using VkGrabber.Utils;
using mshtml;

namespace VkGrabber.View
{
    /// <summary>
    /// Interaction logic for AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : Page
    {
        private bool logOuted = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthorizationView() : this(false) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logout"></param>
        public AuthorizationView(bool logout)
        {
            InitializeComponent();
            OnLoad(logout);
        }

        private async void OnLoad(bool logout)
        {
            wbAuthorization.Visibility = Visibility.Hidden;
            if (logout)
            {
                wbAuthorization.LoadCompleted += WbAuthorization_MainPageLoadCompleted;
                wbAuthorization.Navigate("https://m.vk.com/");
                await Task.Run(() =>
                {
                    while (!logOuted) { }
                });
            }

            wbAuthorization.Navigated += WbAuthorization_AuthorizeNavigated;
            wbAuthorization.Navigate(string.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token",
                  VkSettings.AppId, VkSettings.Scopes, VkSettings.RedirectUri));
        }

        /// <summary>
        /// Обработчик загрузки главной страницы мобильной версии ВК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WbAuthorization_MainPageLoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var document = wbAuthorization.Document as IHTMLDocument3;
                var logoutLink = document.getElementsByTagName("li")
                    .Cast<IHTMLElement>()
                    .FirstOrDefault(el => el.getAttribute("className") == "mmi_logout")
                    .children[0].getAttribute("href");
                if (!string.IsNullOrEmpty(logoutLink))
                {
                    wbAuthorization.Navigated += WbAuthorization_LogoutNavigated;
                    wbAuthorization.Navigate(logoutLink);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            wbAuthorization.LoadCompleted -= WbAuthorization_MainPageLoadCompleted;
        }

        /// <summary>
        /// Обработчик перехода по ссылке логаута
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WbAuthorization_LogoutNavigated(object sender, NavigationEventArgs e)
        {
            // Вышли из учетной записи
            logOuted = true;
            wbAuthorization.Navigated -= WbAuthorization_LogoutNavigated;
        }

        /// <summary>
        /// Обработчик перехода на страницу авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WbAuthorization_AuthorizeNavigated(object sender, NavigationEventArgs e)
        {
            wbAuthorization.Visibility = Visibility.Visible;
            if (string.IsNullOrEmpty(e.Uri.Fragment))
                return;

            var urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment.Substring(1));
            App.VkSettings.AccessToken = urlParams.Get("access_token");
            App.VkSettings.UserId = urlParams.Get("user_id");
            App.NavigationService = new CustomNavigationService(NavigationService);
            App.NavigationService.Navigate(new RootView());

            wbAuthorization.Navigated -= WbAuthorization_AuthorizeNavigated;
        }
    }
}
