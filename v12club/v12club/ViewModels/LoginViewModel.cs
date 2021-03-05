using System;
using System.Threading;

using v12club.Views;

using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace v12club.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public Command LoginCommand { get; }
		public Command RegisterCommand { get; }
		public Command ForgetPasswordCommand { get; }
		public Command SaveSettingsCheckBoxCommand { get; }
		public string Login { get; set; }
		public string Password { get; set; }
		public bool Remember { get; set; }
		readonly HybridWebView HybridWeb;

		public LoginViewModel(HybridWebView webView)
		{
			HybridWeb = webView;

			LoginCommand = new Command(OnLoginClicked);
			RegisterCommand = new Command(OnRegisterClicked);
			SaveSettingsCheckBoxCommand = new Command(OnSaveSettingsCheckBox);
			ForgetPasswordCommand = new Command(OnForgetClicked);

			object settings = "";
			if (App.Current.Properties.TryGetValue("Login", out settings)) Login = settings.ToString();
			if (App.Current.Properties.TryGetValue("Password", out settings)) Password = settings.ToString();
			if (App.Current.Properties.TryGetValue("Remember", out settings)) Remember = bool.Parse(settings.ToString());
		}

		private async void OnLoginClicked(object obj)
		{
			if (Remember)
			{
					if (!App.Current.Properties.ContainsKey("Login"))
					{
						App.Current.Properties.Add("Login", Login);
						App.Current.Properties.Add("Password", Password);
						App.Current.Properties.Add("Remember", Remember);
						await App.Current.SavePropertiesAsync();
					}
			}
			else
			{
				if (App.Current.Properties.ContainsKey("Login"))
				{
					App.Current.Properties.Remove("Login");
					App.Current.Properties.Remove("Password");
					App.Current.Properties.Remove("Remember");
					await App.Current.SavePropertiesAsync();
				}
			}
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);

			App.BridgeObject.ClientStatus = Models.Status.TryAuthorization;

			await HybridWeb.EvaluateJavaScriptAsync($"$('#login_modal').val('{Login}');$('#pass_modal').val('{Password}');$('#go_modal').click();");

			//await HybridWeb.EvaluateJavaScriptAsync($"doLogin({Login},{Password})");

			var content = (App.Current.MainPage as ContentPage).Content.FindByName<StackLayout>("Content_wrapper");

			await content.Children[0].FadeTo(0, 250);
		}

		private async void OnRegisterClicked(object obj)
		{
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);

			await Browser.OpenAsync("https://v12club.ru/reg", BrowserLaunchMode.SystemPreferred);
		}
		private async void OnForgetClicked(object obj)
		{
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);

			await Browser.OpenAsync("https://v12club.ru/remindpass", BrowserLaunchMode.SystemPreferred);

		}
		private void OnSaveSettingsCheckBox(object obj)
		{
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);
		}
	}
}