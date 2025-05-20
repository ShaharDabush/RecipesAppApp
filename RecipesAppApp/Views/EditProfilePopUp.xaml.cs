using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class EditProfilePopUp : Popup
{
	public EditProfilePopUp(EditProfileViewModel vm)
	{
        this.BindingContext = vm;
        InitializeComponent();
	}
    public void ClosePopup(List<string> l) => Close();

    //private void Button_Clicked(object sender, EventArgs e)
    //{
    //    this.Close();
    //}
}