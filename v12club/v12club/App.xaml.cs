using ProgressRingControl.Forms.Plugin;

using System;
using System.IO;
using System.Threading.Tasks;

using v12club.Models;
using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static v12club.HybridWebViewPage;

namespace v12club
{
	public partial class App : Xamarin.Forms.Application
	{
		public static JSBridgeObject BridgeObject { get; set; }
		

		public App()
		{
			InitializeComponent();

			BridgeObject = new JSBridgeObject();
			App.Current.LoadUserData();

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{

		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{

		}
	}
}
