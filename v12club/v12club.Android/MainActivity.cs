using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;

using System;

using Color = Android.Graphics.Color;

namespace v12club.Droid
{
	[Activity(Label = "v12club", Icon = "@drawable/v12club_icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

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

			LoadApplication(new App());

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
				Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Internet }, 0);
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

