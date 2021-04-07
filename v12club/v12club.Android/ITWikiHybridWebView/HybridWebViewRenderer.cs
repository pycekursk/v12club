using Android;
using Android.App;
using Android.Content;


using Java.Interop;

using System;

using v12club;
using v12club.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Application = Android.App.Application;

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
			global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);

			if (e.OldElement != null)
			{
				Control.RemoveJavascriptInterface("jsBridge");
				((HybridWebView)Element).Cleanup();
			}
			if (e.NewElement != null)
			{
				Control.Settings.DomStorageEnabled = true;
				Control.Download += Control_Download;

				Control.Settings.UserAgentString = "android";

				Control.Settings.CacheMode = Android.Webkit.CacheModes.NoCache;
				Control.SetWebViewClient(new JavascriptWebViewClient(this, $"javascript: {JavascriptFunction}"));
				Control.LoadUrl(((HybridWebView)Element).Uri);
				Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
			}
		}

		private void Control_Download(object sender, Android.Webkit.DownloadEventArgs e)
		{
			try
			{
				var source = Android.Net.Uri.Parse(e.Url);
				var request = new DownloadManager.Request(source);
				request.AllowScanningByMediaScanner();
				request.SetNotificationVisibility(DownloadVisibility.Visible);
				request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, "data.xls");
				var manager = DownloadManager.FromContext(Application.Context);
				manager.Enqueue(request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			};
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
