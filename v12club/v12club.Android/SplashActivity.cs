using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

using System.Threading.Tasks;

namespace v12club.Droid
{
	[Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenLayout, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : AppCompatActivity
	{
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
		}
		protected override void OnResume()
		{
			base.OnResume();

			var preloader = new Task(async () =>
			{
				await Task.Delay(1500);
			});

			preloader.Start();

			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}
	}
}