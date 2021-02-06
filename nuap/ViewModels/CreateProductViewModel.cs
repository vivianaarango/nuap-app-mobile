namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class CreateProductViewModel: BaseViewModel
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
        private double price;
        private double salePrice;
        private double specialPrice;
        private int isFeatured;
        private int hasSpecialPrice;
        private bool isEnabled;
        private ImageSource logo;
        private MediaFile file;

        public ImageSource Logo
        {
            get { return this.logo; }
            set { SetValue(ref this.logo, value); }
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

        public double Price
        {
            get { return this.price; }
            set { SetValue(ref this.price, value); }
        }

        public double SalePrice
        {
            get { return this.salePrice; }
            set { SetValue(ref this.salePrice, value); }
        }

        public double SpecialPrice
        {
            get { return this.specialPrice; }
            set { SetValue(ref this.specialPrice, value); }
        }

        public CreateProductViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.IsFeatured = 0;
            this.HasSpecialPrice = 0;
            this.Logo = "logo";
        }

        public ICommand NewProductCommand
        {
            get
            {
                return new RelayCommand(NewProduct);
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
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
            } else
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

        private async void NewProduct()
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

            var response = await this.apiService.CreateProduct(
                 mainViewModel.User.api_token,
                 "https://thenuap.com",
                 mainViewModel.CategoryProductID,
                 this.Name,
                 this.Sku,
                 this.Brand,
                 this.Description,
                 this.Stock,
                 this.Weight,
                 this.Height,
                 this.Length,
                 this.Width,
                 this.Price,
                 this.SalePrice,
                 this.SpecialPrice,
                 this.HasSpecialPrice,
                 this.IsFeatured);


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
                "Se ha creado el producto exitosamente",
                "Aceptar");

            mainViewModel.Inventory = new InventoryViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InventoryPage());
        }
    }
}
