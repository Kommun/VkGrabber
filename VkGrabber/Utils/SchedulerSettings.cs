using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Utils
{
    public class SchedulerSettings : PropertyChangedBase
    {
        /// <summary>
        /// Начальное время добавления постов
        /// </summary>
        public TimeSpan? FromTime { get; set; }

        /// <summary>
        /// Конечное время добавления постов
        /// </summary>
        public TimeSpan? ToTime { get; set; }

        /// <summary>
        /// Интервал между постами
        /// </summary>
        public TimeSpan? Interval { get; set; }

        /// <summary>
        /// Погрещность
        /// </summary>
        public TimeSpan? Error { get; set; }

        private DateTime? _nextPostDate;
        /// <summary>
        /// Время добавления следующего поста
        /// </summary>
        public DateTime? NextPostDate
        {
            get { return _nextPostDate; }
            set
            {
                _nextPostDate = value;
                OnPropertyChanged("NextPostDate");
            }
        }

        /// <summary>
        /// Посчитать дату добавления следующего поста
        /// </summary>
        public void CalculateNextPostDate()
        {
            // Вычисляем погрешность в минутах
            var error = Error == null ? 0 : (new Random().Next(-(int)Error?.TotalMinutes, (int)Error?.TotalMinutes));

            // Высчитываем время добавления следующего поста
            if (NextPostDate?.TimeOfDay.Add(Interval.Value) > ToTime)
                NextPostDate = NextPostDate?.Date.AddDays(1).Add(FromTime.Value).AddMinutes(error);
            else
                NextPostDate = NextPostDate?.Add(Interval.Value).AddMinutes(error);
        }
    }
}
