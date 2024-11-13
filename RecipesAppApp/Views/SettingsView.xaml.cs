using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class SettingsView : ContentPage
{
	public SettingsView(SettingViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}