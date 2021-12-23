using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Common;

using Plugin.Media;
using Plugin.Media.Abstractions;


namespace DriverApp.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripDetails : ContentPage
    {
        private int TripId { get; set; }
        private string TripKey { get; set; }
        private int TripStatusId { get; set; }
        private string ConfirmationMsg { get; set; }

        public TripDetails(int tripId, string tripKey)
        {
            InitializeComponent();

            Tuple<int,string> response = PopulateGridWithTrip(this.tripDetails, tripId, tripKey, StatusButton);

            this.IsBusy = false;

            TripId = tripId;
            TripKey = tripKey;
            TripStatusId = response.Item1;
            ConfirmationMsg = response.Item2;
        }


        private void Button_Back(object sender, EventArgs e)
        {
            Application.Current.MainPage = new DriverApp.Screens.TripsSummaryView();
        }

        private void Button_Refresh(object sender, EventArgs e)
        {
            //Get data from REST API again
            PopulateGridWithTrip(this.tripDetails, TripId, TripKey, StatusButton);
        }

        async void Button_TakePicture(object sender, EventArgs e)
        {
            byte[] imageBytes = await TakePicture();

            Device.BeginInvokeOnMainThread(() => {
                TakePictureButton.BackgroundColor = Color.Red;
                TakePictureButton.Text = "Please wait..";
                this.activityIndicator.IsVisible = true;
                this.activityIndicator.IsRunning = true;
            });

            TripPictureAttachmentRequest picture = new TripPictureAttachmentRequest(this.TripId, this.TripKey, imageBytes);


            TripPictureAttachmentResponse response = TripsServices.AttachPictureToTrip(picture);

            await DisplayAlert("Image Upload Result", response.Message, "Ok");

            Device.BeginInvokeOnMainThread(() => {
                TakePictureButton.BackgroundColor = Color.Blue;
                TakePictureButton.Text = "Take Picture";
                this.activityIndicator.IsVisible = false;
                this.activityIndicator.IsRunning = false;
            });
        }

        async void Button_Status(object sender, EventArgs e)
        {
            string location;

            
            if (TripStatusId < 8)
            {
                TripStatusId += 1;

                //Change status
                bool answer = await DisplayAlert("Confirmation", ConfirmationMsg, "Yes", "No");

                if (answer)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        this.activityIndicator.IsVisible = true;
                        this.activityIndicator.IsRunning = true;
                        this.StatusButton.Text = "Please wait..";
                    });

                    if (TripStatusId > 0 && TripStatusId < 9)
                    {
                        location = await CurrentGpsCoordinates();
                        TripStatusHistoryRequest request = new TripStatusHistoryRequest(TripId, TripStatusId, "", location);
                        TripStatusHistoryResponse response = TripsServices.UpdateTripStatus(request);
                    }
                }
            }
            else
            {
                await DisplayAlert("Completed", ConfirmationMsg, "Ok");
            }
            Device.BeginInvokeOnMainThread(() => {
                Tuple<int, string> tripDetails = PopulateGridWithTrip(this.tripDetails, TripId, TripKey, StatusButton);
                TripStatusId = tripDetails.Item1;
                ConfirmationMsg = tripDetails.Item2;
                this.activityIndicator.IsVisible = false;
                this.activityIndicator.IsRunning = false;
            });
        }


        async Task<string> CurrentGpsCoordinates()
        {
            CancellationTokenSource cts;
            string gpsCoordinates;
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    gpsCoordinates = location.Latitude + "," + location.Longitude;
                }
                else
                {
                    gpsCoordinates = null;
                }

                return gpsCoordinates;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
        }

        async Task<byte[]> TakePicture()
        {
            MediaFile file = null;
            byte[] imageBytes = null;

            try
            {
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front
                });

                string fileExt = System.IO.Path.GetExtension(file.Path);

                if (file != null && fileExt == ".jpg")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        file.Dispose();
                        imageBytes = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return imageBytes;
        }


        private Grid DefineGrid()
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Margin = 5,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star) }
                }
            };

            return grid;
        }

        private Frame AddFramedLabel(string text)
        {
            Frame frame = new Frame();
            frame.BorderColor = Color.Black;
            frame.CornerRadius = 2;
            frame.Padding = 6;

            Label label = new Label();
            label.Text = text;
            label.FontSize = 14;
            frame.Content = label;

            return frame;
        }

        private void DrawGridRow(Grid grid, string label, string value, int rowNum)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel(label), 0, rowNum);
            grid.Children.Add(AddFramedLabel(value), 1, rowNum++);
        }

        private Tuple<int,string> PopulateGridWithTrip(StackLayout stackLayout, int tripId, string key, Button statusButton)
        {
            UserTripRequest tripRequest = new UserTripRequest(tripId, key);

            TripsServices tripsService = new TripsServices();

            UserTripResponse tripResponse = tripsService.GetTripForUser(tripRequest);

            if (tripResponse != null)
            {
                Grid grid = DefineGrid();

                int rowNum = 0;
                DrawGridRow(grid, "Current Trip Status", tripResponse.CurrentStatus, rowNum++);
                DrawGridRow(grid, "Driver", tripResponse.Driver, rowNum++);
                DrawGridRow(grid, "Trip Number", tripResponse.UniqueKey, rowNum++);

                DrawGridRow(grid, "Company", tripResponse.Company, rowNum++);
                
                DrawGridRow(grid, "Pickup Date", tripResponse.PickupDate.ToString("dddd, dd MMMM yyyy hh:mm tt"), rowNum++);
                DrawGridRow(grid, "Pickup Address", tripResponse.PickupAddress, rowNum++);

                if (tripResponse.DeliveryDate == DateTime.MinValue)
                {
                    DrawGridRow(grid, "Delivery Date", "Right After Pickup", rowNum++);
                }
                else
                {
                    DrawGridRow(grid, "Delivery Date", tripResponse.DeliveryDate.ToString("dddd, dd MMMM yyyy hh:mm tt"), rowNum++);
                }

                DrawGridRow(grid, "Delivery Address", tripResponse.DeliveryAddress, rowNum++);

                DrawGridRow(grid, "Packaging", tripResponse.Packaging, rowNum++);
                DrawGridRow(grid, "Freight", tripResponse.Freight, rowNum++);
                

                statusButton.Text = tripResponse.ButtonName;

                stackLayout.Children.Clear();
                stackLayout.ForceLayout();

                stackLayout.Children.Add(grid);
            }

            return Tuple.Create(tripResponse.TripStatusId, tripResponse.ConfirmationMsg);
        }
    }
}