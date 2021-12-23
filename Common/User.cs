using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class User
    {
        public bool IsAuthenticated { get; set; }

        public string Username { get; set; }

        public int RoleId { get; set; }

        public int UserId { get; set; }

        public bool IsResetRequired { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
