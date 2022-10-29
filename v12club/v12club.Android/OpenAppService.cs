using Android.App;
using Android.Content;
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

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
namespace v12club.Droid
{
  public class OpenAppService : Activity, IOpenAppService
  {
    public Task<bool> Launch(string stringUri)
    {
      try
      {
        Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(stringUri);
        if (intent != null)
        {
          intent.AddFlags(ActivityFlags.NewTask);
          Forms.Context.StartActivity(intent);
        }
        else
        {
          intent = new Intent(Intent.ActionView);
          intent.AddFlags(ActivityFlags.NewTask);
          intent.SetData(Android.Net.Uri.Parse("market://details?id=" + stringUri)); // This launches the PlayStore and search for the app if it's not installed on your device
          Forms.Context.StartActivity(intent);
        }
        return Task.FromResult(true);
      }
      catch (Exception exc)
      {
        Console.Write(exc.Message);
      }
      return Task.FromResult(false);
    }
  }
}
