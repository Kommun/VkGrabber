using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkGrabber.Model.Rest
{
    public class WallUploadServer
    {
        public string Upload_Url { get; set; }

        public long Album_Id { get; set; }

        public long User_Id { get; set; }
    }
}
