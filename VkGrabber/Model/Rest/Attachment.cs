using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class Attachment
    {
        /// <summary>
        /// Тип прикрепленного документа
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Фото
        /// </summary>
        public Photo Photo { get; set; }
    }
}
