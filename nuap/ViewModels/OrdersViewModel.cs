namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Models;

    public class OrdersViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<OrderItemViewModel> orders;
        private bool isRefreshing;

        public ObservableCollection<OrderItemViewModel> Orders
        {
            get { return this.orders; }
            set { SetValue(ref this.orders, value); }
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
                return new RelayCommand(LoadOrders);
            }
        }

        public OrdersViewModel()
        {
            this.apiService = new ApiService();
            this.LoadOrders();
        }


        private async void LoadOrders()
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

            var response = await this.apiService.GetOrders(
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

            var list = (List<Order>)response.Data;
            List<Order> newList = new List<Order>();

            foreach (var item in list)
            {
                if (item.Rating == 1)
                {
                    item.RatingImage = "ic_start_one";
                }

                if (item.Rating == 2)
                {
                    item.RatingImage = "ic_start_two";
                }

                if (item.Rating == 3)
                {
                    item.RatingImage = "ic_start_three";
                }

                if (item.Rating == 4)
                {
                    item.RatingImage = "ic_start_four";
                }

                if (item.Rating == 5)
                {
                    item.RatingImage = "ic_start_five";
                }

                if (item.Status == "Iniciado" || item.Status == "Aceptado")
                {
                    item.StatusColor = "#ffc33a";
                }

                if (item.Status == "Alistamiento" || item.Status == "Circulación")
                {
                    item.StatusColor = "#a3cafa";
                }

                if (item.Status == "Entregado")
                {
                    item.StatusColor = "#95dc4e";
                }

                if (item.Status == "Cancelado")
                {
                    item.StatusColor = "#f78d8d";
                }
            
                newList.Add(item);
            }

            mainViewModel.OrderList = newList;
            this.Orders = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel());
            this.IsRefreshing = false;
        }
         
        private IEnumerable<OrderItemViewModel> ToOrderViewModel()
        {
            return MainViewModel.GetInstance().OrderList.Select(o => new OrderItemViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                UserType = o.UserType,
                CancelReason = o.CancelReason,
                ClientId = o.ClientId,
                ClientType = o.ClientType,
                TotalProducts = o.TotalProducts,
                TotalAmount = o.TotalAmount,
                DeliveryAmount = o.DeliveryAmount,
                TotalDiscount = o.TotalDiscount,
                Total = o.Total,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                Rating = o.Rating,
                RatingImage = o.RatingImage,
                StatusColor = o.StatusColor
            });
        }
    }
}