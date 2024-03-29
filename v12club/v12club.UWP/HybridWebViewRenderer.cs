﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using v12club;
using v12club.UWP;

using Windows.UI.Xaml.Controls;

using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace v12club.UWP
{
	public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Windows.UI.Xaml.Controls.WebView>
	{
		const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";

		protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				SetNativeControl(new Windows.UI.Xaml.Controls.WebView());
			}
			if (e.OldElement != null)
			{
				Control.NavigationCompleted -= OnWebViewNavigationCompleted;
				Control.ScriptNotify -= OnWebViewScriptNotify;
			}
			if (e.NewElement != null)
			{
				Control.NavigationCompleted += OnWebViewNavigationCompleted;
				Control.ScriptNotify += OnWebViewScriptNotify;
				Control.Settings.IsJavaScriptEnabled = true;
				var url = new Uri(Element.Uri);
				//Control.Source = url;
				Control.Navigate(url);
			}
		}

		async void OnWebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
		{
			if (args.IsSuccess)
			{
				// Inject JS script
				await Control.InvokeScriptAsync("eval", new[] { JavaScriptFunction });
			}
		}

		void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
		{
			Element.InvokeAction(e.Value);
		}
	}
}
