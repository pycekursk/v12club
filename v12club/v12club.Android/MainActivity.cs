using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;

using v12club.Models;

using Xamarin.Forms;

using Color = Android.Graphics.Color;

namespace v12club.Droid
{
  [Activity(Label = "v12club", Icon = "@drawable/v12club_icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
  //[IntentFilter(new[] { Android.Content.Intent.ActionView }, DataScheme = "http", DataHost = "v12club.ru", DataPathPrefix = "/", AutoVerify = true, Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
  //[IntentFilter(new[] { Android.Content.Intent.ActionView }, DataScheme = "https", DataHost = "v12club.ru", DataPathPrefix = "/", AutoVerify = true, Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      //global::Xamarin.Forms.Forms.Init(this, bundle);

      int uiOptions = (int)Window.DecorView.SystemUiVisibility;
      uiOptions |= (int)SystemUiFlags.LowProfile;
      uiOptions |= (int)SystemUiFlags.ImmersiveSticky;
      try
      {
        Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
        Window.SetNavigationBarColor(Color.Rgb(36, 50, 56));
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
      }
      catch { }

      DependencyService.Register<INotify, Notify>();
      DependencyService.Register<IOpenAppService, OpenAppService>();


      Forms.Init(this, savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);




      if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
        Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessWifiState }, 0);

      LoadApplication(new App());
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnResume()
    {
      int uiOptions = (int)Window.DecorView.SystemUiVisibility;
      uiOptions |= (int)SystemUiFlags.LowProfile;
      uiOptions |= (int)SystemUiFlags.ImmersiveSticky;
      try
      {
        Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
        Window.SetNavigationBarColor(Color.Rgb(36, 50, 56));
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
      }
      catch { }
      base.OnResume();
    }
  }
}

