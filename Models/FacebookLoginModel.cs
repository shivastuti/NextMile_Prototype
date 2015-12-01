using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace NextMile02.Models
{
    public class FacebookLoginModel
    {
        public string uid { get; set; }
        public string accessToken { get; set; }
        public string flag { get; set; }
    }
}