using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NextMile02.Models
{
    public class TruckPushpinInfo
    {
        [Required(ErrorMessage = "Latitude is required")]
        public double latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        public double longitude { get; set; }

        [Required(ErrorMessage = "Truckname is required")]
        public string truckName { get; set; }

        public string location { get; set; }
        public string website { get; set; }
        public string meal { get; set; }
        public int? preference { get; set; }

        public TruckPushpinInfo(string latitude, string longitude, string truckName, string location, string website)
        {
            this.latitude = Convert.ToDouble(latitude);
            this.longitude = Convert.ToDouble(longitude);
            this.truckName = truckName;
            this.location = location;
            this.website = website;
        }

        public TruckPushpinInfo(TruckEvent truckEvent)
        {
            this.truckName = truckEvent.Name;
            this.location = truckEvent.Neighborhood;
            this.website = truckEvent.Url;
            Location loc = MvcApplication.locations[truckEvent.Neighborhood];
            this.latitude = Convert.ToDouble(loc.latitude);
            this.longitude = Convert.ToDouble(loc.longitude);
            this.meal = truckEvent.Time;
            this.preference = null;
        }

        public TruckPushpinInfo(TruckEvent truckEvent, int? preference)
        {
            this.truckName = truckEvent.Name;
            this.location = truckEvent.Neighborhood;
            this.website = truckEvent.Url;
            Location loc = MvcApplication.locations[truckEvent.Neighborhood];
            this.latitude = Convert.ToDouble(loc.latitude);
            this.longitude = Convert.ToDouble(loc.longitude);
            this.meal = truckEvent.Time;
            this.preference = preference;
        }
    }
}