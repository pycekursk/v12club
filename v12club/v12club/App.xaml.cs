using Android.Webkit;

using System;

using v12club.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace v12club
{
	public partial class App : Xamarin.Forms.Application
	{
		public static HybridWebView webView;
		public static bool Loggined = false;
		public App()
		{
			InitializeComponent();

			var layoutOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true };
			webView = new HybridWebView { Uri = "http://id26339.noda.pro/", VerticalOptions = layoutOptions};
			
			//var vis = webView.Visual;
			//Animation animation = new Animation();
			

			webView.Navigating += WebView_Navigating;
			webView.Navigated += WebView_Navigated;


			MainPage = new LoginPage(webView);

			//MainPage = new HybridWebViewPage(webView);
		}

		private async void WebView_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
		{
			try
			{
				await (sender as HybridWebView).EvaluateJavaScriptAsync("fixUI()").ConfigureAwait(true);
			}
			catch { }

			var html = await (sender as HybridWebView).EvaluateJavaScriptAsync("$('#logInModal').text();");

			if (html != null && html.ToLower() == "вход" & App.Loggined)
			{
				App.Loggined = false;

				if (App.Current.MainPage.GetType() != typeof(LoginPage))
				{
					MainPage = new LoginPage(sender as HybridWebView);
				}
			}
			else if (html != null && html?.ToLower() != "вход" && !App.Loggined)
			{
				App.Loggined = true;
				if (App.Current.MainPage.GetType() != typeof(HybridWebView))
				{
					App.Current.MainPage = new HybridWebViewPage(sender as HybridWebView);
				}
			}
			(sender as HybridWebView).IsVisible = true;
		}

		private void WebView_Navigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
		{
			(sender as HybridWebView).IsVisible = false;
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
