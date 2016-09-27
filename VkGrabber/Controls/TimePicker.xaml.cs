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

namespace VkGrabber.Controls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public event EventHandler TimeChanged;

        #region TimeProperty        

        /// <summary>
        /// Время
        /// </summary>
        public TimeSpan? Time
        {
            get { return (TimeSpan?)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeSpan?), typeof(TimePicker), new PropertyMetadata(new PropertyChangedCallback(OnTimeChanged)));

        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var timePicker = sender as TimePicker;

            timePicker.TimeChanged?.Invoke(timePicker, EventArgs.Empty);
            timePicker.cbHours.SelectedIndex = ((TimeSpan)e.NewValue).Hours;
            timePicker.cbMinutes.SelectedIndex = ((TimeSpan)e.NewValue).Minutes;
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public TimePicker()
        {
            InitializeComponent();
            cbHours.ItemsSource = Enumerable.Range(0, 24);
            cbMinutes.ItemsSource = Enumerable.Range(0, 60);
        }

        /// <summary>
        /// Обработчик изменения значения комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Time = new TimeSpan((int?)cbHours.SelectedValue ?? 0, (int?)cbMinutes.SelectedValue ?? 0, 0);
        }
    }
}
