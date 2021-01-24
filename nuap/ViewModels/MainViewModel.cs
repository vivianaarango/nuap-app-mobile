namespace nuap.ViewModels
{
    using Models;
    using System.Collections.Generic;

    public class MainViewModel
    {
        public LoginViewModel Login
        {
            get;
            set;
        }

        public HomeViewModel Home
        {
            get;
            set;
        }

        public SupportViewModel Support
        {
            get;
            set;
        }

        public MessagesViewModel Messages
        {
            get;
            set;
        }

        public CreateTicketViewModel CreateTicket
        {
            get;
            set;
        }

        public UserProfileViewModel UserProfile
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public List<Ticket> TicketList
        {
            get;
            set;
        }

        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
    }
}
