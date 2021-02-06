namespace nuap.ViewModels
{
    using Services;
    using System.Linq;
    using Xamarin.Forms;

    public class CommerceProfileViewModel : BaseViewModel
    {
        private ApiService apiService;

        private string email;
        private string phone;
        private string companyName;
        private string commission;
        private string shippingCost;
        private bool isEnabled;
        private string logo;

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

        public string Commission
        {
            get { return this.commission; }
            set { SetValue(ref this.commission, value); }
        }

        public string ShippingCost
        {
            get { return this.shippingCost; }
            set { SetValue(ref this.shippingCost, value); }
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

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public CommerceProfileViewModel()
        {
            this.apiService = new ApiService();
            this.GetCommerceProfile();
            this.IsEnabled = true;
        }

        private async void GetCommerceProfile()
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

            this.Email = response.Data.Email;
            this.Phone = response.Data.Phone;
            this.CompanyName = response.Data.BusinessName;
            this.Commission = response.Data.Commission;
            this.ShippingCost = response.Data.ShippingCost;
            this.Logo = "https://thenuap.com/" + response.Data.UrlLogo;
        }
    }
}
