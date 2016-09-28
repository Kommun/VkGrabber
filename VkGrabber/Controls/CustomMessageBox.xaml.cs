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
using System.Windows.Shapes;

namespace VkGrabber.Controls
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private int? _result { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Показать модальное окно
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public int? ShowModal(string caption, params string[] buttons)
        {
            tbCaption.Text = caption;
            int index = 0;
            icButtons.ItemsSource = buttons.Select(b => new { Text = b, Index = index++ });
            ShowDialog();
            return _result;
        }

        /// <summary>
        /// Обработчик нажатия кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _result = (int)(sender as Button).Tag;
            Close();
        }
    }
}
