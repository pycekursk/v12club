using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;

using System;

namespace v12club.Droid
{
	[Activity(Label = "v12club", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			//Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
			Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
			//Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

			base.OnCreate(bundle);
			global::Xamarin.Forms.Forms.Init(this, bundle);

			int uiOptions = (int)Window.DecorView.SystemUiVisibility;

			uiOptions |= (int)SystemUiFlags.LowProfile;
			//uiOptions |= (int)SystemUiFlags.Fullscreen;
			uiOptions |= (int)SystemUiFlags.HideNavigation;
			uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

			LoadApplication(new App());

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
			{
				int mycode = 0;
				Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, mycode);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}

