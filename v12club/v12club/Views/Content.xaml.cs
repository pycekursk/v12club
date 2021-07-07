using System;
using System.Linq;

using v12club.ViewModels;


using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

using DeviceInfo = Xamarin.Essentials.DeviceInfo;

namespace v12club.Views
{
	public partial class Content : ContentPage
	{
		public Content()
		{
			InitializeComponent();

			WebView_wrapper.Navigated += WebView_Navigated;
			WebView_wrapper.RegisterAction(data => JSNotifyHandler(data));

			Page_wrapper.Children.Add(new LoginView(WebView_wrapper));

			this.BindingContext = new MainPageViewModel(WebView_wrapper);

			if (DeviceInfo.Platform == DevicePlatform.iOS)
			{
				this.onplatform_button.Source = "back.png";
				this.onplatform_button.Clicked += onplatform_button_Clicked;
			}
			else
			{
				this.onplatform_button.Command = (this.BindingContext as MainPageViewModel).NavigatingCommand;
				this.onplatform_button.CommandParameter = "app_info";
				this.onplatform_button.Source = "info_circle_white.png";
			}
		}

		protected override bool OnBackButtonPressed()
		{
			new Action(async () =>
			{
				if (WebView_wrapper.CanGoBack & WebView_wrapper.IsVisible)
				{
					try
					{
						WebView_wrapper.GoBack();
					}
					catch
					{
						return;
					}
				}
				else if (WebView_wrapper.IsVisible)
				{
					if (await DisplayAlert("", "Вернуться на страницу авторизации?", "Да", "Нет"))
					{
						await WebView_wrapper.EvaluateJavaScriptAsync("doExit();");
					}
				}
				else if (!WebView_wrapper.IsVisible)
				{
					//if (await DisplayAlert("", "Закрыть приложение?", "Да", "Нет")) Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
				}
			}).Invoke();
			return true;
		}

		void WebView_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
		{
			if (App.BridgeObject.IsFirstLoad)
			{
				App.BridgeObject.IsFirstLoad = false;
				Page_wrapper.IsVisible = true;
			}
			if (App.BridgeObject.IsLogined & !WebView_wrapper.IsVisible)//переключение на основную страницу приложения
			{
				Page_wrapper.IsVisible = false;
				WebView_wrapper.IsVisible = true;

				(Page_wrapper.Children[0].BindingContext as LoginViewModel).RememberCredentials();
			}
			if (!App.BridgeObject.IsLogined & WebView_wrapper.IsVisible)//переключение на страницу авторизации
			{
				WebView_wrapper.IsVisible = false;
				Page_wrapper.IsVisible = true;
			}

			if (Indicator_wrapper.IsVisible)
				Indicator_wrapper.FadeTo(0, 300).ContinueWith(t => MainThread.BeginInvokeOnMainThread(() =>
				{
					Indicator_wrapper.IsVisible = false;
					Buttons_grid.InputTransparent = false;
				}));

			App.IsBusy = false;
		}

		public void JSNotifyHandler(string data)
		{
			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<v12club.Models.JSBridgeObject>(data);

			obj.ClientStatus = App.BridgeObject.ClientStatus;
			obj.IsFirstLoad = App.BridgeObject.IsFirstLoad;

			if (obj.EventType == "onbeforeunload")
			{
				App.IsBusy = true;
				if (!WebView_wrapper.IsVisible)
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						Buttons_grid.InputTransparent = true;
						Indicator_wrapper.IsVisible = true;
						Indicator_wrapper.FadeTo(1, 300);
					});
				}
			}

			if (obj.EventType == "click")
			{
				DependencyService.Get<INotify>().Touch();
			}

			if (obj.EventType == "loaded")
			{
				if (App.BridgeObject.ClientStatus == Models.Status.TryAuthorization)
				{
					if (obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
					}
					else if (!obj.IsLogined)
					{
						obj.ClientStatus = Models.Status.AuthorizationFailed;
						Device.BeginInvokeOnMainThread(async () => await this.DisplayAlert("Ошибка авторизации", "Неверный логин/пароль, проверьте корректность ввода и повторите попытку.", "ОК"));
						Page_wrapper.FadeTo(1, 300);
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
		}

		private void onplatform_button_Clicked(object sender, EventArgs e)
		{
			Action<VisualElement> action = new Action<VisualElement>((element) =>
			{
				if (element is ImageButton elem)
				{
					elem.Opacity = 0.5;
					elem.Padding = 12;
				}
			});

			App.Current.MainPage.FindByName<Grid>("Buttons_grid").Children.ForEach(action);

			this.SendBackButtonPressed();
		}
	}
}
