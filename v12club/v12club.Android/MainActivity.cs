using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
			Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
			uiOptions |= (int)SystemUiFlags.LowProfile;
			uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

			try
			{
				Window.SetNavigationBarColor(Color.Rgb(36, 50, 56));
			}
			catch (Exception)
			{

			}

			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;


			var themeAccentColor = new TypedValue();
			this.Theme.ResolveAttribute(Resource.Attribute.colorAccent, themeAccentColor, true);
			var droidAccentColor = new Android.Graphics.Color(themeAccentColor.Data);

			// set Xamarin Color.Accent to match the theme's accent color
			var accentColorProp = typeof(Xamarin.Forms.Color).GetProperty(nameof(Xamarin.Forms.Color.Accent), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			var xamarinAccentColor = new Xamarin.Forms.Color(droidAccentColor.R / 251.0, droidAccentColor.G / 196.0, droidAccentColor.B / 48.0, droidAccentColor.A / 255.0);
			accentColorProp.SetValue(null, xamarinAccentColor, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, null, System.Globalization.CultureInfo.CurrentCulture);


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
			Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
			uiOptions |= (int)SystemUiFlags.LowProfile;
			uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

			try
			{
				Window.SetNavigationBarColor(Color.Rgb(36, 50, 56));
			}
			catch
			{

			}


			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
			base.OnResume();
		}
	}
}

