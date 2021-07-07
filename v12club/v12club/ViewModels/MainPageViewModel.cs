using System;
using System.Linq;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		public Command NavigatingCommand { get; }
		readonly HybridWebView HybridWeb;

		public MainPageViewModel(HybridWebView webView)
		{
			this.HybridWeb = webView;
			NavigatingCommand = new Command(OnNavigating);
		}

		private void OnNavigating(object obj)
		{
			var button = App.Current.MainPage.FindByName<ImageButton>(obj.ToString());
			//var buttons = App.Current.MainPage.FindByName<Grid>("Buttons_grid").Children.Where(child => child.GetType() == typeof(ImageButton));

			var url = obj.ToString() == "garage" ? $"personal_cabinet?{obj}" : obj;
			url = obj.ToString() == "personal_cabinet" ? "personal_cabinet?personal_info_edit" : url;
			url = obj.ToString() == "main" ? "" : url;

			//button.PropagateUpPressed();

			Action<VisualElement> action = new Action<VisualElement>((element) =>
			{
				if (element is ImageButton elem)
				{
					elem.Opacity = 0.5;
					elem.Padding = 12;
				}
				else if (element is StackLayout stack)
				{
					stack.Children.ForEach(b => { b.Opacity = 0.5; (b as ImageButton).Padding = 12; });
				}
			});

			App.Current.MainPage.FindByName<Grid>("Buttons_grid").Children.ForEach(action);

			//buttons.ForEach(b => { b.Opacity = 0.5; (b as ImageButton).Padding = 12; });

			MainThread.InvokeOnMainThreadAsync(() => HybridWeb.Source = @$"https://v12club.ru/{url}");
		}
	}
}
