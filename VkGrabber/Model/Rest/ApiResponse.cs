using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class ApiResponse<T>
    {
        public T Response { get; set; }

        public ApiError Error { get; set; }
    }
}
