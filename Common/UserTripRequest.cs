using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    public class UserTripRequest
    {
        public int TripId { get; set; }
        public string UniqueKey { get; set; }


        public UserTripRequest(int tripId, string key)
        {
            this.TripId = tripId;
            this.UniqueKey = key;
        }
    }

}
