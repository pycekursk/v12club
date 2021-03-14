using System;

using v12club.ViewModels;
using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace v12club
{
	public partial class MainPage : Xamarin.Forms.TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();

			App.IsBusy = true;
		}
	}
}
