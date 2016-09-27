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
        public DateTime? DateTime
        {
            get { return (DateTime?)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(OnDateTimeChanged)));

        private static void OnDateTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var dateTimePicker = sender as DateTimePicker;
            dateTimePicker.tPicker.Time = ((DateTime)e.NewValue).TimeOfDay;
            dateTimePicker.dPicker.SelectedDate = ((DateTime)e.NewValue).Date;
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
            var date = dPicker.SelectedDate;
            var time = tPicker.Time;

            if (date == null || time == null)
                DateTime = null;
            else
                DateTime = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,
                    time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
        }
    }
}
