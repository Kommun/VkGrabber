using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class Like
    {
        /// <summary>
        /// Количество лайков
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Лайкнул ли текущий пользователь запись
        /// </summary>
        public bool User_Likes { get; set; }

        /// <summary>
        /// Можно ли лайкнуть запись
        /// </summary>
        public bool Can_Like { get; set; }

        /// <summary>
        /// Можно ли опубликовать запись
        /// </summary>
        public bool Can_Publish { get; set; }
    }
}
