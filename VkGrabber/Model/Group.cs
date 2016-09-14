using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model
{
    public class Group
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
    }
}
