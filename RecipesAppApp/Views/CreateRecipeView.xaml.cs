using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class CreateRecipeView : ContentPage
{
    private readonly IServiceProvider serviceProvider;
    public CreateRecipeView(CreateRecipeViewModel vm, IServiceProvider sp)
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