using System;

using Xamarin.Forms;

namespace v12club
{
	public class HybridWebView : WebView
	{
		Action<string> action;

		public static readonly BindableProperty UriProperty = BindableProperty.Create(
				propertyName: "Uri",
				returnType: typeof(string),
				declaringType: typeof(HybridWebView),
				defaultValue: default(string));

		public string Uri
		{
			get { return (string)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}

		public HybridWebView()
		{
			this.Navigated += HybridWebView_Navigated;
			
		}

		private void HybridWebView_Navigated(object sender, WebNavigatedEventArgs e)
		{
			Uri = e.Url;
		}

		public void RegisterAction(Action<string> callback)
		{
			action = callback;
		}

		public void Cleanup()
		{
			action = null;
		}

		public void InvokeAction(string data)
		{
			if (action == null || data == null)
			{
				return;
			}
			action.Invoke(data);
		}
	}
}
