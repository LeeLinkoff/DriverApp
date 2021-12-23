using System;
using System.Collections.Generic;
using System.Text;


namespace Common
{

    public class UserTripsSummaryRequest
    {
        public int UserId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }


        public UserTripsSummaryRequest(int userId, string start, string end)
        {
            this.UserId = userId;
            this.StartDate = start;
            this.EndDate = end;
        }
    }

}
