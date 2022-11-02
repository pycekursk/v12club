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
    public Task<bool> Launch(string uri)
    {
      try
      {
        var canOpen = UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri));
        return Task.FromResult(canOpen && UIApplication.SharedApplication.OpenUrl(new NSUrl(uri)));
      }
      catch
      {
        return Task.FromResult(false);
      }
    }
  }
}