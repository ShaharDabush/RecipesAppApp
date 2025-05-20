using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class LoadingPageView : ContentPage
{
	public LoadingPageView(HomePageViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}