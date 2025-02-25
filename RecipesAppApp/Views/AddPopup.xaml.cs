using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class AddPopup : Popup
{
    public AddPopup(CreateRecipeViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }


}