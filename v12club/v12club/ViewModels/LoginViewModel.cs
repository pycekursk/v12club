﻿
using System;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {
    public Command LoginCommand { get; }
    public Command RegisterCommand { get; }
    public Command ForgetPasswordCommand { get; }
    public Command ShowPasswordCommand { get; }
    public Command SaveSettingsCheckBoxCommand { get; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool remember { get => _remember; set { _remember = value; Remember = value; } }

    public static bool Remember { get; set; }

    readonly HybridWebView HybridWeb;
    private bool _remember;

    public LoginViewModel(HybridWebView webView)
    {
      HybridWeb = webView;
      LoginCommand = new Command(OnLoginClicked);
      RegisterCommand = new Command(OnRegisterClicked);
      ForgetPasswordCommand = new Command(OnForgetClicked);
      ShowPasswordCommand = new Command(OnShowPasswordClicked);
      object settings = "";
      if (App.Current.Properties.TryGetValue("Login", out settings)) Login = settings.ToString();
      if (App.Current.Properties.TryGetValue("Password", out settings)) Password = settings.ToString();
      if (App.Current.Properties.TryGetValue("Remember", out settings)) remember = bool.Parse(settings.ToString());
    }
    private void OnShowPasswordClicked(object obj)
    {
      DependencyService.Get<INotify>().Touch();
      var field = ((App.Current.MainPage as ContentPage).FindByName<StackLayout>("Page_wrapper").Children[0] as ContentView).FindByName<Entry>("Password");
      (field.IsPassword ? new Action(() =>
      {
        (obj as ImageButton).Opacity = 1;
        (obj as ImageButton).Source = "eye.png";
        field.IsPassword = false;
      }) : new Action(() =>
      {
        (obj as ImageButton).Opacity = 0.5;
        (obj as ImageButton).Source = "eye_slash.png";
        field.IsPassword = true;
      })).Invoke();
    }

    private void OnLoginClicked(object obj)
    {
      //DependencyService.Get<INotify>().Touch();
      if (string.IsNullOrEmpty(Login)) return;
      else if (string.IsNullOrEmpty(Password))
      {
        App.Current.MainPage.FindByName<Entry>("Password")?.Focus();
      }

      App.BridgeObject.ClientStatus = Models.Status.TryAuthorization;

      //HybridWeb.Eval("location.reload()");

      HybridWeb.Eval($"$('#login_modal').val('{Login}');$('#pass_modal').val('{Password}');$('#lgnform_modal').submit();");
      //await HybridWeb.EvaluateJavaScriptAsync($"$('#login_modal').val('{Login}');$('#pass_modal').val('{Password}');$('#go_modal').click();");
    }

    private void OnRegisterClicked(object obj)
    {
      DependencyService.Get<INotify>().Touch();
      if (HybridWeb.Dispatcher.IsInvokeRequired)
        MainThread.InvokeOnMainThreadAsync(() => HybridWeb.Source = "https://v12club.ru/reg");
      else HybridWeb.Source = "https://v12club.ru/reg";
    }

    private void OnForgetClicked(object obj)
    {
      DependencyService.Get<INotify>().Touch();
      if (HybridWeb.Dispatcher.IsInvokeRequired)
        MainThread.InvokeOnMainThreadAsync(() => HybridWeb.Source = "https://v12club.ru/reg");
      else HybridWeb.Source = "https://v12club.ru/remindpass";
    }

    public void RememberCredentials()
    {
      if (remember)
      {
        if (!App.Current.Properties.ContainsKey("Login"))
        {
          App.Current.Properties.Add("Login", Login);
          App.Current.Properties.Add("Password", Password);
          App.Current.Properties.Add("Remember", remember);
          App.Current.SavePropertiesAsync();
        }
      }
      else if (App.Current.Properties.ContainsKey("Login"))
      {
        App.Current.Properties.Remove("Login");
        App.Current.Properties.Remove("Password");
        App.Current.Properties.Remove("Remember");
        App.Current.SavePropertiesAsync();
      }
    }
  }
}