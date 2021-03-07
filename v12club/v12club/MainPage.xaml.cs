using System;

using v12club.ViewModels;
using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			App.IsBusy = true;

			WebView_wrapper.Navigating += WebView_Navigating;
			WebView_wrapper.Navigated += WebView_Navigated;
			WebView_wrapper.RegisterAction(data => JSNotifyHandler(data));

			Page_wrapper.Children.Add(new LoginView(WebView_wrapper));

			this.BindingContext = new MainPageViewModel(WebView_wrapper);

			this.SetBinding(IsEnabledProperty, "IsBusy", BindingMode.Default, new ValueConverter());

			Logo_wrapper.FadeTo(1, 250);
		}

		protected override bool OnBackButtonPressed()
		{
			if (App.IsBusy) return true;
			new Action(async () =>
			{
				if (WebView_wrapper.CanGoBack & WebView_wrapper.IsVisible)
				{
					WebView_wrapper.GoBack();
				}
				else if (!WebView_wrapper.CanGoBack & WebView_wrapper.IsVisible)
				{
					if (await DisplayAlert("", "Вернуться на страницу авторизации?", "Да", "Нет"))
					{
						await WebView_wrapper.EvaluateJavaScriptAsync("doExit();");
					}
				}
				else if (!WebView_wrapper.IsVisible)
				{
					if (await DisplayAlert("", "Закрыть приложение?", "Да", "Нет")) Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
				}
			}).Invoke();
			return true;
		}

		async void WebView_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
		{
			if (App.BridgeObject.IsFirstLoad)
			{
				await WebView_wrapper.EvaluateJavaScriptAsync("doExit()");
				App.BridgeObject.IsFirstLoad = false;
				Page_wrapper.IsVisible = true;
				Page_wrapper.FadeTo(1, 250);
				return;
			}
			else if (App.BridgeObject.IsLogined & !WebView_wrapper.IsVisible)//переключение на основную страницу приложения
			{
				Logo_wrapper.FadeTo(0, 250);
				Page_wrapper.FadeTo(0, 250);
				WebView_wrapper.FadeTo(1, 250);

				Logo_wrapper.IsVisible = false;
				Page_wrapper.IsVisible = false;
				WebView_wrapper.IsVisible = true;
			}
			else if (!App.BridgeObject.IsLogined & WebView_wrapper.IsVisible)//переключение на страницу авторизации
			{
				WebView_wrapper.FadeTo(0, 250);
				Logo_wrapper.FadeTo(1, 250);
				Page_wrapper.FadeTo(1, 250);

				WebView_wrapper.IsVisible = false;
				Page_wrapper.IsVisible = true;
				Logo_wrapper.IsVisible = true;

				App.BridgeObject = new Models.JSBridgeObject();
				await WebView_wrapper.EvaluateJavaScriptAsync("location.reload()");
			}

			App.IsBusy = false;
		}

		private void WebView_Navigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
		{
			App.IsBusy = true;
		}

		public static void JSNotifyHandler(string data)
		{
			Support.ConsoleLog("HybridWebViewPage >>>" + data);

			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<v12club.Models.JSBridgeObject>(data);

			obj.ClientStatus = App.BridgeObject.ClientStatus;
			obj.IsFirstLoad = App.BridgeObject.IsFirstLoad;

			if (obj.EventType == "click")
			{
				Vibration.Vibrate(10);
			}

			if (obj.EventType == "DOMContentLoaded")
			{
				if (App.BridgeObject.ClientStatus == Models.Status.TryAuthorization)
				{
					var content = (App.Current.MainPage as ContentPage).Content.FindByName<StackLayout>("Page_wrapper");
					if (obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
					}
					else
					{
						obj.ClientStatus = Models.Status.AuthorizationFailed;
						Device.BeginInvokeOnMainThread(async () => await (App.Current.MainPage as ContentPage).DisplayAlert("Ошибка авторизации", "Неверный логин/пароль, проверьте корректность ввода и повторите попытку.", "ОК"));
					}
				}

				else if (App.BridgeObject.ClientStatus == Models.Status.NotAuthorized)
				{
					if (obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
					}
					else
					{
						obj.ClientStatus = Models.Status.NotAuthorized;
					}
				}

				else if (App.BridgeObject.ClientStatus == Models.Status.SuccessfullyAuthorized)
				{
					if (!obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.NotAuthorized;
					}
					else
					{
						obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
					}
				}
				App.BridgeObject = obj;
			}
			Support.ConsoleLog(Newtonsoft.Json.JsonConvert.SerializeObject(App.BridgeObject));
		}


		//void ToggleView(HybridWebView webView, StackLayout wrapper)
		//{
		//	if (webView.IsVisible & !wrapper.IsVisible)
		//	{
		//		webView.FadeTo(0, 250).ContinueWith(t=> webView.IsVisible = false);
		//		wrapper.FadeTo(1, 250).ContinueWith(t => wrapper.IsVisible = true);
		//	}
		//	else
		//	{
		//		wrapper.FadeTo(1, 250).ContinueWith(t => wrapper.IsVisible = false);
		//		webView.FadeTo(0, 250).ContinueWith(t => webView.IsVisible = true);
		//	}
		//}
	}
}
