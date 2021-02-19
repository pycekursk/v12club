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
			Wrapper.Children.Add(webView);
		}

		//protected override bool OnBackButtonPressed()
		//{
		//	new Action(async () =>
		//	{
		//		var view = Wrapper.Children[0] as HybridWebView;
		//		if (view.CanGoBack) view.GoBack();
		//		else if (await DisplayAlert("", "Вернуться на страницу авторизации?", "Да", "Нет")) { 
		//			await view.EvaluateJavaScriptAsync("$('.exitButton')[0].click();"); 
		//			await view.FadeTo(0, 300);
		//		};
		//	})
		//	.Invoke();
		//	return true;
		//}
	}
}