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
        /// Ширина
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Высота
        /// </summary>
        public double Height { get; set; }

        #region Url фото в разных размерах

        public string Photo_75 { get; set; }

        public string Photo_130 { get; set; }

        public string Photo_604 { get; set; }

        public string Photo_807 { get; set; }

        public string Photo_1280 { get; set; }

        public string Photo_2560 { get; set; }

        /// <summary>
        /// Самый большой доступный формат фото
        /// </summary>
        public string BiggestPhoto
        {
            get { return new string[] { Photo_2560, Photo_1280, Photo_807, Photo_604, Photo_130, Photo_75 }.First(p => !string.IsNullOrEmpty(p)); }
        }

        #endregion
    }
}
