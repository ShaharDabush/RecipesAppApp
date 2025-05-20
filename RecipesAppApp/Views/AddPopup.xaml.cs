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
    public void ClosePopup(List<string> l) => Close();

    private void Button_Clicked(object sender, EventArgs e)
    {
        //CreateRecipeViewModel vm = (CreateRecipeViewModel)this.BindingContext;
        this.Close();
    }
}