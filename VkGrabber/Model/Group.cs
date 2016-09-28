using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkGrabber.Utils;

namespace VkGrabber.Model
{
    public class Group : PropertyChangedBase
    {
        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Минимальное количество лайков
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Минимальное количество репостов
        /// </summary>
        public int RepostCount { get; set; }

        private int _offset;
        /// <summary>
        /// Сдвиг
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                OnPropertyChanged("Offset");
            }
        }

        private bool _isActive = true;
        /// <summary>
        /// Активна ли группа
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }
    }
}
