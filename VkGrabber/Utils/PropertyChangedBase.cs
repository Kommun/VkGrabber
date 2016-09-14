using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VkGrabber.Utils
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Обработчик изменения привязанного свойства
        /// </summary>
        /// <param name="propName"></param>
        public void OnPropertyChanged(string propName = "")
        {
            try
            {
                var handler = PropertyChanged;

                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propName));
            }
            catch { }
        }

        /// <summary>
        /// Обработчик изменения набора привязанных свойств
        /// </summary>
        /// <param name="propNames"></param>
        public void OnPropertiesChanged(params string[] propNames)
        {
            foreach (var prop in propNames)
                OnPropertyChanged(prop);
        }
    }
}