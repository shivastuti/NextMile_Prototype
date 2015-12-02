using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace NextMile02.Models
{
    public class TruckEvent
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Neighborhood { get; set; }
        public Location Coordinates { get; set; }

        public TruckEvent()
        { }

        public TruckEvent(string html)
        {
            this.Name = WebUtility.HtmlDecode(html.Substring(html.IndexOf('>', html.IndexOf("<td class=\"com\">") + 25) + 1, (html.IndexOf("</a>", html.IndexOf("<td class=\"com\">") + 25) - html.IndexOf('>', (html.IndexOf("<td class=\"com\">") + 25))) - 1));
            this.Day = html.Substring(html.IndexOf("<td class=\"dow\">") + 16, (html.IndexOf("</td>", html.IndexOf("<td class=\"dow\">")) - html.IndexOf("<td class=\"dow\">") - 16));
            this.Time = html.Substring(html.IndexOf("<td class=\"tod\">") + 16, (html.IndexOf("</td>", html.IndexOf("<td class=\"tod\">")) - html.IndexOf("<td class=\"tod\">") - 16));
            this.Url = html.Substring(html.IndexOf("<td class=\"com\">") + 25, (html.IndexOf('>', (html.IndexOf("<td class=\"com\">") + 25)) - html.IndexOf("<td class=\"com\">") - 26));
            this.Neighborhood = html.Substring(html.IndexOf("</script>") + 9, (html.LastIndexOf("</td>") - html.IndexOf("</script>") - 9));
            this.Coordinates = MvcApplication.locations != null ? MvcApplication.locations[Neighborhood] : null;
            //this.Print();
        }

        public void Print()
        {
            Console.WriteLine(this.Name + "\t" + this.Day + "\t" + this.Time + "\t" + this.Url + "\t" + this.Neighborhood);
        }

        public void PrintCsv()
        {
            Console.WriteLine(this.Name + ";" + this.Url + ";" + this.Day + ";" + this.Time + ";" + this.Neighborhood);
        }
    }
}