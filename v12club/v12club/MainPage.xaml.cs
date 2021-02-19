using ProgressRingControl.Forms.Plugin;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
	public partial class MainPage : ContentPage
	{
		public static ProgressRingControl.Forms.Plugin.ProgressRing Spinner = new ProgressRing() { RingProgressColor = Color.FromHex("#fbc430"), RingThickness = 25, Margin = 70, Opacity = 0, IsEnabled = false };
		HybridWebView WebView;

		public MainPage()
		{
			InitializeComponent();

			var layoutOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true };
			WebView = new HybridWebView { Uri = "https://v12club.ru", VerticalOptions = layoutOptions, HorizontalOptions = layoutOptions };
			WebView_wrapper.IsVisible = false;

			WebView_wrapper.Children.Add(WebView);

			var wrapper = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			wrapper.Children.Add(new LoginView(WebView));

			Content_wrapper.Children.Insert(0, wrapper);
			((ICollection<View>)Spinner_wrapper.Children).Add(Spinner);
			Spinner.InitAnimation(500);

			WebView.Navigating += WebView_Navigating;
			WebView.Navigated += WebView_Navigated;

			WebView.RegisterAction(data => JSNotifyHandler(data));
		}

		protected override bool OnBackButtonPressed()
		{
			new Action(async () =>
			{
				if (WebView.CanGoBack & WebView_wrapper.IsVisible)
				{
					WebView.GoBack();
				}
				else if (!WebView.CanGoBack & WebView_wrapper.IsVisible)
				{
					if (await DisplayAlert("", "Вернуться на страницу авторизации?", "Да", "Нет"))
					{
						await WebView.EvaluateJavaScriptAsync("$('.exitButton')[0].click();");
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
			Spinner.IsEnabled = false;
			//if (e.Source.ToString().Contains("https://v12club.ru/reg"))
			//{

			//}
			if (App.BridgeObject.IsLogined & !WebView_wrapper.IsVisible)//переключение на основную страницу приложения
			{
				Content_wrapper.Children.Remove(Content_wrapper.Children[0]);
				WebView_wrapper.IsVisible = true;
				await WebView_wrapper.FadeTo(1, 250);
			}
			else if (!App.BridgeObject.IsLogined & WebView_wrapper.IsVisible)//переключение на страницу авторизации
			{
				App.BridgeObject = new Models.JSBridgeObject();
				App.Current.LoadUserData();

				var wrapper = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Opacity = 1 };
				wrapper.Children.Add(new LoginView(WebView));
				Content_wrapper.Children.Insert(0, wrapper);
				//await wrapper.FadeTo(1, 250);
				await WebView_wrapper.FadeTo(0, 250);
				WebView_wrapper.IsVisible = false;
			}
			await (sender as HybridWebView).FadeTo(1, 250);
		}

		private async void WebView_Navigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
		{
			if (App.BridgeObject.ClientStatus == Models.Status.SuccessfullyAuthorized)
			{
				Spinner.IsEnabled = true;
			}
			await (sender as HybridWebView).FadeTo(0, 250);
		}

		public static void JSNotifyHandler(string data)
		{
			Support.ConsoleLog("HybridWebViewPage >>>" + data);

			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<v12club.Models.JSBridgeObject>(data);

			obj.Login = App.BridgeObject.Login;
			obj.Password = App.BridgeObject.Password;
			obj.SaveSettings = App.BridgeObject.SaveSettings;
			obj.ClientStatus = App.BridgeObject.ClientStatus;

			if (obj.EventType == "click")
			{
				Vibration.Vibrate(25);
			}

			if (obj.EventType == "DOMContentLoaded")
			{
				if (App.BridgeObject.ClientStatus == Models.Status.TryAuthorization)
				{
					var content = (App.Current.MainPage as ContentPage).Content.FindByName<StackLayout>("Content_wrapper");
					if (obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
						content.Children[0].FadeTo(0, 250);
					}
					else
					{
						obj.ClientStatus = Models.Status.AuthorizationFailed;
						content.Children[0].FadeTo(1, 250);
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
	}
}
