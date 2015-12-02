using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class MockCurrentUser : ICurrentUser
    {
        Dictionary<string,string> _state = new Dictionary<string,string>();

        public MockCurrentUser()
        {
            // Initialize with null userid
            _state.Add("uid", null);
        }
        public virtual string UserId()
        {
            return _state["uid"];
        }

        public void setUserId(string userId)
        {
            SetState("uid", userId);
        }

        public void SetState(string key, string value)
        {
            if (! _state.ContainsKey(key))
            {
                _state.Add(key, value);
            }
            else
            {
                _state[key] = value;
            }
        }

    }
}