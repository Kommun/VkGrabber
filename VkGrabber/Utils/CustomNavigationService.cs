using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace VkGrabber.Utils
{
    public class CustomNavigationService
    {
        private NavigationService _navigationService;

        /// <summary>
        /// Текущая страница
        /// </summary>
        public Page CurrentPage { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="navigationService"></param>
        public CustomNavigationService(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Перейти на страницу
        /// </summary>
        /// <param name="page"></param>
        public void Navigate(Page page)
        {
            CurrentPage = page;
            _navigationService.Navigate(page);
        }
    }
}
