using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}