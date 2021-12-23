using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Common;
using System.Threading;

namespace DriverApp.Screens
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripsSummaryView : TabbedPage
    {
        public TripsSummaryView()
        {
            InitializeComponent();

            CurrentPage = Children[1];
        }
    }

}