using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class GroupInfo
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Короткое название группы
        /// </summary>
        public string Screen_Name { get; set; }

        /// <summary>
        /// Тип сообщества
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Иконка 50*50
        /// </summary>
        public string Photo_50 { get; set; }
    }
}
