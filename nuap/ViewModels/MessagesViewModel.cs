namespace nuap.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;
    using Services;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    public class MessagesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Message> messages;
        private bool isRefreshing;
        private string message;

        public Ticket Ticket
        {
            get;
            set;
        }

        public ObservableCollection<Message> Messages
        {
            get { return this.messages; }
            set { SetValue(ref this.messages, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string Message
        {
            get { return this.message; }
            set { SetValue(ref this.message, value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMessages);
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return new RelayCommand(SendMessage);
            }
        }

        public MessagesViewModel(Ticket ticket)
        {
            this.Ticket = ticket;
            this.apiService = new ApiService();
            this.LoadMessages();
        }

        private async void LoadMessages()
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

            var response = await this.apiService.GetTicketMessages(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Ticket.Id);

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

            var list = (List<Message>)response.Data;
            List<Message> newList = new List<Message>();

            foreach (var item in list)
            {
                if (item.SenderType == "Usuario" || item.SenderType == "Comercio")
                {
                    item.Color = "#0274bc";
                    item.FontColor = "White";
                } else
                {
                    item.Color = "White";
                    item.FontColor = "#3a3a3a";
                }
                newList.Add(item);
            }

            this.Messages = new ObservableCollection<Message>(newList);
            this.IsRefreshing = false;
        }

        private async void SendMessage()
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No haz ingresado una respuesta para este ticket",
                        "Aceptar"
                    );
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.Message = "";

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.ReplyTicket(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Ticket.Id,
                this.Message);

            if (response == null)
            {
                this.Message = "";

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

            this.Message = "";
            this.LoadMessages();
        }
    }
}
