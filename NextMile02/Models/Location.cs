using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class Location
    {
        public string latitude { get; set;  }
        public string longitude { get; set; }

        public Location()
        { }

        public Location(string locationString)
        {
            string[] coords = locationString.Split(',');
            this.latitude = coords[0];
            this.longitude = coords[1];
        }
    }

}