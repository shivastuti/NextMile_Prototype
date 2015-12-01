using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public interface IPreferenceRepository
    {
        int UpdatePreference(string userid, string truckname, string newPreference);
        IEnumerable<Preference.PreferenceData> GetAllPreferences();
        IEnumerable<Preference.PreferenceData> GetPreferencesForUser(string userid);
        Preference.PreferenceData GetPreferenceForUserAndTruck(string userid, string truckName);
    }
}