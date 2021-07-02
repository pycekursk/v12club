using Foundation;

using v12club.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(ScreeniOS))]
namespace v12club.iOS
{
	public class ScreeniOS : IScreen
	{

		public string Version
		{
			get
			{
				NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
				return ver.ToString();
			}
		}

	}
}