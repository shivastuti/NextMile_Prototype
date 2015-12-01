using NextMile02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NextMile02.Controllers
{
    public class HomeController : Controller
    {
        // Preference repository
        IPreferenceRepository _preferenceStore;

        // Truck Events repository
        ITruckDataRepository _truckEventsSource;

        // Cache of Current Truck Events.  Removing for now unless necessary for performance.
        //static List<Models.TruckEvent> currentEvents;

        // Test userid for debugging purposes
        static string testuser = null;

        public HomeController() : this(new DB_PreferenceRepository(), new TruckDataRepository()) { }

        public HomeController(IPreferenceRepository preferenceStore, ITruckDataRepository truckEventsSource)
        {
            _preferenceStore = preferenceStore;
            _truckEventsSource = truckEventsSource;
        }

        public ActionResult Index()
        {

            // Obtain Facebook UserId from Session state uid
            string loggedinuser = (String)Session["uid"];

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            // Obtain list of current events
            var currentEvents = getCurrentEvents();

            List<Models.TruckPushpinInfo> currentTruckPins = null;

            if (userid != null)
            {

                // Obtain User Preferences
                var preferences = _preferenceStore.GetPreferencesForUser(userid);

                // Obtain list of current Truck PushPins for view, left-outer join with user preferences
                currentTruckPins = (from te in currentEvents
                                    join trucktemp in preferences on te.Name equals trucktemp.truckname into tempjoin
                                    from profile in tempjoin.DefaultIfEmpty()
                                    select new Models.TruckPushpinInfo(te, profile == null ? null : profile.preference)).ToList();
            }
            else
            {
                // Obtain list of current Truck PushPins for view
                currentTruckPins = (from te in currentEvents
                                    select new Models.TruckPushpinInfo(te)).ToList();
            }

            // Obtain list of distinct locations
            List<Models.Neighborhood> currentNeighborhoods = (from te in currentEvents
                                                              group te by te.Neighborhood into n
                                                              select new Models.Neighborhood(n.Key))
                                                              .Distinct().ToList();
            //Inserts Show all Locations to dropdown List
            currentNeighborhoods.Insert(0, new Models.Neighborhood("Show All"));

            currentTruckPins = OffsetLatitudeToEachTruckWithSameLoc(currentTruckPins);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewData["CurrentTruckPins"] = serializer.Serialize(currentTruckPins);
            ViewData["CurrentNeighborhoods"] = currentNeighborhoods;
            return View();
        }

        //This method needs refactoring
        //Decluttering the pushpins
        private static List<Models.TruckPushpinInfo> OffsetLatitudeToEachTruckWithSameLoc(List<Models.TruckPushpinInfo> currentTruckPins)
        {
            var groupedTrucks = currentTruckPins
                                      .GroupBy(u => u.location)
                                      .Select(grp => grp.ToList())
                                      .ToList();

            //each group contains trucks with same latitude
            double padding = 0.0002;

            //Clear the currentTruckPins so we can put back to same location
            currentTruckPins.Clear();

            foreach (var group in groupedTrucks)
            {
                double offset = padding;
                //skip the first item and then add offset
                foreach (var truck in group)
                {
                    truck.latitude += offset;
                    offset += padding;
                    currentTruckPins.Add(truck);
                }
            }
            return currentTruckPins;
        }

        private List<Models.TruckEvent> getCurrentEvents()
        {
            // Determine today's day of week
            string today = DateTime.Now.DayOfWeek.ToString();

            // Determine current upcoming meal
            string meal =
                DateTime.Now.Hour < 10 ? "Breakfast" :
                DateTime.Now.Hour < 15 ? "Lunch" :
                DateTime.Now.Hour < 21 ? "Dinner" : "Breakfast"; //For testing changed

            // Hardcode any date/meal values here for testing
            today = "Wednesday";
            meal = "Lunch";

            // Obtain ALL truck events by scraping cityofboston.gov
            List<Models.TruckEvent> allEvents = _truckEventsSource.GetAllTruckData();

            // Obtain CURRENT truck events by filtering to current day/time
            List<Models.TruckEvent> currentEvents = (from te in allEvents
                                                     where te.Day == today && te.Time == meal
                                                     select te).ToList();
            return currentEvents;
        }

        [HttpPost]
        public ActionResult FilterNeighborhood(String selection)
        {
            // Obtain neighborhood from dropdownlist input
            string neighborhood = selection.Trim('"');

            // Obtain Facebook UserId from Session state uid
            string loggedinuser = (String)Session["uid"];

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            // Obtain list of current events
            var currentEvents = getCurrentEvents();

            List<Models.TruckPushpinInfo> currentTruckPins = null;
            if (userid != null)
            {
                // Obtain User Preferences
                var preferences = _preferenceStore.GetPreferencesForUser(userid);

                // Obtain list of current Truck PushPins for view, left-outer join with user preferences
                //System.Data.SqlClient.SqlException - To be handled
                currentTruckPins = (from te in currentEvents
                                    join trucktemp in preferences on te.Name equals trucktemp.truckname into tempjoin
                                    from profile in tempjoin.DefaultIfEmpty()
                                    select new Models.TruckPushpinInfo(te, profile == null ? null : profile.preference)).ToList();
            }
            else
            {
                currentTruckPins = (from te in currentEvents
                                    select new Models.TruckPushpinInfo(te)).ToList();
            }
            List<Models.TruckPushpinInfo> localTruckPins;

            if (neighborhood.Equals("Show All", StringComparison.Ordinal))
            {
                //Display All the trucks
                localTruckPins = currentTruckPins;
            }
            else
            {
                // Obtain list of current Truck PushPins for selected neighborhood
                localTruckPins = (from truckPin in currentTruckPins
                                  where String.Compare(truckPin.location, neighborhood, true) == 0
                                  select truckPin).ToList();
            }

            //Decluttering pushpins
            localTruckPins = OffsetLatitudeToEachTruckWithSameLoc(localTruckPins);
            return Json(localTruckPins, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult VoteFoodTruck(String foodTruckName, String vote)
        {
            // Obtain neighborhood from dropdownlist input
            string truckName = foodTruckName.Trim('"');

            // Obtain Facebook UserId from Session state uid
            string loggedinuser = (String)Session["uid"];

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            // Note: we can remove this check once we remove this method from anonymous users UI.
            int userPreferenceAfter = -1;
            var message = "";
            var sucessVal = false;
            int? color = Convert.ToInt32(vote); //should retain old value as it is in case of error
            if (userid != null)
            {
                // Update User Preference for specified Truck, retain new value
                userPreferenceAfter = _preferenceStore.UpdatePreference(userid, truckName, vote);
                if (userPreferenceAfter != -1)
                {
                    //Sucess
                    // Use updated user preference to pass updated color to view                    
                    message = "Preferences Updated";
                    sucessVal = true;
                    color = userPreferenceAfter;
                    //json = "{\"newIconColor\":\"" + color.ToString() + "\"}";
                }
                else
                {
                    sucessVal = false;
                    message = "Preferences Not Saved Sucessfully";
                }
            }
            else
            {
                sucessVal = false;
                message = "Log In to use this feature";
            }


            return Json(new { sucess = sucessVal, message = message, newIconColor = color });
        }

        public ActionResult About()
        {
            ViewBag.Message = "NextMile team. Northeastern University";

            return View();
        }
    }
}