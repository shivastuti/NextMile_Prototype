using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class FacebookUserModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public Picture picture { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string link { get; set; }
        public int timezone { get; set; }
        public FacebookLocation location { get; set; }
    }
}