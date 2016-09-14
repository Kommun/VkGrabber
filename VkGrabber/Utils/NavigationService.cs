using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VkGrabber.Utils
{
    public class NavigationService
    {
        private Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void Navigate(Page page)
        {
            _mainFrame.NavigationService.Navigate(page);
        }
    }
}
