using Android.App;
using Android.Content;

using System.Threading;

using v12club.Views;

using Xamarin.Forms;

namespace v12club.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public Command LoginCommand { get; }
		public string Login { get; set; }
		public string Password { get; set; }
		public static bool IsLoading { get; set; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLoginClicked);

			//Login = "+7(920)704-88-84";
			//Password = "3339393";
		}

		private async void OnLoginClicked(object obj)
		{
			await App.webView.EvaluateJavaScriptAsync($"$('#login_modal').val('{Login}');$('#pass_modal').val('{Password}');$('#go_modal').click();");
		}
	}
}
