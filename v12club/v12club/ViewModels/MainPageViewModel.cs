using System;
using System.Linq;
using System.Text.RegularExpressions;

using v12club.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club.ViewModels
{
  public class MainPageViewModel : BaseViewModel
  {
    public Command NavigatingCommand { get; }
    public Command ScrollToTopCommand { get; }
    readonly HybridWebView HybridWeb;

    public MainPageViewModel(HybridWebView webView)
    {
      HybridWeb = webView;
      HybridWeb.Navigating += HybridWeb_Navigating;
      HybridWeb.Navigated += HybridWeb_Navigated;
      NavigatingCommand = new Command(OnButtonClick);

      ScrollToTopCommand = new Command(async () => await HybridWeb.EvaluateJavaScriptAsync("$([document.documentElement, document.body]).animate({scrollTop: 0}, 500)"));
    }

    private void HybridWeb_Navigated(object sender, WebNavigatedEventArgs e)
    {
      //HybridWeb.EvaluateJavaScriptAsync("loader(false)");
    }

    private void HybridWeb_Navigating(object sender, WebNavigatingEventArgs e)
    {
      var regex = new Regex(@"(?<=ru\/)\w+");
      var match = regex.Match(e.Url);

      if (DeviceInfo.Platform != DevicePlatform.iOS)
      {
        MainThread.BeginInvokeOnMainThread(() =>
        {
          foreach (ImageButton child in App.Current.MainPage.FindByName<Grid>("Buttons_grid")?.Children)
          {
            child.Opacity = 0.5;
            child.Padding = 12;
          }
        });
      }
      else
      {
        var butt = App.Current.MainPage.FindByName<ImageButton>("onplatform_button");
        butt.Padding = 12;
        butt.Opacity = 0.5;
      }

      string buttonName;

      if (e.Url.Contains("garage") & DeviceInfo.Platform != DevicePlatform.iOS)
      {
        buttonName = "onplatform_button";
      }
      else if (match.Value == string.Empty || new Regex(@"catalog$|carbase|laximo").Match(e.Url).Success)
      {
        buttonName = "main";
      }
      else
      {
        buttonName = match?.Value;
      }

      ImageButton button = App.Current.MainPage.FindByName<ImageButton>(buttonName);

      if (button != null)
      {
        button.Padding = new Thickness(7, 5, 7, 9);
        button.Opacity = 1;
      }
    }

    private void OnButtonClick(object obj)
    {
      if (DeviceInfo.Platform == DevicePlatform.iOS)
      {
        foreach (ImageButton child in App.Current.MainPage.FindByName<Grid>("Buttons_grid")?.Children)
        {
          child.Opacity = 0.5;
          child.Padding = 12;
        }
      }

      //await HybridWeb.EvaluateJavaScriptAsync("loader(true)");

      obj =
        obj.ToString() == "onplatform_button" ? "personal_cabinet?garage" :
        obj.ToString() == "personal_cabinet" ? "personal_cabinet?personal_info_edit" :
        obj.ToString() == "main" ? "" : obj;

      if (DeviceInfo.Platform == DevicePlatform.iOS & obj.ToString() == "personal_cabinet?garage" & HybridWeb.CanGoBack)
      {
        HybridWeb.GoBack();
      }
      else
      {
        HybridWeb.Source = @$"https://v12club.ru/{obj}";
      }
    }
  }
}
