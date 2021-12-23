using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class TripStatusHistoryRequest
    {
        public int TripId { get; set; }
        public int TripStatusId { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }


        public TripStatusHistoryRequest(int tripId, int tripStatusId, string status, string location)
        {
            this.TripId = tripId;
            this.TripStatusId = tripStatusId;
            this.Status = status;
            this.Location = location;
        }
    }
}
