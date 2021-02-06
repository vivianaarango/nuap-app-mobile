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
    using Views;
    using System;

    public class ProductCommerceViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<ProductCommerceItemViewModel> products;
        private bool isRefreshing;
        private string categoryName;
        private string filter;

        public Category Category
        {
            get;
            set;
        }

        public string Filter
        {
            get { return this.filter; }
            set 
            { 
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        public ObservableCollection<ProductCommerceItemViewModel> Products
        {
            get { return this.products; }
            set { SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string CategoryName
        {
            get { return this.categoryName; }
            set { SetValue(ref this.categoryName, value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        public ICommand CreateProductCommand
        {
            get
            {
                return new RelayCommand(CreateProduct);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        
        public ProductCommerceViewModel(Category category)
        {
            this.Category = category;
            this.CategoryName = this.Category.Name;
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Products = new ObservableCollection<ProductCommerceItemViewModel>(ToProductCommerceViewModel());
            }
            else
            {
                this.Products = new ObservableCollection<ProductCommerceItemViewModel>
                    (ToProductCommerceViewModel().Where(p => p.Name.ToLower().Contains(this.Filter.ToLower()) ||
                                                             p.Brand.ToLower().Contains(this.Filter.ToLower())));
            }
        }

        private async void CreateProduct()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CategoryProductID = this.Category.Id;
            mainViewModel.CreateProduct = new CreateProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CreateProductPage());
        }

        private async void LoadProducts()
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

            var response = await this.apiService.GetProductsCommerce(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Category.Id);

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

            var list = (List<Product>)response.Data;
            List<Product> newList = new List<Product>();

            foreach (var item in list)
            {
                if (item.Status == 1)
                {
                    item.StatusColor = "#21b441";
                    item.StatusText = "Aprobado";
                }
                else
                {
                    item.StatusColor = "#f24f4c";
                    item.StatusText = "Pendiente";
                }
                newList.Add(item);
            }

            mainViewModel.ProductList = newList;
            this.Products = new ObservableCollection<ProductCommerceItemViewModel>(ToProductCommerceViewModel());
            this.IsRefreshing = false;
        }

        private IEnumerable<ProductCommerceItemViewModel> ToProductCommerceViewModel()
        {
            return MainViewModel.GetInstance().ProductList.Select(p => new ProductCommerceItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                Sku = p.Sku,
                Brand = p.Brand,
                Description = p.Description,
                Status = p.Status,
                IsFeatured = p.IsFeatured,
                Stock = p.Stock,
                Weight = p.Weight,
                Length = p.Length,
                Width = p.Width,
                Height = p.Height,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                SpecialPrice = p.SpecialPrice,
                HasSpecialPrice = p.HasSpecialPrice,
                Image = "https://thenuap.com/"+p.Image,
                Position = p.Position,
                StatusText = p.StatusText,
                StatusColor = p.StatusColor
            });
        }
    }
}