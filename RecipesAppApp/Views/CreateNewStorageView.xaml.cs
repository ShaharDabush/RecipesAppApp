using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class CreateNewStorageView : Popup
{
    StorageViewModel vm;
    public CreateNewStorageView(StorageViewModel vm)
    {
        this.BindingContext = vm;
        this.vm = vm;
        InitializeComponent();
	}
    public void ClosePopup(List<string> l) => Close();

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}