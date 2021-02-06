namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class OrderItemViewModel : Order
    {
        public ICommand GetProductsCommand
        {
            get
            {
                return new RelayCommand(GetProducts);
            }
        }

        private void GetProducts()
        {
           /* var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.ProductCommerce = new ProductCommerceViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new ProductCommercePage());*/
        }
    }
}
