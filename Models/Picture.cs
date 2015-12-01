using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class Picture
    {
        public PicureData data { get; set; }
    }
    public class PicureData
    {
        public string url { get; set; }
        public bool is_silhouette { get; set; }
    }
}