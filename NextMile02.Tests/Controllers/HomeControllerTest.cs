using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NextMile02.Controllers;
using NextMile02.Models;

namespace NextMile02.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        [TestMethod]
        public void Index_Get_IndexView_Empty_NotNull()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(), 
                new MockTruckDataRepository(), 
                new MockCurrentUser());

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index_Get_IndexView_Empty_ViewNameIndex()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(), 
                new MockTruckDataRepository(), 
                new MockCurrentUser());

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_IndexView_Empty_Neighborhoods_Contains_ShowAll()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(),
                new MockCurrentUser());

            // Act
            ViewResult result = controller.Index();

            // Assert
            var neighborhoods = result.ViewData["CurrentNeighborhoods"] as SelectList;
            Assert.AreEqual(neighborhoods.SelectedValue, Constants.AllNeighborhoodsString);
            var trucknames = result.ViewData["CurrentTrucks"] as SelectList;
            Assert.AreEqual(trucknames.SelectedValue, Constants.AllTrucksString);
        }

        [TestMethod]
        public void Index_Get_IndexView_Empty_Contains_NoPins()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(),
                new MockCurrentUser());

            // Act
            ViewResult result = controller.Index();

            // Assert
            var pins = result.ViewData["CurrentTruckPins"] as List<Models.TruckPushpinInfo>;
            Assert.IsNull(pins);
        }

        [TestMethod]
        public void Index_Get_IndexView_Anonymous_1Event_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
            };
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                new MockCurrentUser());

            // Act
            ViewResult result = controller.Index(setDay : "Friday", setMeal : "Lunch");

            // Assert
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            // Deserialize
            //MapView view = serializer.Deserialize<MapView>(example);
            var pinsString = result.ViewData["CurrentTruckPins"] as string;
            List<Models.TruckPushpinInfo> pins = serializer.Deserialize<List<Models.TruckPushpinInfo>>(pinsString);
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 1);
            Assert.AreEqual(pins[0].truckName, "TestTruck1");
            Assert.AreEqual(pins[0].location, "NEU, on Opera Place at Huntington Ave");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0913604));
        }

        [TestMethod]
        public void Index_Get_IndexView_UserNoPreference_1Event_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            ViewResult result = controller.Index(setDay: "Friday", setMeal: "Lunch");

            // Assert
            var pinsString = result.ViewData["CurrentTruckPins"] as string;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Models.TruckPushpinInfo> pins = serializer.Deserialize<List<Models.TruckPushpinInfo>>(pinsString);
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 1);
            Assert.AreEqual(pins[0].truckName, "TestTruck1");
            Assert.AreEqual(pins[0].location, "NEU, on Opera Place at Huntington Ave");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0913604));
            Assert.AreEqual(pins[0].preference, null);
        }

        [TestMethod]
        public void Index_Get_IndexView_UserWithPreferenceUpvote_1Event_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
            };
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck1",
                    preference = 1  // Upvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            ViewResult result = controller.Index(setDay: "Friday", setMeal: "Lunch");

            // Assert
            var pinsString = result.ViewData["CurrentTruckPins"] as string;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Models.TruckPushpinInfo> pins = serializer.Deserialize<List<Models.TruckPushpinInfo>>(pinsString);
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 1);
            Assert.AreEqual(pins[0].truckName, "TestTruck1");
            Assert.AreEqual(pins[0].location, "NEU, on Opera Place at Huntington Ave");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0913604));
            Assert.AreEqual(pins[0].preference, 1);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_AllNeighborhoods_Anonymous_2Event2Neighborhood_Validate_2Pins()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                new MockCurrentUser());

            // Act
            JsonResult result = controller.FilterTrucks(Constants.AllNeighborhoodsString, "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 2);
            Assert.AreEqual(pins[1].truckName, "TestTruck2");
            Assert.AreEqual(pins[1].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[1].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[1].preference, null);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_1Neighborhood_Anonymous_2Event2Neighborhood_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                new MockCurrentUser());

            // Act
            JsonResult result = controller.FilterTrucks("SoWa Open Market, 500 Harrison Avenue", "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 1);
            Assert.AreEqual(pins[0].truckName, "TestTruck2");
            Assert.AreEqual(pins[0].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[0].preference, null);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_AllNeighborhoods_UserNoPreference_2Event2Neighborhood_Validate_2Pins()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            JsonResult result = controller.FilterTrucks(Constants.AllNeighborhoodsString, "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 2);
            Assert.AreEqual(pins[1].truckName, "TestTruck2");
            Assert.AreEqual(pins[1].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[1].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[1].preference, null);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_AllNeighborhoods_UserWithPreference_2Event2Neighborhood_Validate_2Pins()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck2",
                    preference = 1  // Upvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            JsonResult result = controller.FilterTrucks(Constants.AllNeighborhoodsString, "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 2);
            Assert.AreEqual(pins[1].truckName, "TestTruck2");
            Assert.AreEqual(pins[1].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[1].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[1].preference, 1);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_1Neighborhood_UserNoPreference_2Event2Neighborhood_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            JsonResult result = controller.FilterTrucks("SoWa Open Market, 500 Harrison Avenue", "","Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(1, pins.Count);
            Assert.AreEqual(pins[0].truckName, "TestTruck2");
            Assert.AreEqual(pins[0].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[0].preference, null);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_1Neighborhood_UserWithPreference_2Event2Neighborhood_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck2",
                    preference = 1  // Upvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            JsonResult result = controller.FilterTrucks("SoWa Open Market, 500 Harrison Avenue", "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(
                1, 
                pins.Count);
            Assert.AreEqual(
                "TestTruck2", 
                pins[0].truckName);
            Assert.AreEqual(
                "SoWa Open Market, 500 Harrison Avenue", 
                pins[0].location);
            Assert.AreEqual(
                (Double)(-71.0674873), 
                pins[0].longitude);
            Assert.AreEqual(
                1, 
                pins[0].preference);
        }

        [TestMethod]
        public void FilterNeighborhood_Get_1Neighborhood_UserWithPreference_3Event2Neighborhood_Validate_2Pins()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
                    new TruckEvent() { 
                    Name = "TestTruck3",
                    Url = "www.testtruck3.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck3",
                    preference = 2  // Downvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(testEvents),
                user);

            // Act
            JsonResult result = controller.FilterTrucks("SoWa Open Market, 500 Harrison Avenue", "", "Friday", "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(2,                                          pins.Count);
            Assert.AreEqual("TestTruck3",                               pins[1].truckName);
            Assert.AreEqual("SoWa Open Market, 500 Harrison Avenue",    pins[1].location);
            Assert.AreEqual((Double)(-71.0674873),                      pins[1].longitude);
            Assert.AreEqual(2,                                          pins[1].preference);
        }

        [TestMethod]
        public void FilterTrucks_Get_1Truckname_Anonymous_2Event2Neighborhood_Validate_1Pin()
        {
            // Arrange
            List<Models.TruckEvent> testEvents = new List<TruckEvent>()
            {
                new TruckEvent() { 
                    Name = "TestTruck1",
                    Url = "www.testtruck1.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "NEU, on Opera Place at Huntington Ave",
                    Coordinates = new Location("42.3398106,-71.0913604")
                },
                    new TruckEvent() { 
                    Name = "TestTruck2",
                    Url = "www.testtruck2.com",
                    Day = "Friday",
                    Time = "Lunch",
                    Neighborhood = "SoWa Open Market, 500 Harrison Avenue",
                    Coordinates = new Location("42.3425311,-71.0674873")
                },
            };
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(testEvents),
                new MockCurrentUser());

            // Act
            JsonResult result = controller.FilterTrucks(Constants.AllNeighborhoodsString, truckname: "TestTruck2", day: "Friday", meal: "Lunch");

            // Assert
            dynamic jsonResult = result.Data;
            var pins = jsonResult.PushpinFilteredData as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(pins);
            Assert.AreEqual(pins.Count, 1);
            Assert.AreEqual(pins[0].truckName, "TestTruck2");
            Assert.AreEqual(pins[0].location, "SoWa Open Market, 500 Harrison Avenue");
            Assert.AreEqual(pins[0].longitude, (Double)(-71.0674873));
            Assert.AreEqual(pins[0].preference, null);
        }

        [TestMethod]
        public void VoteFoodTruck_Post_UserNoPriorPreference_UpVote_Validate_NewUpvote()
        {
            // Arrange
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(),
                user);

            // Act
            JsonResult result = controller.VoteFoodTruck("TestTruck2", "1"); // New Upvote

            // Assert
            dynamic jsonResult = result.Data;
            //var pins = result.Data as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(
                true,
                jsonResult.success);
            Assert.AreEqual(
                1,
                jsonResult.newIconColor);
        }

        [TestMethod]
        public void VoteFoodTruck_Post_UserWithPriorUpvotePreference_UpVote_Validate_ToggleNoPreference()
        {
            // Arrange
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck2",
                    preference = 1  // Prior Upvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(),
                user);

            // Act
            JsonResult result = controller.VoteFoodTruck("TestTruck2", "1"); // New Upvote

            // Assert
            dynamic jsonResult = result.Data;
            //var pins = result.Data as List<Models.TruckPushpinInfo>;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(
                true,
                jsonResult.success);
            Assert.AreEqual(
                0,
                jsonResult.newIconColor);
        }

        [TestMethod]
        public void VoteFoodTruck_Post_UserWithPriorUpvotePreference_DownVote_Validate_ChangeNewDownvote()
        {
            // Arrange
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck2",
                    preference = 1  // Prior Upvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(),
                user);

            // Act
            JsonResult result = controller.VoteFoodTruck("TestTruck2", "2"); // New DownVote

            // Assert
            dynamic jsonResult = result.Data;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(
                true,
                jsonResult.success);
            Assert.AreEqual(
                2,
                jsonResult.newIconColor);
        }

        [TestMethod]
        public void VoteFoodTruck_Post_UserWithPriorDownvotePreference_UpVote_Validate_ChangeNewUpVote()
        {
            // Arrange
            List<Models.Preference.PreferenceData> testPreferences = new List<Preference.PreferenceData>()
            {
                new Preference.PreferenceData() { 
                    userid = "TestUser1",
                    truckname = "TestTruck2",
                    preference = 2  // Prior Downvote
                },
            };
            var user = new MockCurrentUser();
            user.setUserId("TestUser1");
            var controller = new HomeController(
                new MockPreferenceRepository(testPreferences),
                new MockTruckDataRepository(),
                user);

            // Act
            JsonResult result = controller.VoteFoodTruck("TestTruck2", "1"); // New Upvote

            // Assert
            dynamic jsonResult = result.Data;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(
                true,
                jsonResult.success);
            Assert.AreEqual(
                1,
                jsonResult.newIconColor);
        }

        [TestMethod]
        public void About_Get_AboutView_Empty_NotNull()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(),
                new MockCurrentUser());

            // Act
            ViewResult result = controller.About();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About_Get_AboutView_Empty_ViewNameAbout()
        {
            // Arrange
            var controller = new HomeController(
                new MockPreferenceRepository(),
                new MockTruckDataRepository(),
                new MockCurrentUser());

            // Act
            ViewResult result = controller.About();

            // Assert
            Assert.AreEqual("About", result.ViewName);
        }
    }
}
