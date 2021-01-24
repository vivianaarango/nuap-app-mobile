namespace nuap.ViewModels
{
    using System.Collections.ObjectModel;
    using Models;
    using Services;
    using Xamarin.Forms;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;

    public class HomeViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<TicketItemViewModel> tickets;
        private bool isRefreshing;
        private string userName;

        public ObservableCollection<TicketItemViewModel> Tickets
        {
            get { return this.tickets; }
            set { SetValue(ref this.tickets, value); }
        }

        public string UserName
        {
            get { return this.userName; }
            set { SetValue(ref this.userName, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadTickets);
            }
        }

        public ICommand CreateTicketCommand
        {
            get
            {
                return new RelayCommand(CreateTicket);
            }
        }

        public ICommand ProfileCommand
        {
            get
            {
                return new RelayCommand(EditProfile);
            }
        }

        public HomeViewModel()
        {
            this.apiService = new ApiService();
            this.GetUserName();
            this.LoadTickets();
        }

        private async void CreateTicket()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CreateTicket = new CreateTicketViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CreateTicketPage());
        }

        private async void LoadTickets()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }


            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.GetTickets(
                mainViewModel.User.api_token,
                "https://thenuap.com");

            if (response == null)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Data == null)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            mainViewModel.TicketList = (List<Ticket>)response.Data;
            this.Tickets = new ObservableCollection<TicketItemViewModel>(ToTicketViewModel());
            this.IsRefreshing = false;
        }

        private async void GetUserName()
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

            this.UserName = response.Data.Name;
        }

        private async void EditProfile()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.UserProfile = new UserProfileViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new UserProfilePage());
        }

        private IEnumerable<TicketItemViewModel> ToTicketViewModel()
        {
            return MainViewModel.GetInstance().TicketList.Select(t => new TicketItemViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
                UserType = t.UserType,
                Issues = t.Issues,
                InitDate = t.InitDate,
                FinishDate = t.FinishDate,
                Status = t.Status,
                Description = t.Description
            });
        }
    }
}
