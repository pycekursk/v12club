using System;
using System.Collections.Generic;
using System.Text;

namespace v12club
{
	public class Support
	{
		public static void ConsoleLog(string data)
		{
			System.Diagnostics.Debug.WriteLine(data);
		}

		public static string GetTextFromFile()
		{
			//using (var stream = await FileSystem.OpenAppPackageFileAsync("scripts.js"))
			//{
			//	using (var reader = new StreamReader(stream))
			//	{
			//		App.Scripts = await reader.ReadToEndAsync();
			//	}
			//}
			//using (var stream = await FileSystem.OpenAppPackageFileAsync("styles.css"))
			//{
			//	using (var reader = new StreamReader(stream))
			//	{
			//		App.Styles = await reader.ReadToEndAsync();
			//	}
			//}
			return default;
		}
	}
}
