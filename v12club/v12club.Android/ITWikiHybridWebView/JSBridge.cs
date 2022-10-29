using Android.Webkit;

using Java.Interop;

using System;

namespace v12club.Droid
{
  public class JSBridge : Java.Lang.Object
  {
    readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

    public JSBridge(HybridWebViewRenderer hybridRenderer)
    {
      hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
    }

    [JavascriptInterface]
    [Export("invokeAction")]
    public void InvokeAction(string data)
    {
      HybridWebViewRenderer hybridRenderer;

      if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
      {
        ((HybridWebView)hybridRenderer.Element).InvokeAction(data);
      }
    }

    [JavascriptInterface]
    [Export("invokeApplication")]
    public void InvokeApplication(string data)
    {
      HybridWebViewRenderer hybridRenderer;

      if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
      {
        ((HybridWebView)hybridRenderer.Element).InvokeApplication(data);
      }
    }
  }
}

