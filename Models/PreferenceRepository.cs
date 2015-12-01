using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    /// <summary>
    ///    void CreateNewPreference(Preference preferenceToCreate);
    ///    void UpdatePreference(string userid, string truckname, int newPreference);
    ///    IEnumerable<Preference> GetAllPreferences();
    ///    IEnumerable<Preference> GetPreferencesForUser(string userid);
    ///    int SaveChanges();
    /// </summary>
    public class DB_PreferenceRepository : IPreferenceRepository
    {
        // NextMile DB Context
        private static NextMileDB1DataContext NextMileDB = new NextMileDB1DataContext();

        public IEnumerable<Preference.PreferenceData> GetAllPreferences()
        {
            // Obtain User Preferences
            var preferences = (from pref in NextMileDB.UserProfileTest1s
                               select new Preference.PreferenceData {
                                   Id = pref.Id,
                                   userid = pref.userid,
                                   truckname = pref.truckname,
                                   preference = pref.preference
                               }
                               ).AsEnumerable();

            return preferences;
        }

        public IEnumerable<Preference.PreferenceData> GetPreferencesForUser(string userid)
        {
            // Obtain User Preferences for Specified User
            var preferences = (from pref in NextMileDB.UserProfileTest1s
                               where pref.userid == userid
                               select new Preference.PreferenceData {
                                   Id = pref.Id,
                                   userid = pref.userid,
                                   truckname = pref.truckname,
                                   preference = pref.preference
                               }
                               ).AsEnumerable();

            return preferences;
        }

        public Preference.PreferenceData GetPreferenceForUserAndTruck(string userid, string truckName)
        {
            // Obtain User Preferences for Specified User
            var preference = (from pref in NextMileDB.UserProfileTest1s
                              where pref.userid == userid && pref.truckname == truckName
                              select new Preference.PreferenceData
                              {
                                  Id = pref.Id,
                                  userid = pref.userid,
                                  truckname = pref.truckname,
                                  preference = pref.preference
                              }
                               ).FirstOrDefault();

            return preference;
        }
        public int UpdatePreference(string userid, string truckname, string vote)
        {
            // Initialize newPreference to new vote
            int newPreference = Int32.Parse(vote);

            // Obtain User Preferences
            var allPreferences = (from pref in NextMileDB.UserProfileTest1s
                                  select pref);

            var userPreferences = (from pref in allPreferences
                                   where pref.userid == userid && pref.truckname == truckname
                                   select pref);

            // If no prior preference, write a new one
            if (userPreferences.Count() < 1)
            {
                UserProfileTest1 newpref = new UserProfileTest1();
                newpref.Id = allPreferences.Count();
                newpref.userid = userid;
                newpref.truckname = truckname;
                newpref.preference = vote == "1" ? 1 : 2;

                NextMileDB.UserProfileTest1s.InsertOnSubmit(newpref);
                NextMileDB.SubmitChanges();
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
                    NextMileDB.SubmitChanges();
                }
            }

            return newPreference;
        }
    }
}