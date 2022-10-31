using System;
using System.Linq;
using System.Threading.Tasks;

using v12club.ViewModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;

using DeviceInfo = Xamarin.Essentials.DeviceInfo;

namespace v12club.Views
{
  public partial class Content : ContentPage
  {
    public Content()
    {
      InitializeComponent();
      WebView_wrapper.Navigated += WebView_Navigated;
      WebView_wrapper.Navigating += WebView_wrapper_Navigating;

      WebView_wrapper.ApplicationInvoking += ApplicationInvoking_Invoking;
      WebView_wrapper.RegisterAction(data => JSNotifyHandler(data));
      WebView_wrapper.RegisterInvokeApplication(data => AppInvokeHandler(data));
     
      Page_wrapper.Children.Add(new LoginView(WebView_wrapper));

      this.BindingContext = new MainPageViewModel(WebView_wrapper);

      onplatform_button.Source = DeviceInfo.Platform == DevicePlatform.iOS ? "left_arrow.png" : "garage_white.png";

      On<iOS>().SetPrefersStatusBarHidden(StatusBarHiddenMode.True)
         .SetPreferredStatusBarUpdateAnimation(UIStatusBarAnimation.Fade);
    }

    private void ApplicationInvoking_Invoking(object sender, WebNavigatingEventArgs e)
    {
      if (e.Url == "https://v12club.ru/?logout")
      {
        WebView_wrapper.IsVisible = false;
        Page_wrapper.IsVisible = true;
        Buttons_grid.IsVisible = false;
      }
      else if (!e.Url.StartsWith("http"))
      {
        WebView_wrapper.IsVisible = false;
        Buttons_grid.IsVisible = false;
      }
    }

    private void WebView_wrapper_Navigating(object sender, WebNavigatingEventArgs e)
    {
      if (e.Url == "https://v12club.ru/?logout")
      {
        WebView_wrapper.IsVisible = false;
        Page_wrapper.IsVisible = true;
        Buttons_grid.IsVisible = false;
      }
      else if (!e.Url.StartsWith("http"))
      {
        WebView_wrapper.IsVisible = false;
        Buttons_grid.IsVisible = false;
      }
    }


    private async void AppInvokeHandler(string data)
    {
      await Launcher.TryOpenAsync(data);
    }

    protected override bool OnBackButtonPressed()
    {
      DependencyService.Get<INotify>().Touch();
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
      if (e.Url == "https://v12club.ru/reg" || e.Url == "https://v12club.ru/remindpass")
      {
        App.BridgeObject.ClientStatus = Models.Status.OnRegistration;
        Page_wrapper.IsVisible = false;
        WebView_wrapper.IsVisible = true;
        Buttons_grid.IsVisible = false;
      }
      else if (App.BridgeObject.IsFirstLoad)
      {
        App.BridgeObject.IsFirstLoad = false;
        Page_wrapper.IsVisible = true;
      }
      else if (App.BridgeObject.IsLogined && !WebView_wrapper.IsVisible)//переключение на основную страницу приложения
      {
        Buttons_grid.IsVisible = true;
        Page_wrapper.IsVisible = false;
        WebView_wrapper.IsVisible = true;
        (Page_wrapper.Children[0].BindingContext as LoginViewModel).RememberCredentials();
      }
      else if (!App.BridgeObject.IsLogined && WebView_wrapper.IsVisible)//переключение на страницу авторизации
      {
        WebView_wrapper.IsVisible = false;
        Page_wrapper.IsVisible = true;
        Buttons_grid.IsVisible = false;
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

      if (obj.IsFirstLoad && obj.IsLogined)
      {


        App.BridgeObject.IsFirstLoad = false;
        obj.IsFirstLoad = false;
        App.BridgeObject.IsLogined = true;

        App.BridgeObject.ClientStatus = Models.Status.SuccessfullyAuthorized;
        obj.ClientStatus = Models.Status.SuccessfullyAuthorized;
      }

      if (obj.EventType == "invokeApp")
      {
        Launcher.TryOpenAsync(obj.Name);
        return;
      }

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

      else if (obj.EventType == "loaded")
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
          obj.ClientStatus = obj.IsLogined ? OnNavigated() : Models.Status.NotAuthorized;
        }
        App.BridgeObject = obj;
      }
    }


    Models.Status OnNavigated()
    {
      return Models.Status.SuccessfullyAuthorized;
    }
  }
}
