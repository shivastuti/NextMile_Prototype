using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class MockPreferenceRepository : IPreferenceRepository
    {
        private List<Preference.PreferenceData> _db = new List<Preference.PreferenceData>();

        public MockPreferenceRepository()
        { }

        public MockPreferenceRepository(List<Preference.PreferenceData> preferences)
        {
            _db = preferences;
        }

        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<Preference.PreferenceData> GetAllPreferences()
        {
            // Obtain User Preferences
            var preferences = (from pref in _db
                               select pref
                               ).AsEnumerable();

            return preferences;
        }

        public IEnumerable<Preference.PreferenceData> GetPreferencesForUser(string userid)
        {
            // Obtain User Preferences for Specified User
            var preferences = (from pref in _db
                               where pref.userid == userid
                               select pref
                               ).AsEnumerable();

            return preferences;
        }

        public Preference.PreferenceData GetPreferenceForUserAndTruck(string userid, string truckName)
        {
            // Obtain User Preferences for Specified User
            var preference = (from pref in _db
                              where pref.userid == userid && pref.truckname == truckName
                              select pref
                              ).FirstOrDefault();

            return preference;
        }
        public int UpdatePreference(string userid, string truckname, string vote)
        {
            // Initialize newPreference to new vote
            int newPreference = Int32.Parse(vote);

            // Obtain User Preferences
            var allPreferences = (from pref in _db
                                  select pref);

            var userPreferences = (from pref in allPreferences
                                   where pref.userid == userid && pref.truckname == truckname
                                   select pref);

            // If no prior preference, write a new one
            if (userPreferences.Count() < 1)
            {
                Preference.PreferenceData newpref = new Preference.PreferenceData();

                newpref.Id = allPreferences.Count();
                newpref.userid = userid;
                newpref.truckname = truckname;
                newpref.preference = vote == "1" ? 1 : 2;

                _db.Add(newpref);
            }
            else
            {
                // Otherwise we need to change an existing preference
                foreach (var oldpref in userPreferences)
                {
                    // If prior new vote == previous, toggle to 0
                    if (oldpref.preference.ToString() == vote)
                    {
                        oldpref.preference = 0;
                        newPreference = 0;
                    }
                    else
                    {
                        // Otherwise change preference to new
                        oldpref.preference = vote == "1" ? 1 : 2;
                        newPreference = (int)oldpref.preference;
                    }
                }
            }

            return newPreference;
        }
    }
}