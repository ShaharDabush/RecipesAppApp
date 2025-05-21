using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class LoadingPageView : ContentPage
{
	public LoadingPageView(LoadingPageViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}