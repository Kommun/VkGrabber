using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkGrabber.Utils;

namespace VkGrabber.ViewModel
{
    public class RootViewModel : PropertyChangedBase
    {
        private MainMenuItem _selectedPage;

        /// <summary>
        /// Список элементов меню
        /// </summary>
        public List<MainMenuItem> MainMenuItems { get; set; }

        /// <summary>
        /// Выбранная страница
        /// </summary>
        public MainMenuItem SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                OnPropertyChanged("SelectedPage");
                App.NavigationService.Navigate(value.Page);
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RootViewModel()
        {
            MainMenuItems = new List<MainMenuItem> {
                new MainMenuItem { Name = "Главная", Page="View/MainView.xaml" },
                new MainMenuItem { Name = "Планировщик", Page="View/SchedulerView.xaml" }
            };

            SelectedPage = MainMenuItems.First();
        }

        public class MainMenuItem
        {
            public string Name { get; set; }

            public string Page { get; set; }
        }
    }
}
