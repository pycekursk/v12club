using System;
using System.Threading.Tasks;

using v12club.Models;
using v12club.Views;

using Xamarin.Forms;

namespace v12club
{
	public partial class HybridWebViewPage : ContentPage
	{
		public HybridWebViewPage(HybridWebView webView)
		{
			InitializeComponent();
			this.Content = webView;
		}
	}
}