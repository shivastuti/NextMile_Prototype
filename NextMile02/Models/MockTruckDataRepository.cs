using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class MockTruckDataRepository : ITruckDataRepository
    {
        List<Models.TruckEvent> _db = new List<TruckEvent>();

        public MockTruckDataRepository()
        { }
        public MockTruckDataRepository(List<TruckEvent> events)
        {
            _db = events;
        }
        public List<Models.TruckEvent> GetAllTruckData()
        {
            return _db;
        }

        public void AddEvent(Models.TruckEvent item)
        {
            _db.Add(item);
        }
    }
}