using Camera.MAUI.ZXing;
using Camera.MAUI;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;
using System.Diagnostics;

namespace RecipesAppApp.Views;

public partial class CreateIngredientView : Popup
{
    StorageViewModel vm;
    public CreateIngredientView( StorageViewModel vm )
	{
        this.BindingContext = vm;
        this.vm = vm;
        InitializeComponent();
	}
    public void ClosePopup(List<string> l) => Close();

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
        vm.AddIngredientToStorage();
    }
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        this.Close();
    }
    private void InitZing()
    {
        cameraView.BarcodeDetected += CameraView_BarcodeDetected;
        cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
        cameraView.BarCodeOptions = new BarcodeDecodeOptions
        {
            AutoRotate = true,
            PossibleFormats = { BarcodeFormat.QR_CODE },
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = true
        };
        cameraView.BarCodeDetectionFrameRate = 10;
        cameraView.BarCodeDetectionMaxThreads = 5;
        cameraView.ControlBarcodeResultDuplicate = true;
        cameraView.BarCodeDetectionEnabled = true;
    }
    private void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        Debug.WriteLine("BarcodeText=" + args.Result[0].Text);
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.NumCamerasDetected > 0)
        {

            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();

            });
        }
    }

    private void cameraView_BarcodeDetected_1(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            vm.IngredientCode = args.Result[0].BarcodeFormat.ToString();
            vm.GetIngredientByBarcode();
        });
    }
}