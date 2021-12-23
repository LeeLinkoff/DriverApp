using System;


namespace Common
{
    public class UserTripResponse
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public string Company { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public string Freight { get; set; }
        public string Packaging { get; set; }
        public string Driver { get; set; }

        public int TripStatusId { get; set; }

        public string CurrentStatus { get; set; }

        public string ButtonName { get; set; }

        public string ConfirmationMsg { get; set; }
    }
}
