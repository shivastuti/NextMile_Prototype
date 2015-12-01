using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NextMile02
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static string locationsfilepath = HttpContext.Current.Server.MapPath("~/Data/NextMileTruckLocations.tsv");

        public static Dictionary<string, Models.Location> locations = new Dictionary<string, Models.Location>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            /* Loads location to coordinates map*/
            LoadLocationsFromFile();
        }

        
        /// <summary>
        /// This loads the NextMileTruckLocations.tsv DB file which maps
        /// Locations to Coordinates
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,Models.Location> LoadLocationsFromFile()
        {
            using (System.IO.StreamReader locationfile = new System.IO.StreamReader(locationsfilepath))
            {
                string line;

                while ((line = locationfile.ReadLine()) != null)
                {
                    string[] record = line.Split('\t');
                    if (record[0] == "Neighborhood") continue;
                    string[] coords = record[1].Trim('"').Split(',');
                    string neighborhood = record[0].Trim('"');
                    string x = coords[0];
                    string y = coords[1]; //NOTE- Double to String For coordinates to check
                    locations.Add(
                        neighborhood,
                        new Models.Location
                        {
                            latitude = x,
                            longitude = y
                        });
                }
            }

            return locations;
        }

    }
}
