using Foundation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UIKit;

using v12club.iOS;
using v12club.Models;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
namespace v12club.iOS
{
  public class OpenAppService : IOpenAppService
  {
    public Task<bool> Launch(string stringUrl)
    {
      try
      {
        NSUrl request = new NSUrl(stringUrl);
        bool isOpened = UIApplication.SharedApplication.OpenUrl(request);

        if (isOpened == false)
          UIApplication.SharedApplication.OpenUrl(new NSUrl(stringUrl));

        return Task.FromResult(true);
      }
      catch (Exception ex)
      {
        var alertView = new UIAlertView("Error", ex.Message, null, "OK", null);
        alertView.Show();
        return Task.FromResult(false);
      }
    }
  }
}