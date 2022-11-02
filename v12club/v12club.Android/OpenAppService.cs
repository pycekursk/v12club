using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using v12club.Droid;
using v12club.Models;
using Application = Android.App.Application;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
namespace v12club.Droid
{
  public class OpenAppService : Activity, IOpenAppService
  {
    public async Task<bool> Launch(string packageName)
    {
      try
      {
        //var intent1 = new Intent(Intent.ActionView, Android. Uri.Parse(packageName));
        //intent1.SetFlags(ActivityFlags.NewTask);
        //Application.Context.StartActivity(intent1);

        var canOpen = await Xamarin.Essentials.Launcher.CanOpenAsync("sberpay://");
        if (canOpen)
        {
          await Xamarin.Essentials.Launcher.OpenAsync(packageName);
        }
        else
        {
          var intent = new Intent(Intent.ActionView);
          intent.SetData(Android.Net.Uri.Parse($"market://search?q={packageName}&c=apps"));
          intent.SetFlags(ActivityFlags.NewTask);
          Application.Context.StartActivity(intent);
        }
        return true;
      }
      catch (Exception exc)
      {
        App.Toast(exc.Message);
        return false;
      }
    }

    private bool IsAppInstalled(string packageName)
    {
      PackageManager pm = Android.App.Application.Context.PackageManager;
      bool installed = false;
      try
      {
        pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
        installed = true;
      }
      catch (PackageManager.NameNotFoundException e)
      {
        installed = false;
      }
      return installed;
    }
  }
}