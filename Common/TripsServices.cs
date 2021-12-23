using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace Common
{

    public class TripsServices
    {
        public List<UserTripsSummaryResponse> GetTripsForUser(UserTripsSummaryRequest tripsSummaryRequest)
        {
            List<UserTripsSummaryResponse> trips = new List<UserTripsSummaryResponse>();

            try
            {
                var getTripsForUserTask = Task.Run(async () => { trips = await TripsForUserApi(tripsSummaryRequest); });
                getTripsForUserTask.Wait();
            }
            catch (Exception ex)
            {

            }

            return trips;
        }

        public UserTripResponse GetTripForUser(UserTripRequest tripRequest)
        {
            UserTripResponse trip = new UserTripResponse();

            try
            {
                var getTripForUserTask = Task.Run(async () => { trip = await TripForUserApi(tripRequest); });
                getTripForUserTask.Wait();
            }
            catch (Exception ex)
            {

            }

            return trip;
        }

        public static TripStatusHistoryResponse UpdateTripStatus(TripStatusHistoryRequest tripRequest)
        {
            TripStatusHistoryResponse trip = new TripStatusHistoryResponse();

            try
            {
                var updateTripStatusTask = Task.Run(async () => { trip = await TripStatusHistoryApi(tripRequest); });
                updateTripStatusTask.Wait();
            }
            catch (Exception ex)
            {

            }

            return trip;
        }

        public static TripPictureAttachmentResponse AttachPictureToTrip(TripPictureAttachmentRequest tripRequest)
        {
            TripPictureAttachmentResponse trip = new TripPictureAttachmentResponse();

            try
            {
                var attachPictureToTripTask = Task.Run(async () => { trip = await TripPictureAttachmentApi(tripRequest); });
                attachPictureToTripTask.Wait();
            }
            catch (Exception ex)
            {

            }

            return trip;
        }


        private static async Task<List<UserTripsSummaryResponse>> TripsForUserApi(UserTripsSummaryRequest tripsSummaryRequest)
        {
            List<UserTripsSummaryResponse> trips = new List<UserTripsSummaryResponse>();

            try
            {
                string jsonContent = JsonConvert.SerializeObject(tripsSummaryRequest);  //debugging purposes

                HttpClient client = new HttpClient();
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    trips = JsonConvert.DeserializeObject<List<UserTripsSummaryResponse>>(data);
                }
            }
            catch (Exception ex)
            {

            }

            return trips;
        }

        private static async Task<UserTripResponse> TripForUserApi(UserTripRequest tripRequest)
        {
            UserTripResponse trip = null;

            try
            {
                string jsonContent = JsonConvert.SerializeObject(tripRequest);  //debugging purposes

                HttpClient client = new HttpClient();
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    trip = JsonConvert.DeserializeObject<UserTripResponse>(data);
                }
            }
            catch (Exception ex)
            {

            }

            return trip;
        }

        private static async Task<TripStatusHistoryResponse> TripStatusHistoryApi(TripStatusHistoryRequest tripRequest)
        {
            TripStatusHistoryResponse trip = null;

            try
            {
                string jsonContent = JsonConvert.SerializeObject(tripRequest);  //debugging purposes

                HttpClient client = new HttpClient();
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    trip = JsonConvert.DeserializeObject<TripStatusHistoryResponse>(data);
                }
            }
            catch (Exception ex)
            {

            }

            return trip;
        }

        private static async Task<TripPictureAttachmentResponse> TripPictureAttachmentApi(TripPictureAttachmentRequest tripRequest)
        {
            TripPictureAttachmentResponse trip = null;

            try
            {
                string jsonContent = JsonConvert.SerializeObject(tripRequest);  //debugging purposes

                HttpClient client = new HttpClient();
                MultipartFormDataContent formData = new MultipartFormDataContent();

                

                formData.Add(new ByteArrayContent(tripRequest.ImageData, 0, tripRequest.ImageData.Length), "file", DateTime.Now.ToString().Replace("/","-") + ".jpg");
                formData.Add(new StringContent(tripRequest.TripId.ToString()), "tripId");
                formData.Add(new StringContent(tripRequest.TripUniqueKey), "uniqueKey");

                HttpResponseMessage response = await client.PostAsync("", formData);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    trip = JsonConvert.DeserializeObject<TripPictureAttachmentResponse>(data);
                }
            }
            catch (Exception ex)
            {

            }

            return trip;
        }
    }

}
