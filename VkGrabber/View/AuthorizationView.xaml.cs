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
                l = true;
                wbAuthorization.Navigate("https://vk.com/");
            }

            wbAuthorization.Navigated += WbAuthorization_Navigated;
            wbAuthorization.Navigate(string.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=page&response_type=token",
                  VkSettings.AppId, VkSettings.Scopes, VkSettings.RedirectUri));
        }

        private void WbAuthorization_Navigated(object sender, NavigationEventArgs e)
        {

            if (l)
            {

                try
                {
                    MessageBox.Show(e.Uri.ToString());
                    mshtml.IHTMLDocument3 doc = wbAuthorization.Document as mshtml.IHTMLDocument3;
                    doc.getElementById("top_logout_link").click();
                }
                catch (Exception ex) { MessageBox.Show(ex.InnerException.Message); }
            }
            l = false;
            if (string.IsNullOrEmpty(e.Uri.Fragment))
                return;

            var urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment.Substring(1));
            App.VkSettings.AccessToken = urlParams.Get("access_token");
            App.VkSettings.UserId = urlParams.Get("user_id");
            App.NavigationService = new CustomNavigationService(NavigationService);
            App.NavigationService.Navigate(new RootView());
        }
    }
}
