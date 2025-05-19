using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;
using Camera.MAUI;
using System.Diagnostics;

namespace RecipesAppApp.Views;

public partial class StorageView : ContentPage
{
    private readonly IServiceProvider serviceProvider;
    StorageViewModel vm;
    public StorageView(StorageViewModel vm, IServiceProvider sp)
	{
        this.serviceProvider = sp;
        this.BindingContext = vm;
        this.vm = vm;
        vm.OpenPopup += DisplayPopup;
        InitializeComponent();
        
        
	}

    public void DisplayPopup(List<string> l)
    {
        var popup = new CreateIngredientView((StorageViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }

   
    
}