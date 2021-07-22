
using v12club.Models;
using v12club.Views;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{

	public partial class App : Xamarin.Forms.Application
	{
		public static readonly BindableProperty IsBusyProperty =
			 BindableProperty.Create("IsBusy", // название обычного свойства
					 typeof(bool), // возвращаемый тип 
					 typeof(App), // тип,  котором объявляется свойство
					 false,// значение по умолчанию
					BindingMode.TwoWay
			 );
		public static bool IsBusy
		{
			set
			{
			
			}
			get
			{
				return (bool)App.Current.GetValue(IsBusyProperty);
			}
		}

		public static JSBridgeObject BridgeObject { get; set; }
		public static string Version { get; } = AppInfo.Version.ToString();

		public App()
		{
			InitializeComponent();

			BridgeObject = new JSBridgeObject();

			MainPage = new Content();
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
