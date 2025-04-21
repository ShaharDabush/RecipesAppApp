using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class StorageView : ContentPage
{
    private readonly IServiceProvider serviceProvider;
    public StorageView(StorageViewModel vm, IServiceProvider sp)
	{
        this.serviceProvider = sp;
        this.BindingContext = vm;
        vm.OpenPopup += DisplayPopup;
        InitializeComponent();
	}

    public void DisplayPopup(List<string> l)
    {
        var popup = new AddPopup((CreateRecipeViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }
}