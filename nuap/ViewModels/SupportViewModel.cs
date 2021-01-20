namespace nuap.ViewModels
{
    using System.Collections.ObjectModel;
    using Models;
    using Services;
    using Xamarin.Forms;
    using System.Linq;
    using System.Collections.Generic;

    public class SupportViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Ticket> tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get { return this.tickets; }
            set { SetValue(ref this.tickets, value); }
        }

        public SupportViewModel()
        {
            this.apiService = new ApiService();
            this.LoadTickets();
        }

        private async void LoadTickets()
        {
            /*var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }*/


            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.GetTickets(
                "https://4cd1f30e257e.ngrok.io",
                mainViewModel.User.api_token);

            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error 2",
                    "Aceptar");
                return;
            }


            /*if (response.Data == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Data.First().Description,
                    "Aceptar");
            return;*/

            //var list = (List Ticket>)response.Data;
            //this.Tickets = new ObservableCollection<Ticket>(list); 
        }
    }
}
