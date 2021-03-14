using System.Linq;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		public Command NavigatingCommand { get; }
		readonly HybridWebView HybridWeb;

		public MainPageViewModel(HybridWebView webView)
		{
			this.HybridWeb = webView;
			NavigatingCommand = new Command(OnNavigating);
		}

		private void OnNavigating(object obj)
		{
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);

			var button = App.Current.MainPage.FindByName<ImageButton>(obj.ToString());
			var buttons = App.Current.MainPage.FindByName<Grid>("Buttons_grid").Children;

			var url = obj.ToString() == "garage" ? $"personal_cabinet?{obj}" : obj;
			url = obj.ToString() == "personal_cabinet" ? "personal_cabinet?personal_info_edit" : url;
			url = obj.ToString() == "main" ? "" : url;
			
			button.PropagateUpPressed();

			buttons.Where(b => b.Opacity > 0.5).ToList().ForEach((b) => { b.Opacity = 0.5; b.Scale = 0.8; });

			MainThread.InvokeOnMainThreadAsync(() => HybridWeb.Source = @$"https://v12club.ru/{url}");
		}
	}
}
