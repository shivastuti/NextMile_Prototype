using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class TruckDataRepository : ITruckDataRepository
    {
        public List<Models.TruckEvent> GetAllTruckData()
        {
            return Controllers.TruckDataHelper.GetHtmlViaWebRequest();
        }
    }
}