namespace nuap.Views
{
	using Xamarin.Forms;
	using Xamarin.Forms.Xaml;

	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : TabbedPage
	{
		public HomeTabbedPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}