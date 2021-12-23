using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DriverApp.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LastWeek : ContentPage
    {

        public LastWeek()
        {
            InitializeComponent();

            ShowLastWeekTrips();
        }


        private void ShowLastWeekTrips()
        {
            string startDateToday = DateTime.Today.AddDays(-8).ToString();
            string endDateToday = DateTime.Today.ToString();

            TripGrid tripGrid = new TripGrid();
            tripGrid.PopulateGridWithTrips(tripsListLastWeek, Preferences.Get("UserId", 0), startDateToday, endDateToday);
        }

        private void Button_Refresh(object sender, EventArgs e)
        {
            //Get data from REST API again
            ShowLastWeekTrips();
        }

    }
}