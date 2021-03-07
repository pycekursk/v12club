using Android.Webkit;

using System;

using Xamarin.Forms.Platform.Android;

namespace v12club.Droid
{
	public class JavascriptWebViewClient : FormsWebViewClient
	{
		string _javascript;

		public JavascriptWebViewClient(HybridWebViewRenderer renderer, string javascript) : base(renderer)
		{
			_javascript = javascript;
		}


		public override void OnPageFinished(WebView view, string url)
		{
			base.OnPageFinished(view, url);
			try
			{
				view.EvaluateJavascript(_javascript, null);
			}
			catch (Exception exp)
			{

				Support.ConsoleLog(exp.Message + "\n" + exp.StackTrace);
			}

		}
	}
}
