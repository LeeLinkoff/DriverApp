using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class TripStatusHistoryResponse
    {
        public int TripId { get; set; }
        public string TripUniqueKey { get; set; }
        public int TripStatusId { get; set; }
        public string CurrentStatus { get; set; }
        public string ButtonName { get; set; }
        public string ConfirmationMsg { get; set; }
    }
}
