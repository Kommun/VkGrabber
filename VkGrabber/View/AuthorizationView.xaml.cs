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
        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthorizationView() : this(false) { }

        bool l = false;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logout"></param>
        public AuthorizationView(bool logout)
        {
            InitializeComponent();

            if (logout)
            {
                wbAuthorization.LoadCompleted += WbAuthorization_LoadCompleted;
                wbAuthorization.Navigate("https://m.vk.com/");
            }

            else {
                wbAuthorization.Navigated += WbAuthorization_AuthorizeNavigated;
                wbAuthorization.Navigate(string.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token",
                      VkSettings.AppId, VkSettings.Scopes, VkSettings.RedirectUri));
            }
        }

        private void WbAuthorization_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var document = wbAuthorization.Document as IHTMLDocument3;
                var inputs = document.getElementsByTagName("a");
                foreach (IHTMLElement element in inputs)
                {
                    var a = element.getAttribute("className");
                    if (element.getAttribute("className") == "lfm_item")
                        wbAuthorization.Navigate(element.getAttribute("href"));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void WbAuthorization_AuthorizeNavigated(object sender, NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Uri.Fragment))
                return;

            var urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment.Substring(1));
            App.VkSettings.AccessToken = urlParams.Get("access_token");
            App.VkSettings.UserId = urlParams.Get("user_id");
            App.NavigationService = new CustomNavigationService(NavigationService);
            App.NavigationService.Navigate(new RootView());
        }

        private void WbAuthorization_LogoutNavigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
