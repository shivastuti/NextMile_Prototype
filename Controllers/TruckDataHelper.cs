using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace NextMile02.Controllers
{
    public static class TruckDataHelper
    {
        //static Uri TruckUri = new Uri("http://data.streetfoodapp.com/1.1/schedule/boston/");
        //static Uri TruckUri = new Uri("http://www.cityofboston.gov/foodtrucks/schedule-app-min.asp");
        //static Uri TruckUri = new Uri("http://www.cityofboston.gov/business/mobile/schedule-app.asp?v=1&71");
        static Uri TruckUri = new Uri("http://www.cityofboston.gov/business/mobile/schedule-app-min.asp");

        static List<Models.TruckEvent> allTruckEvents = null;

        private static void PrintAllTruckEvents()
        {
            foreach (var truckevent in allTruckEvents)
            {
                truckevent.Print();
            }
        }
        private static void PrintEventsToCsv()
        {
            // Headings
            Console.WriteLine("TruckName;TruckUrl;Day;Time;Neighborhood");

            foreach (var truckevent in allTruckEvents)
            {
                truckevent.PrintCsv();
            }
        }

        public static List<Models.TruckEvent> GetHtmlViaWebRequest()
        {
            //System.Net.WebClient client = new System.Net.WebClient();
            //client.DownloadDataAsync(new Uri("http://data.streetfoodapp.com/1.1/schedule/boston/"));

            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(TruckUri);

            // Get the response.
            WebResponse response = request.GetResponse();

            // Read response from server
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            // Parse results from response
            List<Models.TruckEvent> events = GetEventsFromResponse(responseFromServer);

            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return events;
        }

        private static List<Models.TruckEvent> GetEventsFromResponse(string responseFromServer)
        {
            // New empty list
            List<Models.TruckEvent> events = new List<Models.TruckEvent>();

            string[] separator = new string[] { "<tr class=\"trFoodTrucks\">" };
            string[] rawevents = responseFromServer.Split(separator, new StringSplitOptions());

            foreach (string blurb in rawevents)
            {
                if (blurb.Contains("<td class=\"dow\">"))
                {
                    Models.TruckEvent item = new Models.TruckEvent(blurb);
                    events.Add(item);
                }
            }
            return events;
        }

    }
}