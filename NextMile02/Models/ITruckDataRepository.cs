using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public interface ITruckDataRepository
    {
        List<Models.TruckEvent> GetAllTruckData();
    }
}