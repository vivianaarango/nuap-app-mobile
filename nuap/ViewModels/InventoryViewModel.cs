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

    public class InventoryViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<CategoryItemViewModel> categories;
        private bool isRefreshing;

        public ObservableCollection<CategoryItemViewModel> Categories
        {
            get { return this.categories; }
            set { SetValue(ref this.categories, value); }
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
                return new RelayCommand(LoadCategories);
            }
        }

        public InventoryViewModel()
        {
            this.apiService = new ApiService();
            this.LoadCategories();
        }


        private async void LoadCategories()
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

            var response = await this.apiService.GetCategories(
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

            mainViewModel.CategoryList = (List<Category>)response.Data;
            this.Categories = new ObservableCollection<CategoryItemViewModel>(ToCategoryViewModel());
            this.IsRefreshing = false;
        }

        private IEnumerable<CategoryItemViewModel> ToCategoryViewModel()
        {
            return MainViewModel.GetInstance().CategoryList.Select(c => new CategoryItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = "https://thenuap.com/"+c.Image
            });
        }
    }
}
