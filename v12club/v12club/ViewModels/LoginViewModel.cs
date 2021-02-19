using Android.App;
using Android.Content;

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
		public Command SaveSettingsCheckBoxCommand { get; }
		public string Login { get; set; }
		public string Password { get; set; }
		public bool IsSaveSettings { get; set; }

		HybridWebView HybridWeb;
		public LoginViewModel(HybridWebView webView)
		{
			HybridWeb = webView;
			LoginCommand = new Command(OnLoginClicked);
			RegisterCommand = new Command(OnRegisterClicked);
			SaveSettingsCheckBoxCommand = new Command(OnSaveSettingsCheckBox);

			if (App.BridgeObject.SaveSettings)
			{
				Login = App.BridgeObject.Login;
				Password = App.BridgeObject.Password;
				IsSaveSettings = true;
			}
			//Login = "+7(920)704-88-84";
			//Password = "3339393";
		}

		private async void OnLoginClicked(object obj)
		{
			App.BridgeObject.Login = Login;
			App.BridgeObject.Password = Password;
			if (App.BridgeObject.SaveSettings) App.Current.SaveUserData();

			Vibration.Vibrate(50);
			await HybridWeb.EvaluateJavaScriptAsync($"$('#login_modal').val('{Login}');$('#pass_modal').val('{Password}');$('#go_modal').click();");

			var content = (App.Current.MainPage as ContentPage).Content.FindByName<StackLayout>("Content_wrapper");
			var ring = (App.Current.MainPage as ContentPage).Content.FindByName<RelativeLayout>("Spinner_wrapper").Children[0] as ProgressRingControl.Forms.Plugin.ProgressRing;

			await content.Children[0].FadeTo(0, 250);
			ring.IsEnabled = true;

			App.BridgeObject.ClientStatus = Models.Status.TryAuthorization;
		}

		private void OnRegisterClicked(object obj)
		{
			Vibration.Vibrate(50);
			//var webviewWrapper = obj as StackLayout;
			//var wv = webviewWrapper.Children[0] as HybridWebView;

			//var page = App.Current.MainPage as ContentPage;
			//var contentWrapper = page.Content.FindByName<StackLayout>("Content_wrapper");

			//wv.Source = "https://v12club.ru/reg";
			//contentWrapper.IsVisible = false;
			//webviewWrapper.IsVisible = true;
		}

		private void OnSaveSettingsCheckBox(object obj)
		{
			Vibration.Vibrate(50);
			//var webviewWrapper = obj as StackLayout;
			//var wv = webviewWrapper.Children[0] as HybridWebView;

			//var page = App.Current.MainPage as ContentPage;
			//var contentWrapper = page.Content.FindByName<StackLayout>("Content_wrapper");

			//wv.Source = "https://v12club.ru/reg";
			//contentWrapper.IsVisible = false;
			//webviewWrapper.IsVisible = true;
		}
	}
}