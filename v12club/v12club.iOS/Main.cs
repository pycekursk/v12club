using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;

using UIKit;

using Xamarin.Essentials;

namespace v12club.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			//UIApplicationDelegate uIApplicationDelegate = new UIApplicationDelegate();
			//uIApplicationDelegate.BeginInvokeOnMainThread(() =>UIApplication.Main(args, null, "AppDelegate"));
			MainThread.BeginInvokeOnMainThread(() => UIApplication.Main(args, null, "AppDelegate"));
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
		}
	}
}
