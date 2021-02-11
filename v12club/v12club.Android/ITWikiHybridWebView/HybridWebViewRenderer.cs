﻿using Android.Content;

using v12club;
using v12club.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace v12club.Droid
{
	public class HybridWebViewRenderer : WebViewRenderer
	{
		const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
		Context _context;

		public HybridWebViewRenderer(Context context) : base(context)
		{
			_context = context;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				Control.RemoveJavascriptInterface("jsBridge");
				((HybridWebView)Element).Cleanup();
			}
			if (e.NewElement != null)
			{
				Control.SetWebViewClient(new JavascriptWebViewClient(this, $"javascript: {JavascriptFunction}"));
				Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
				Control.LoadUrl(((HybridWebView)Element).Uri);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((HybridWebView)Element).Cleanup();
			}
			base.Dispose(disposing);
		}
	}
}
