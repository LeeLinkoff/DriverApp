using System;

namespace Common
{
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }


        public UserCredentials(string user, string password)
        {
            this.Username = user;
            this.Password = password;
        }
    }
}
