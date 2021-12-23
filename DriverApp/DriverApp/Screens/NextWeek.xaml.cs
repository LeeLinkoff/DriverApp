using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DriverApp.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextWeek : ContentPage

    {
        public NextWeek()
        {
            InitializeComponent();

            ShowNextWeekTrips();
        }


        private void ShowNextWeekTrips()
        {
            string startDateToday = DateTime.Today.AddDays(1).ToString();
            string endDateToday = DateTime.Today.AddDays(7).ToString();

            TripGrid tripGrid = new TripGrid();
            tripGrid.PopulateGridWithTrips(tripsListNextWeek, Preferences.Get("UserId", 0), startDateToday, endDateToday);
        }

        private void Button_Refresh(object sender, EventArgs e)
        {
            //Get data from REST API again
            ShowNextWeekTrips();
        }

    }
}