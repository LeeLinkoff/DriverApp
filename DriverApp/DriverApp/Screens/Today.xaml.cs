using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DriverApp.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Today : ContentPage
    {

        public Today()
        {
            InitializeComponent();

            ShowTodaysTrips();
        }


        private void ShowTodaysTrips()
        {
            string  startDateToday = DateTime.Today.ToString();
            string endDateToday = DateTime.Today.AddDays(1).ToString();

            TripGrid tripGrid = new TripGrid();
            tripGrid.PopulateGridWithTrips(tripsListToday, Preferences.Get("UserId", 0), startDateToday, endDateToday);
        }

        private void Button_Refresh(object sender, EventArgs e)
        {
            //Get data from REST API again
            ShowTodaysTrips();
        }

    }
}