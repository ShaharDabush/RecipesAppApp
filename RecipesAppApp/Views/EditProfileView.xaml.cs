using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class EditProfileView : ContentPage
{
	public EditProfileView(EditProfileViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}