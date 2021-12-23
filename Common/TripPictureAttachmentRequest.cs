using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class TripPictureAttachmentRequest
    {
        public int TripId { get; set; }
        public string TripUniqueKey { get; set; }
        public byte[] ImageData { get; set; }


        public TripPictureAttachmentRequest(int tripId, string tripUniqueKey, byte[] imageData)
        {
            this.TripId = tripId;
            this.TripUniqueKey = tripUniqueKey;
            this.ImageData = imageData;
        }
    }
}
