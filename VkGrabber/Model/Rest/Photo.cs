using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class Photo
    {
        /// <summary>
        /// Id фото
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id владельца фото
        /// </summary>
        public long Owner_Id { get; set; }

        /// <summary>
        /// Фото с шириной 604
        /// </summary>
        public string Photo_604 { get; set; }

        /// <summary>
        /// Фото с шириной 1280
        /// </summary>
        public string Photo_1280 { get; set; }
    }
}
