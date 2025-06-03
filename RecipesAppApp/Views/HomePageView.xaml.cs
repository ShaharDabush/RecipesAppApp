using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using RecipesAppApp.Classes;
using RecipesAppApp.ViewModels;
using Syncfusion.Maui.Buttons;

namespace RecipesAppApp.Views;

public partial class HomePageView : ContentPage
{
   
    public HomePageView(HomePageViewModel vm)
	{
        BindingContext = vm;
        //WeakReferenceMessenger.Default.Register<TriggerUiMessage>(this, (r, m) =>
        //{
        //    if (m.Value == "RunAllergieChangeStateTrue")
        //    {
        //        AllergieChangeStateTrue();
        //    }
        //    else if (m.Value == "RunAllergieChangeStateFalse")
        //    {
        //        AllergieChangeStateFalse();
        //    }
        //    else if (m.Value == "RunAllergieChangeStateInter")
        //    {
        //        AllergieChangeStateInter();
        //    }
        //});
        InitializeComponent();
	}

    private void AllergiesCheckChanged(object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
        HomePageViewModel vm = (HomePageViewModel)this.BindingContext;
        vm.FilterRecipes();
    }
    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    HomePageViewModel vm = (HomePageViewModel)this.BindingContext;
    //    vm.Refresh();
    //}

    //private void YourAllergiesCheck(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    //{
    //    HomePageViewModel vm = (HomePageViewModel)this.BindingContext;
    //    vm.YourAllergiesCheck(sender, e);
    //}
    //private void AllergiesStateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    //{
    //    HomePageViewModel vm = (HomePageViewModel)this.BindingContext;
    //    vm.AllergiesStateChanged(sender, e);
    //}
    //private void AllergieChangeStateTrue()
    //{
    //    var checkbox = this.FindByName<SfCheckBox>("allergiesCheckBox");
    //    checkbox.IsChecked = true;
    //}
    //private void AllergieChangeStateFalse()
    //{
    //    var checkbox = this.FindByName<SfCheckBox>("allergiesCheckBox");
    //    checkbox.IsChecked = false;
    //}
    //private void AllergieChangeStateInter()
    //{
    //    var checkbox = this.FindByName<SfCheckBox>("allergiesCheckBox");
    //    checkbox.IsChecked = null;
    //}
}