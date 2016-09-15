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
    /// Interaction logic for DateTimeDialog.xaml
    /// </summary>
    public partial class DateTimeDialog : Window
    {
        public DateTimeDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public DateTime? ShowModal()
        {
            ShowDialog();
            return dp.Value;
        }
    }
}
