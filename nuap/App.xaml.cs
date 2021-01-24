namespace nuap
{
    using Xamarin.Forms;
    using Views;
    using Helpers;
    using ViewModels;
    using System;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
            /*if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
                Console.WriteLine("error");
            } else
            {
                Console.WriteLine("errordeded");
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.User.api_token = Settings.Token;
                mainViewModel.User.role = Settings.UserType;
                mainViewModel.User.phone = Settings.Phone;
                mainViewModel.User.email = Settings.Email;

                mainViewModel.Home = new HomeViewModel();
                MainPage = new HomeTabbedPage();
            }*/
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
