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
			if (App.IsBusy) return;
			var page = App.Current.MainPage as MainPage;
			var button = page.FindByName<ImageButton>(obj.ToString());
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);
			Support.ConsoleLog(obj);
			var url = obj.ToString() == "garage" ? $"personal_cabinet?{obj}" : obj;
			url = obj.ToString() == "personal_cabinet" ? "personal_cabinet?personal_info_edit" : url;
			url = obj.ToString() == "main" ? "" : url;
			HybridWeb.Source = @$"https://v12club.ru/{url}";
			var buttons = (button.Parent as Grid).Children;
			buttons.Where(b => b.Opacity > 0.5).ToList().ForEach((b) => { b.Opacity = 0.5; b.Scale = 0.8; });
			button.PropagateUpPressed();
		}
	}
}
