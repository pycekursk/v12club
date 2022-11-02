using System;
using System.Text.RegularExpressions;

using v12club.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
  public class HybridWebView : WebView
  {
    Action<string> action;
    Action<string> invokeApplication;

    public Action<object, WebNavigatingEventArgs> ApplicationInvoking { get; internal set; }

    public static readonly BindableProperty UriProperty = BindableProperty.Create(
        propertyName: "Uri",
        returnType: typeof(string),
        declaringType: typeof(HybridWebView),
        defaultValue: default(string));

    public string Uri
    {
      get { return (string)GetValue(UriProperty); }
      set { SetValue(UriProperty, value); }
    }


    public HybridWebView()
    {
      this.Navigated += HybridWebView_Navigated;
      this.Navigating += HybridWebView_Navigating;
    }

    private void HybridWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
      var url = e.Url;
      var regex = new Regex("^http.+");
      var match = regex.Match(url);

      if (!match.Success && match.Value != "about:blank")
      {
        OpenUri(url);
        e.Cancel = true;
        this.GoBack();
      }
    }

    private void HybridWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
      Uri = e.Url;
    }

    public async void OpenUri(string url)
    {
      var canOpen = await Xamarin.Essentials.Launcher.CanOpenAsync(url);
      if (canOpen)
        await Xamarin.Essentials.Launcher.OpenAsync(url);
    }

    public void StopLoading()
    {
      this.StopLoading();
    }

    public void RegisterAction(Action<string> callback)
    {
      action = callback;
    }

    public void RegisterInvokeApplication(Action<string> callback)
    {
      invokeApplication = callback;
    }

    public void Cleanup()
    {
      invokeApplication = null;
      action = null;
    }

    public void InvokeApplication(string data)
    {
      if (invokeApplication == null || data == null)
      {
        return;
      }
      invokeApplication(data);
    }

    public void InvokeAction(string data)
    {
      if (action == null || data == null)
      {
        return;
      }
      action.Invoke(data);
    }
  }
}