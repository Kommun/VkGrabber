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
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        #region DateTimeProperty        

        /// <summary>
        /// Дата (со временем)
        /// </summary>
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(OnDateTimeChanged)));

        private static void OnDateTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var timePicker = sender as DateTimePicker;

        }

        #endregion

        public DateTimePicker()
        {
            InitializeComponent();
        }

        private void dPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDateTime();
        }

        private void tPicker_TimeChanged(object sender, EventArgs e)
        {
            RefreshDateTime();
        }

        private void RefreshDateTime()
        {
            var date = dPicker.SelectedDate.Value;
            var time = tPicker.Time;
            DateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
