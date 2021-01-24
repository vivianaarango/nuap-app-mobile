namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using Services;

    public class CreateTicketViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string issues;
        private string message;
        private bool isEnabled;

        public string Issues
        {
            get { return this.issues; }
            set { SetValue(ref this.issues, value); }
        }
        public string Message
        {
            get { return this.message; }
            set { SetValue(ref this.message, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public CreateTicketViewModel()
        {
            this.apiService = new ApiService();
            this.isEnabled = true;
        }

        public ICommand NewTicketCommand
        {
            get
            {
                return new RelayCommand(NewTicket);
            }
        }

        private async void NewTicket()
        {
            if (string.IsNullOrEmpty(this.Issues))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar un asunto para poder realizar la solicitud",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Message))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar un mensaje para poder realizar la solicitud",
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

            var response = await this.apiService.CreateTicket(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Issues,
                this.Message);

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
                this.Message = "";

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            mainViewModel.Home = new HomeViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new HomeTabbedPage());
        }
    }
}
