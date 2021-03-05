﻿using Foundation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UIKit;

using v12club;
using v12club.iOS;

using WebKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace v12club.iOS
{
	public class HybridWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
	{
		const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
		WKUserContentController userController;

		public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
		{
		}

		public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
		{
			userController = config.UserContentController;
			var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
			userController.AddUserScript(script);
			userController.AddScriptMessageHandler(this, "invokeAction");
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				userController.RemoveAllUserScripts();
				userController.RemoveScriptMessageHandler("invokeAction");
				HybridWebView hybridWebView = e.OldElement as HybridWebView;
				hybridWebView.Cleanup();
			}

			if (NativeView != null && e.NewElement != null)
			{
				var webView = NativeView as UIWebView;

				if (webView == null)
					return;

				webView.ScalesPageToFit = true;

				//string filename = Path.Combine(NSBundle.MainBundle.BundlePath, $"Content/{((HybridWebView)Element).Uri}");
				//LoadRequest(new NSUrlRequest(new NSUrl(filename, false)));
				//string filename = Path.Combine(NSBundle.MainBundle.BundlePath, $"Content/{((HybridWebView)Element).Uri}");
				LoadRequest(new NSUrlRequest(new NSUrl(((HybridWebView)Element).Uri)));
			}
		}

		public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
		{
			((HybridWebView)Element).InvokeAction(message.Body.ToString());
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