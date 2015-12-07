using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NextMile02.Models;

namespace NextMile02.Controllers
{
    public class HomeController : Controller
    {
        // Preference repository
        IPreferenceRepository _preferenceStore;

        // Truck Events repository
        ITruckDataRepository _truckEventsSource;

        // Current User Id
        ICurrentUser _currentUser;

        // Cache of Current Truck Events.  Removing for now unless necessary for performance.
        //static List<Models.TruckEvent> currentEvents;

        // Test userid for debugging purposes
        static string testuser = null;

        public HomeController()
            : this(
                new DB_PreferenceRepository(),
                new TruckDataRepository(),
                new CurrentUser()) { }

        public HomeController(
            IPreferenceRepository preferenceStore,
            ITruckDataRepository truckEventsSource,
            ICurrentUser currentUser)
        {
            _preferenceStore = preferenceStore;
            _truckEventsSource = truckEventsSource;
            _currentUser = currentUser;
        }

        public ViewResult Index(string setDay = "", string setMeal = "")
        {

            // Obtain Facebook UserId from Session state uid
            string loggedinuser = _currentUser.UserId();

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            String day = setDay;
            String meal = setMeal;

            // Obtain list of current events
            var currentEvents = getCurrentEvents(ref day, ref meal);

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

            List<String> currentNeighborhoods = getNeighborhoodList(currentEvents);

            List<String> currentTrucknames = getTruckNameList(currentEvents);

            currentTruckPins = OffsetLatitudeToEachTruckWithSameLoc(currentTruckPins);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewData["CurrentTruckPins"] = serializer.Serialize(currentTruckPins);
            ViewData["CurrentNeighborhoods"] = new SelectList(currentNeighborhoods, Constants.AllNeighborhoodsString);
            ViewData["CurrentTrucks"] = new SelectList(currentTrucknames, Constants.AllTrucksString);
            ViewData["MealTimes"] = new SelectList(new[] { "Breakfast", "Lunch", "Dinner", "Late Night" }, meal);
            ViewData["DaysInWeek"] = new SelectList(new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, day);
            return View("Index", ViewData);
        }

        private static List<String> getTruckNameList(List<TruckEvent> currentEvents)
        {
            // Obtain list of distinct truck names sorted alphabetically
            List<String> currentTrucknames = (from te in currentEvents
                                              select te.Name)
                                              .Distinct().OrderBy(n => n).ToList();

            //Inserts Show all Locations to dropdown List
            currentTrucknames.Insert(0, Constants.AllTrucksString);
            return currentTrucknames;
        }

        private static List<String> getNeighborhoodList(List<TruckEvent> currentEvents)
        {
            // Obtain list of distinct locations sorted alphabetically
            List<String> currentNeighborhoods = (from te in currentEvents
                                                 select te.Neighborhood)
                                                 .Distinct().OrderBy(n => n).ToList();

            //Inserts Show all Locations to dropdown List
            currentNeighborhoods.Insert(0, Constants.AllNeighborhoodsString);
            return currentNeighborhoods;
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

        private List<Models.TruckEvent> getCurrentEvents(ref string setDay, ref string setMeal)
        {
            // Obtain datetime per Boston timezone
            TimeZoneInfo BostonTime = System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime now = System.TimeZoneInfo.ConvertTime(DateTime.Now, System.TimeZoneInfo.Local, BostonTime);

            // Determine today's day of week
            string today = now.DayOfWeek.ToString();

            // Determine current upcoming meal
            string meal =
                now.Hour < 10 ? "Breakfast" :
                now.Hour < 15 ? "Lunch" :
                now.Hour < 22 ? "Dinner" : "Late Night"; //Aligned according to city of boston site

            // Hardcode any date/meal values here for testing/debugging
            //today = "Wednesday";
            //meal = "Lunch";

            // For Unit Testing
            if (setDay != "")
                today = setDay;
            if (setMeal != "")
                meal = setMeal;

            // Obtain ALL truck events by scraping cityofboston.gov
            List<Models.TruckEvent> allEvents = _truckEventsSource.GetAllTruckData();

            // Obtain CURRENT truck events by filtering to current day/time
            List<Models.TruckEvent> selectedEvents = (from te in allEvents
                                                      where te.Day == today && te.Time == meal
                                                      select te).ToList();
            setDay = today;
            setMeal = meal;
            return selectedEvents;
        }

        [HttpPost]
        public JsonResult FilterTrucks(string neighborhood = "", string truckname = "", string day = "", string meal = "")
        {
            // Obtain Facebook UserId from Session state uid if logged in
            string loggedinuser = _currentUser.UserId();

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            // Obtain selections from dropdownlist inputs
            neighborhood = neighborhood.Trim('"');
            truckname = truckname.Trim('"');
            day = day.Trim('"');
            meal = meal.Trim('"');

            String tempDay = day;
            String tempMeal = meal;
            // Obtain list of events from specified meal
            var selectedEvents = getCurrentEvents(ref tempDay, ref tempMeal);

            // Filter by neighborhood if required
            if (neighborhood != "" && !neighborhood.Equals(Constants.AllNeighborhoodsString, StringComparison.Ordinal))
            {
                // Obtain list of current Truck events for selected neighborhood
                selectedEvents = (from truck in selectedEvents
                                  where String.Compare(truck.Neighborhood, neighborhood, true) == 0
                                  select truck).ToList();
            }

            // Filter by TruckName if required
            if (truckname != "" && !truckname.Equals(Constants.AllTrucksString, StringComparison.Ordinal))
            {
                // Obtain list of current Truck events for selected truck name
                selectedEvents = (from truck in selectedEvents
                                  where String.Compare(truck.Name, truckname, true) == 0
                                  select truck).ToList();
            }

            List<Models.TruckPushpinInfo> selectedTruckPins = null;
            if (userid != null)
            {
                // Obtain User Preferences
                var preferences = _preferenceStore.GetPreferencesForUser(userid);

                // Obtain list of current Truck PushPins for view, left-outer join with user preferences
                selectedTruckPins = (from te in selectedEvents
                                     join trucktemp in preferences on te.Name equals trucktemp.truckname into tempjoin
                                     from profile in tempjoin.DefaultIfEmpty()
                                     select new Models.TruckPushpinInfo(te, profile == null ? null : profile.preference)).ToList();
            }
            else
            {
                selectedTruckPins = (from te in selectedEvents
                                     select new Models.TruckPushpinInfo(te)).ToList();
            }

            // Declutter pushpins
            selectedTruckPins = OffsetLatitudeToEachTruckWithSameLoc(selectedTruckPins);

            List<String> currentNeighborhoods = getNeighborhoodList(selectedEvents);
            List<String> currentTrucknames = getTruckNameList(selectedEvents);

            // Return filtered truck pins
            return Json(new { PushpinFilteredData = selectedTruckPins, FilteredNeighborhoods = currentNeighborhoods, FilteredTrucks = currentTrucknames }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VoteFoodTruck(String foodTruckName, String vote)
        {
            // Obtain neighborhood from dropdownlist input
            string truckName = foodTruckName.Trim('"');

            // Obtain Facebook UserId from Session state uid
            string loggedinuser = _currentUser.UserId();

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            // Note: we can remove this check once we remove this method from anonymous users UI.
            int userPreferenceAfter = -1;
            var message = "";
            var successVal = false;
            int? color = Convert.ToInt32(vote); //should retain old value as it is in case of error
            if (userid != null)
            {
                // Update User Preference for specified Truck, retain new value
                userPreferenceAfter = _preferenceStore.UpdatePreference(userid, truckName, vote);
                if (userPreferenceAfter != -1)
                {
                    // Success
                    // Use updated user preference to pass updated color to view                    
                    message = "Preferences Updated";
                    successVal = true;
                    color = userPreferenceAfter;
                }
                else
                {
                    successVal = false;
                    message = "Preferences Not Saved Sucessfully";
                }
            }
            else
            {
                successVal = false;
                message = "Log In to use this feature";
            }

            return Json(new { success = successVal, message = message, newIconColor = color });
        }

        [HttpPost]
        public JsonResult GetPreferencesForUser()
        {
            // Obtain Facebook UserId from Session state uid if logged in
            string loggedinuser = _currentUser.UserId();

            // Use logged in user if available
            string userid =
                loggedinuser != null ? loggedinuser :
                testuser;

            var preferences = new List<Models.Preference.PreferenceData>();
            if (userid != null)
            {
                // Obtain User Preferences
                preferences = _preferenceStore.GetPreferencesForUser(userid).ToList();
            }

            // Return filtered truck pins
            return Json(new { PreferenceData = preferences }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult About()
        {
            ViewBag.Message = "NextMile team. Northeastern University";

            return View("About", ViewBag);
        }
        public ViewResult Welcome()
        {
            

            return View("Welcome", ViewBag);
        }
    }
}