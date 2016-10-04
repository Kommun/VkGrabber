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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VkGrabber.Utils;
using VkGrabber.Model.Messenger;

namespace VkGrabber.View
{
    /// <summary>
    /// Interaction logic for PostsListView.xaml
    /// </summary>
    public partial class PostsListView : Page
    {
        private double? _lastVerticalOffset;
        private ScrollViewer _sw;

        /// <summary>
        /// Находится ли курсор над левой панелью
        /// </summary>
        private bool? _mouseOver;

        /// <summary>
        /// Конструктор
        /// </summary>
        public PostsListView()
        {
            InitializeComponent();
            Loaded += PostsListView_Loaded;
            DataContext = new ViewModel.PostsListViewModel();

            // Подписываемся на обновление списка постов
            Messenger.Default.Register(this, (GrabMessage o) => _sw?.ScrollToTop());
        }

        /// <summary>
        /// Обработчик полной загрузки страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostsListView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _sw = ((VisualTreeHelper.GetChild(lwPosts, 0) as Border).Child) as ScrollViewer;
                _sw.ScrollChanged += _sw_ScrollChanged;
            }
            catch { }
        }

        /// <summary>
        /// Обработчик прокрутки списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _sw_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_sw.VerticalOffset != 0)
                tbScroll.Text = "˄ Наверх";
            else if (_lastVerticalOffset != null)
                tbScroll.Text = "˅";
            else tbScroll.Text = "";
        }

        /// <summary>
        /// Обработчик нажатия на список
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lwPosts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_sw == null || e.GetPosition(lwPosts).X > grdToTop.ActualWidth)
                return;

            if (_sw.VerticalOffset == 0 && _lastVerticalOffset != null)
                _sw.ScrollToVerticalOffset(_lastVerticalOffset.Value);
            else
            {
                _lastVerticalOffset = _sw.VerticalOffset;
                _sw.ScrollToTop();
            }
        }

        /// <summary>
        /// Обработчик движения мыши над списком
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lwPosts_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                bool mouseOver = e.GetPosition(lwPosts).X <= grdToTop.ActualWidth;

                if (mouseOver == _mouseOver)
                    return;

                _mouseOver = mouseOver;
                (Resources[mouseOver ? "mouseEnter" : "mouseLeave"] as Storyboard).Begin();
            }
            catch { }
        }
    }
}
