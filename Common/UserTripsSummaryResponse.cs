using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class UserTripsSummaryResponse
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Company { get; set; }
        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
