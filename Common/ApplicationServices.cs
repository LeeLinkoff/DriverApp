using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Newtonsoft.Json;

using Xamarin.Essentials;


namespace Common
{
    public static class ApplicationServices
    {

        public static Location GetGpsCoordinates()
        {
            Location location = null;

            try
            {
                var locationTask = Task.Run(async () => { location = await GetCurrentLocation(); });
                locationTask.Wait();
            }
            catch (Exception ex)
            {

            }

            return location;
        }

        public static bool ValidUser(UserCredentials user)
        {
            bool validated = false;

            try
            {
                var validateUserTask = Task.Run(async () => { validated = await ValidateUser(user); });
                validateUserTask.Wait();
            }
            catch(Exception ex)
            {

            }

            return validated;
        }

        private static async Task<Location> GetCurrentLocation()
        {
            CancellationTokenSource cts;
            Location location = null;

            try
            {
               
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(30));
                cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    throw new Exception("Location returned as null");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return location;
        }

        private static async Task<bool> ValidateUser(UserCredentials userCredentials)
        {
            bool validated = false;

            try
            {
                userCredentials.Password = GetHashSha256(userCredentials.Password);

                string jsonContent = JsonConvert.SerializeObject(userCredentials);  //debugging purposes

                HttpClient client = new HttpClient();  
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(data);

                    if (user.IsAuthenticated)
                    {
                        Preferences.Set("IsAuthenticated", user.IsAuthenticated);
                        Preferences.Set("Username", user.Username);
                        Preferences.Set("RoleId", user.RoleId);
                        Preferences.Set("UserId", user.UserId);
                        Preferences.Set("IsResetRequired", user.IsResetRequired);
                        Preferences.Set("Phone", user.Phone);
                        Preferences.Set("Email", user.Email);

                        validated = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return validated;
        }


        private static string GetHashSha256(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString.ToUpper();
        }
    }
}
