using System.Diagnostics;

using v12club.Models;

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
				Support.ConsoleLog("App > set " + value);
				App.Current.SetValue(IsBusyProperty, value);
			}
			get
			{
				return (bool)App.Current.GetValue(IsBusyProperty);
			}
		}

		public static JSBridgeObject BridgeObject { get; set; }
		public static bool ready = false;

		public App()
		{
			InitializeComponent();

			this.PropertyChanged += App_PropertyChanged;

			BridgeObject = new JSBridgeObject();

			MainPage = new MainPage();
		}

		private void App_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Debug.WriteLine(e.PropertyName);
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
