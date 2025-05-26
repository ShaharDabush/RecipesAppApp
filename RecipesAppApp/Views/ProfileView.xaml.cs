using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView(ProfileViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ProfileViewModel vm = (ProfileViewModel)this.BindingContext;
        vm.IsStorageNullInitData();
    }
}