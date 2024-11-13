using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView(ProfileViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}