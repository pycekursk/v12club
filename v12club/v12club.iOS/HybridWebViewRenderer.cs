using Foundation;

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
    const string ApplicationInvokeFunction = "function invokeApplication(data){window.webkit.messageHandlers.invokeApplication.postMessage(data);}";
    readonly WKUserContentController userController;

    public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
    {
      CustomUserAgent = "ios";
    }

    public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
    {
      userController = config.UserContentController;
      var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentStart, true);
      var script2 = new WKUserScript(new NSString(ApplicationInvokeFunction), WKUserScriptInjectionTime.AtDocumentStart, true);
      userController.AddUserScript(script);
      userController.AddUserScript(script2);
      userController.AddScriptMessageHandler(this, "invokeAction");
      userController.AddScriptMessageHandler(this, "invokeApplication");
    }

    protected override void OnElementChanged(VisualElementChangedEventArgs e)
    {
      base.OnElementChanged(e);
     
      if (e.OldElement != null)
      {
        userController.RemoveAllUserScripts();
        userController.RemoveScriptMessageHandler("invokeAction");
        userController.RemoveScriptMessageHandler("invokeApplication");
        HybridWebView hybridWebView = e.OldElement as HybridWebView;
        hybridWebView.Cleanup();
      }

      if (NativeView != null && e.NewElement != null)
      {
        var url = new NSUrlRequest(new NSUrl(((HybridWebView)Element).Uri));
        LoadRequest(url);
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