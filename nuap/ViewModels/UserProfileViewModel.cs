namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class UserProfileViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string email;
        private string phone;
        private string name;
        private bool isEnabled;


        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Phone
        {
            get { return this.phone; }
            set { SetValue(ref this.phone, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public UserProfileViewModel()
        {
            this.apiService = new ApiService();
            this.GetUserProfile();
            this.IsEnabled = true;
        }

        public ICommand UpdateProfileCommand
        {
            get
            {
                return new RelayCommand(UpdateProfile);
            }
        }

        private async void UpdateProfile()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "El campo email esta vacio",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "El campo teléfono esta vacio",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "El campo nombre esta vacio",
                        "Aceptar"
                    );
                return;
            }

            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.EditProfile(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Name,
                this.Email,
                this.phone);

            if (response == null)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Errors != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                    "",
                    "Hemos actualizado tu perfil exitosamente !",
                    "Aceptar");
            return;
        }

        private async void GetUserProfile()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.GetUserProfile(
                mainViewModel.User.api_token,
                "https://thenuap.com");

            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Data == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            this.Email = response.Data.Email;
            this.Phone = response.Data.Phone;
            this.Name = response.Data.Name;
        }
    }
}
