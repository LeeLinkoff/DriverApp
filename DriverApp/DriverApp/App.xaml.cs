using Xamarin.Forms;
using Common;

namespace DriverApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // NEEDED - draws the initial "page"/screen
            MainPage = new DriverApp.Screens.MainView();
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
