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
        vm.OpenPopup1 += DisplayPopup1;
        InitializeComponent();
        
        
	}

    public void DisplayPopup(List<string> l)
    {
        var popup = new CreateIngredientView((StorageViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }
    public void DisplayPopup1(List<string> l)
    {
        var popup = new CreateNewStorageView((StorageViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StorageViewModel vm = (StorageViewModel)this.BindingContext;
        vm.IsStorageNullInitData();
    }



}