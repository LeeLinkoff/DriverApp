using Common;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DriverApp
{
    class TripGrid
    {

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
                    new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star)  }
                }
            };


            return grid;
        }

        private Frame AddFramedButton(string buttonText, int tripId, string tripKey)
        {
            Frame frame = new Frame();
            frame.BorderColor = Color.Black;
            frame.CornerRadius = 0;
            frame.Padding = 10;

            Button button = new Button();
            button.BackgroundColor = Color.Blue;
            button.Text = buttonText;
            button.TextColor = Color.White;
            button.FontSize = 12;
            button.HeightRequest = 30;
            button.Padding = 1;

            button.Clicked += delegate (object sender, EventArgs e) { Button_Clicked(sender, e, tripId, tripKey); };
            frame.Content = button;

            return frame;
        }

        private Frame AddFramedLabel(string text)
        {
            Frame frame = new Frame();
            frame.BorderColor = Color.Black;
            frame.CornerRadius = 2;
            frame.Padding = 12;


            Label label = new Label();
            label.Text = text;
            label.FontSize = 16;
            frame.Content = label;

            return frame;
        }

        private void Button_Clicked(object sender, EventArgs e, int tripId, string tripKey)
        {
            Application.Current.MainPage = new Screens.TripDetails(tripId, tripKey);
        }


        private void DrawGridRow(Grid grid, UserTripsSummaryResponse trip, int rowNum)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedButton("Details", trip.Id, trip.UniqueKey), 0, rowNum);
            grid.Children.Add(AddFramedLabel(trip.UniqueKey), 1, rowNum++);

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel("Company"), 0, rowNum);
            grid.Children.Add(AddFramedLabel(trip.Company), 1, rowNum++);

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel("Pickup Date"), 0, rowNum);
            grid.Children.Add(AddFramedLabel(trip.PickupDate.ToString("dddd, dd MMMM yyyy hh:mm tt")), 1, rowNum++);

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel("Pickup Address"), 0, rowNum);
            grid.Children.Add(AddFramedLabel(trip.PickupAddress), 1, rowNum++);

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel("Delivery Date"), 0, rowNum);
            if (trip.DeliveryDate == DateTime.MinValue)
            {
                grid.Children.Add(AddFramedLabel("Right After Pickup"), 1, rowNum++);
            }
            else
            {
                grid.Children.Add(AddFramedLabel(trip.DeliveryDate.ToString("dddd, dd MMMM yyyy hh:mm tt")), 1, rowNum++);
            }
                

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(AddFramedLabel("Delivery Address"), 0, rowNum);
            grid.Children.Add(AddFramedLabel(trip.DeliveryAddress), 1, rowNum++);

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
        }


        //external method - will be called from outside the class
        public void PopulateGridWithTrips(StackLayout stackLayout, int userId, string startDate, string endDate)
        {
            UserTripsSummaryRequest tripsRequest = new UserTripsSummaryRequest(userId, startDate, endDate);

            TripsServices tripsService = new TripsServices();

            List<UserTripsSummaryResponse> tripsResponse = tripsService.GetTripsForUser(tripsRequest);

            Grid grid = DefineGrid();
            int rowNum = 0;
            foreach (UserTripsSummaryResponse trip in tripsResponse)
            {
                DrawGridRow(grid, trip, rowNum);
                rowNum += 7;
            }

            stackLayout.Children.Clear();
            stackLayout.ForceLayout();

            stackLayout.Children.Add(grid);
        }

    }
}
