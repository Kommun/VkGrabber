using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Utils
{
    public class SchedulerSettings
    {
        /// <summary>
        /// Начальное время добавления постов
        /// </summary>
        public TimeSpan FromTime { get; set; }

        /// <summary>
        /// Конечное время добавления постов
        /// </summary>
        public TimeSpan ToTime { get; set; }

        /// <summary>
        /// Интервал между постами
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Погрещность
        /// </summary>
        public TimeSpan Error { get; set; }

        /// <summary>
        /// Время добавления следующего поста
        /// </summary>
        public DateTime NextPostDate { get; set; }

        /// <summary>
        /// Посчитать дату добавления следующего поста
        /// </summary>
        public void CalculateNextPostDate()
        {
            // Вычисляем погрешность в минутах
            var errorMinutes = Error.Hours * 60 + Error.Minutes;
            var error = new Random().Next(-errorMinutes, errorMinutes);

            // Высчитываем время добавления следующего поста
            if (NextPostDate.TimeOfDay.Add(Interval) > ToTime)
                NextPostDate = NextPostDate.Date.AddDays(1).Add(FromTime).AddMinutes(error);
            else
                NextPostDate = NextPostDate.Add(Interval).AddMinutes(error);
        }
    }
}
