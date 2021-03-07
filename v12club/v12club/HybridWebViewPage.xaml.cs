
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