using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
   public class Repost
    {
        /// <summary>
        /// Количество репостов
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Репостнул ли пользователь
        /// </summary>
        public bool User_Reposted { get; set; }
    }
}
