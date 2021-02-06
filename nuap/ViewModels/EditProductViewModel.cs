namespace nuap.ViewModels
{
    using Services;
    using Models;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using System.Linq;
    using Views;

    public class EditProductViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string name;
        private string sku;
        private string brand;
        private string description;
        private int stock;
        private double weight;
        private double length;
        private double width;
        private double height;
        private double purchasePrice;
        private double price;
        private double specialPrice;
        private int isFeatured;
        private int hasSpecialPrice;
        private bool isEnabled;
        private ImageSource logo;
        private MediaFile file;
        private Product product;

        public ImageSource Logo
        {
            get { return this.logo; }
            set { SetValue(ref this.logo, value); }
        }

        public Product Product
        {
            get { return this.product; }
            set { SetValue(ref this.product, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        public string Sku
        {
            get { return this.sku; }
            set { SetValue(ref this.sku, value); }
        }

        public string Brand
        {
            get { return this.brand; }
            set { SetValue(ref this.brand, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { SetValue(ref this.description, value); }
        }

        public int IsFeatured
        {
            get { return this.isFeatured; }
            set { SetValue(ref this.isFeatured, value); }
        }

        public int HasSpecialPrice
        {
            get { return this.hasSpecialPrice; }
            set { SetValue(ref this.hasSpecialPrice, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public int Stock
        {
            get { return this.stock; }
            set { SetValue(ref this.stock, value); }
        }

        public double Weight
        {
            get { return this.weight; }
            set { SetValue(ref this.weight, value); }
        }

        public double Length
        {
            get { return this.length; }
            set { SetValue(ref this.length, value); }
        }

        public double Width
        {
            get { return this.width; }
            set { SetValue(ref this.width, value); }
        }

        public double Height
        {
            get { return this.height; }
            set { SetValue(ref this.height, value); }
        }

        public double PurchasePrice
        {
            get { return this.purchasePrice; }
            set { SetValue(ref this.purchasePrice, value); }
        }

        public double Price
        {
            get { return this.price; }
            set { SetValue(ref this.price, value); }
        }

        public double SpecialPrice
        {
            get { return this.specialPrice; }
            set { SetValue(ref this.specialPrice, value); }
        }

        public EditProductViewModel(Product product)
        {
            this.Product = product;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.SetProduct();
        }

        private void SetProduct()
        {
            this.Name = this.Product.Name;
            this.Sku = this.Product.Sku;
            this.Brand = this.Product.Brand;
            this.Description = this.Product.Description;
            this.Stock = this.Product.Stock;
            this.Weight = this.Product.Weight;
            this.Length = this.Product.Length;
            this.Width = this.Product.Width;
            this.Height = this.Product.Height;
            this.PurchasePrice = this.Product.PurchasePrice;
            this.Price = this.Product.SalePrice;
            this.SpecialPrice = this.Product.SpecialPrice;
            this.IsFeatured = this.Product.IsFeatured;
            this.HasSpecialPrice = this.Product.HasSpecialPrice;
            this.Logo = this.Product.Image;
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }


        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                        "Desde donde quieres cargar la imagen",
                        "Cancelar",
                        null,
                        "Cargar desde galeria",
                        "Cámara"
                    );

            if (source == "Cancelar")
            {
                this.file = null;
                return;
            }

            if (source == "Tomar una nueva imagen")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.Logo = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        private async void EditProduct()
        {
            if (string.IsNullOrEmpty(this.Name) || string.IsNullOrEmpty(this.Description) || string.IsNullOrEmpty(this.Sku) || string.IsNullOrEmpty(this.Brand))
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar todos los datos",
                        "Aceptar"
                    );
                return;
            }

            /*byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }*/

            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.EditProduct(
                 mainViewModel.User.api_token,
                 "https://thenuap.com",
                 this.product.CategoryId,
                 this.Name,
                 this.Sku,
                 this.Brand,
                 this.Description,
                 this.Stock,
                 this.Weight,
                 this.Height,
                 this.Length,
                 this.Width,
                 this.PurchasePrice,
                 this.Price,
                 this.SpecialPrice,
                 this.HasSpecialPrice,
                 this.IsFeatured,
                 this.product.Id);

            if (response == null)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Errors != null)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "",
                "Se ha editado el producto exitosamente",
                "Aceptar");

            mainViewModel.Inventory = new InventoryViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InventoryPage());
        }
    }
}
