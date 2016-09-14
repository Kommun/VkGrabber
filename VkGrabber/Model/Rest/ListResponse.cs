using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    /// <summary>
    /// Ответ сервера в виде списка
    /// </summary>
    /// <typeparam name="T">Тип элемента списка</typeparam>
    public class ListResponse<T>
    {
        /// <summary>
        /// Количество элементов списка
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Элементы
        /// </summary>
        public List<T> Items { get; set; }
    }
}
