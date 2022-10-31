
using System;
using System.Threading.Tasks;

using v12club.Models;
using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{

  public partial class App : Xamarin.Forms.Application
  {
    public static readonly BindableProperty IsBusyProperty =
       BindableProperty.Create("IsBusy", // название обычного свойства
           typeof(bool), // возвращаемый тип 
           typeof(App), // тип,  котором объявляется свойство
           false,// значение по умолчанию
          BindingMode.TwoWay
       );
    public static bool IsBusy
    {
      set
      {

      }
      get
      {
        return (bool)App.Current.GetValue(IsBusyProperty);
      }
    }

    public static void Toast(string message) => DependencyService.Get<IMessage>().LongAlert(message);

    public static JSBridgeObject BridgeObject { get; set; }
    public static string Version { get; } = AppInfo.Version.ToString();

    public App()
    {
      InitializeComponent();

      BridgeObject = new JSBridgeObject();

      MainPage = new Content();
    }

    protected override void OnAppLinkRequestReceived(Uri uri)
    {
      DependencyService.Get<IMessage>().LongAlert($"OnAppLinkRequestReceived - {uri.OriginalString}");
      base.OnAppLinkRequestReceived(uri);
    }

    protected override void OnStart()
    {
      //DependencyService.Get<IMessage>().LongAlert($"OnStart");
    }

    protected override async void OnSleep()
    {
      //var permission = await Permissions.CheckStatusAsync<Permissions.LaunchApp>();
      //DependencyService.Get<IMessage>().LongAlert($"OnSleep, {permission}");
    }

    protected override async void OnResume()
    {
      //var permission = await Permissions.CheckStatusAsync<Permissions.LaunchApp>();
      //DependencyService.Get<IMessage>().LongAlert($"OnResume, {permission}");
    }

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
      var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

      DependencyService.Get<IMessage>().LongAlert(status.ToString());

      if (status == PermissionStatus.Granted)
        return status;

      if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
      {
        // Prompt the user to turn on in settings
        // On iOS once a permission has been denied it may not be requested again from the application
        return status;
      }

      if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
      {
        // Prompt the user with additional information as to why the permission is needed
      }

      status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

      return status;
    }
  }
}
