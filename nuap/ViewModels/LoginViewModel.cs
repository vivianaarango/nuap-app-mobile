namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using Services;
    using Helpers;

    public class LoginViewModel : BaseViewModel
    {
        private ApiService apiService;

        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;

        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu correo electrónico o celular",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu contraseña",
                        "Aceptar"
                    );
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.GetUser(
                "https://thenuap.com",
                this.Email,
                this.Password);

            if (response == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Data == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }
            
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.User = response.Data;
            Settings.Token = response.Data.api_token;
            Settings.UserType = response.Data.role;
            Settings.Phone = response.Data.phone;
            Settings.Email = response.Data.email;

            this.IsEnabled = true;

            if (response.Data.role == "Usuario")
            {
                mainViewModel.Home = new HomeViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeTabbedPage());
            } else
            {
                mainViewModel.HomeCommerce = new HomeCommerceViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeCommerceTabbedPage());
            }
        }
    }
}
