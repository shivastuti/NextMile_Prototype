using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class UserInfo
    {
        public string fbname { get; set; }
        public string fbid { get; set; }
        public string fbimg { get; set; }

        public UserInfo()
        { 
        
        }
        public UserInfo(string fbname,string fbid, string fbimg )
        {
            this.fbname = fbname;
            this.fbid = fbid;
            this.fbimg = fbimg;      
        }
    }
}