 namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class TicketItemViewModel : Ticket
    {
        public ICommand GetMessagesCommand
        {
            get
            {
                return new RelayCommand(GetMessages);
            }
        }

        private async void GetMessages()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Messages = new MessagesViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new MessagesPage());
        }
    }
}
