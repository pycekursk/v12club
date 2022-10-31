using System;
using System.Collections.Generic;
using System.Linq;

using AudioToolbox;

using Foundation;

using UIKit;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club.iOS
{
  public class Application
  {
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
      //var newArgs = new string[args.Length + 1];
      //for (int i = 0; i < args.Length; i++)
      //  newArgs[i] = args[i];

      //newArgs[newArgs.Length - 1] = "AppDelegate";

      UIApplication.Main(args);

      //UIApplication.Main(args, null, "AppDelegate");
    }
  }
}

