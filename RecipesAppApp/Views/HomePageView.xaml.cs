using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView(HomePageViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}