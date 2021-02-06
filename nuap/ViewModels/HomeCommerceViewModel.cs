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
    using System;

    public class HomeCommerceViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<TicketItemViewModel> tickets;
        private bool isRefreshing;
        private string companyName;
        private string logo;

        public ObservableCollection<TicketItemViewModel> Tickets
        {
            get { return this.tickets; }
            set { SetValue(ref this.tickets, value); }
        }

        public string CompanyName
        {
            get { return this.companyName; }
            set { SetValue(ref this.companyName, value); }
        }

        public string Logo
        {
            get { return this.logo; }
            set { SetValue(ref this.logo, value); }
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

        public ICommand InventoryCommand
        {
            get
            {
                return new RelayCommand(Inventory);
            }
        }

        public ICommand OrdersCommand
        {
            get
            {
                return new RelayCommand(Orders);
            }
        }

        public HomeCommerceViewModel()
        {
            this.apiService = new ApiService();
            this.GetCompanyName();
            this.LoadTickets();
            this.IsRefreshing = false;
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

        private async void GetCompanyName()
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

            var response = await this.apiService.GetCommerceProfile(
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

            this.CompanyName = response.Data.BusinessName;
            this.Logo = "https://thenuap.com/" + response.Data.UrlLogo;
        }

        private async void EditProfile()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CommerceProfile = new CommerceProfileViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CommerceProfilePage());
        }

        private async void Inventory()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Inventory = new InventoryViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InventoryPage());
        }

        private async void Orders()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Orders = new OrdersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new OrdersPage());
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
