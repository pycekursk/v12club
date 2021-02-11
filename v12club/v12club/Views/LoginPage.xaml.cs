using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using v12club.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace v12club.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage(HybridWebView webView)
		{
			InitializeComponent();
			this.WView.Children.Add(webView);
		}
	}
}