using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class Post
    {
        /// <summary>
        /// Id поста
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public long Date { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Лайки
        /// </summary>
        public Like Likes { get; set; }

        /// <summary>
        /// Репосты
        /// </summary>
        public Repost Reposts { get; set; }

        /// <summary>
        /// Информация о группе
        /// </summary>
        public GroupInfo GroupInfo { get; set; }

        /// <summary>
        /// Локальное время
        /// </summary>
        public string LocalDate
        {
            get { return DateTimeOffset.FromUnixTimeSeconds(Date).ToLocalTime().ToString("dd MMM yyyy в HH:mm"); }
        }
    }
}
