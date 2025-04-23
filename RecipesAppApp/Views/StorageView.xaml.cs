using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;
using Camera.MAUI.ZXing;
using Camera.MAUI;
using System.Diagnostics;
using Android.Service.Controls.Templates;

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
        InitZing();
        
	}

    public void DisplayPopup(List<string> l)
    {
        var popup = new CreateIngredientView((StorageViewModel)this.BindingContext);
        this.ShowPopup(popup);
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
}