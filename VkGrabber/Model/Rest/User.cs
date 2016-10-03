using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class User
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string First_Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Last_Name { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public City City { get; set; }

        /// <summary>
        /// Аватар
        /// </summary>
        public string Photo_50 { get; set; }
    }
}
