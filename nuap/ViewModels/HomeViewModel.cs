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
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;

    public class HomeViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<TicketItemViewModel> tickets;
        private bool isRefreshing;

        public ObservableCollection<TicketItemViewModel> Tickets
        {
            get { return this.tickets; }
            set { SetValue(ref this.tickets, value); }
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

        public HomeViewModel()
        {
            this.apiService = new ApiService();
            this.LoadTickets();
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


            /*var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Authorization", "Bearer " + mainViewModel.User.api_token);
            client.BaseAddress = new Uri("https://thenuap.com");
            var response = await client.GetAsync("/api/users/ticket");
            var resultJSON = await response.Content.ReadAsStringAsync();
            //var resultJSON = "{\r\n    \"data\": [\r\n        {\r\n            \"id\": 19,\r\n            \"user_id\": 37,\r\n            \"user_type\": \"Usuario\",\r\n            \"issues\": \"Error compra\",\r\n            \"init_date\": \"2021-01-14 22:30:07\",\r\n            \"finish_date\": null,\r\n            \"status\": \"Pendiente Administrador\",\r\n            \"description\": \"Ayuda para realizar mi compra\"\r\n        },\r\n        {\r\n            \"id\": 20,\r\n            \"user_id\": 37,\r\n            \"user_type\": \"Usuario\",\r\n            \"issues\": \"No puedo realizar el pago\",\r\n            \"init_date\": \"2021-01-17 09:50:47\",\r\n            \"finish_date\": null,\r\n            \"status\": \"Pendiente Administrador\",\r\n            \"description\": \"Ayuda para poder realizar el pago de mi compra\"\r\n        }\r\n    ],\r\n    \"meta\": {\r\n        \"pagination\": {\r\n            \"total\": 2,\r\n            \"count\": 2,\r\n            \"per_page\": 2,\r\n            \"current_page\": 1,\r\n            \"total_pages\": 1,\r\n            \"links\": []\r\n        }\r\n    }\r\n}";
            var result = JsonConvert.DeserializeObject<TicketResponse>(resultJSON);

            await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    resultJSON,
                    "Aceptar");
            return;*/

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

            //var list = (List<Ticket>)response.Data;
            mainViewModel.TicketList = (List<Ticket>)response.Data;
            this.Tickets = new ObservableCollection<TicketItemViewModel>(ToTicketViewModel());
            this.IsRefreshing = false;
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
