using System;
using System.Collections.Generic;
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
      NavigatingCommand = new Command(OnButtonClick);
      ScrollToTopCommand = new Command(async (obj) => await HybridWeb.EvaluateJavaScriptAsync("$([document.documentElement, document.body]).animate({scrollTop: 0}, 500)"));
    }



    private void HybridWeb_Navigating(object sender, WebNavigatingEventArgs e)
    {
      if (!e.Url.Contains("v12club")) return;
      var regex = new Regex(@"(?<=ru\/)\w+");
      var match = regex.Match(e.Url);
      string buttonName = String.Empty;
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
      if (string.IsNullOrEmpty(buttonName)) return;
      ImageButton button = App.Current.MainPage.FindByName<ImageButton>(buttonName);
      var buttons = HybridWeb.Parent.FindByName<Grid>("Buttons_grid").Children;
      buttons.ForEach(b =>
      {
        if (b is ImageButton button && b.AutomationId != "up_arrow")
        {
          if (Dispatcher.IsInvokeRequired)
          {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
              button.Padding = new Thickness(10);
              button.Opacity = 0.5;
            });
          }
          else
          {
            button.Padding = new Thickness(10);
            button.Opacity = 0.5;
          }
        }
      });
      if (button != null)
      {
        button.Padding = new Thickness(7, 5, 7, 9);
        button.Opacity = 1;
      }
    }

    private void OnButtonClick(object obj)
    {
      var buttons = HybridWeb.Parent.FindByName<Grid>("Buttons_grid").Children;
      buttons.ForEach(b =>
      {
        if (b is ImageButton button && b.AutomationId != "up_arrow")
        {
          button.Padding = new Thickness(10);
          button.Opacity = 0.5;
        }
      });

      obj =
        obj.ToString() == "onplatform_button" ? "personal_cabinet?garage" :
        obj.ToString() == "personal_cabinet" ? "personal_cabinet?personal_info_edit" :
        obj.ToString() == "main" ? "" : obj;

      if (DeviceInfo.Platform == DevicePlatform.iOS && obj.ToString() == "personal_cabinet?garage" && HybridWeb.CanGoBack)
      {
        HybridWeb.GoBack();
        return;
      }
      HybridWeb.Source = @$"https://v12club.ru/{obj}";
    }
  }
}
