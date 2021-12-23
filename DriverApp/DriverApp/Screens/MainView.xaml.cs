using System;

using Xamarin.Essentials;
using Xamarin.Forms;

using Common;
using System.Threading.Tasks;

namespace DriverApp.Screens
{
    public partial class MainView : ContentPage
    {

        public MainView()
        {
            InitializeComponent();

            // Attach event handler "Connectivity_ConnectivityChanged" to Connectivitychanged event of Connectivity object
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }


        async void OnLoginClick(object sender, EventArgs args)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                Device.BeginInvokeOnMainThread(() => {
                    loginBtn.BackgroundColor = Color.Green;
                    loginBtn.Text = "Logging in";
                    loginMsg.Text = "Please wait...";
                });

                await Task.Run(() =>
                {
                    UserCredentials userCredentials = new UserCredentials(UserIdEntry.Text, PasswordEntry.Text);
                    if (ApplicationServices.ValidUser(userCredentials))
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            Application.Current.MainPage = new DriverApp.Screens.TripsSummaryView();
                        });
                        
                    }
                });

                Device.BeginInvokeOnMainThread(() => {
                    loginBtn.BackgroundColor = Color.Gray;
                    loginBtn.Text = "Login";
                    loginMsg.Text = "Login Failed";
                });

            }
        }


        void OnExitClick(object sender, EventArgs args)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            DriverApp.App.Current.Quit();   //NOT WORKING
        }



        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //stub
        }

    }
}
