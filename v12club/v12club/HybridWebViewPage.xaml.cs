using Xamarin.Forms;

namespace v12club
{
	public partial class HybridWebViewPage : ContentPage
	{
		HybridWebView view;
		public HybridWebViewPage(HybridWebView webView)
		{
			InitializeComponent();
			view = webView;
			View_Wrapper.Children.Add(webView);
			webView.RegisterAction(data => Support.ConsoleLog(data));
			webView.IsVisible = true;
		}

		protected override bool OnBackButtonPressed()
		{
			try
			{
				view.GoBack();
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}