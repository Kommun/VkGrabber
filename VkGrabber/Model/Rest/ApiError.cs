using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class ApiError
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int Error_Code { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Error_Msg { get; set; }
    }
}
