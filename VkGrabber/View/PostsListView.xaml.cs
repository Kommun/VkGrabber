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
            DataContext = new ViewModel.PostsListViewModel();

            // Подписываемся на обновление списка постов
            Messenger.Default.Register(this, (GrabMessage o) => ScrollToTop());
        }

        /// <summary>
        /// Прокрутить в начало списка
        /// </summary>
        private void ScrollToTop()
        {
            try
            {
                (((VisualTreeHelper.GetChild(lwPosts, 0) as Border).Child) as ScrollViewer).ScrollToVerticalOffset(0);
            }
            catch { }
        }

        /// <summary>
        /// Обработчик нажатия на список
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lwPosts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(lwPosts).X <= grdToTop.ActualWidth)
                ScrollToTop();
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
