using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using v12club.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
	public class PressingTriggerAction : TriggerAction<ImageButton>
	{
		protected override void Invoke(ImageButton sender)
		{
			if (App.IsBusy) return;
			sender.ScaleTo(1.2, 150);
			sender.FadeTo(1, 150);
		}
	}

	public class ValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((string)parameter == "color")
			{
				return (bool)value == true ? Color.White : Color.FromHex("#333333");
			}
			else return (bool)value ? false : true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value;
		}
	}

	public static class Extensions
	{
		public static void SaveUserData(this Application app)
		{
			if (DeviceInfo.Platform == DevicePlatform.UWP) return;
			File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "cfg.json", Newtonsoft.Json.JsonConvert.SerializeObject(App.BridgeObject));
		}

		public static void LoadUserData(this Application app)
		{
			if (DeviceInfo.Platform == DevicePlatform.UWP) return;
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "cfg.json";
			try
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JSBridgeObject>(File.ReadAllText(appData));
				if (obj.SaveSettings)
				{
					obj.ClientStatus = App.BridgeObject.ClientStatus;
					obj.IsLogined = App.BridgeObject.IsLogined;
					App.BridgeObject = obj;
				}
			}
			catch
			{
				File.Create(appData);
			}
		}

		public static bool IsNumeric(this string s)
		{
			foreach (char c in s)
			{
				if (!char.IsDigit(c) && c != '.')
				{
					return false;
				}
			}

			return true;
		}

		public static string RemoveLast(this string str) => str.Length > 0 ? str.Remove(str.Length - 1) : str;

		public static void RemoveLast(this Entry entry) => entry.Text = entry.Text.RemoveLast();

		public static void UpdateTextThreadSafe(this Entry entry, string value)
		{
			if (entry.Dispatcher.IsInvokeRequired) entry.Dispatcher.BeginInvokeOnMainThread(() => entry.Text = value);
			else entry.Text = value;
		}

		public static string GetLast(this string str)
		{
			return str.Last().ToString();
		}

		public static async Task VerifyPhone(this Entry entry, TextChangedEventArgs e = null)
		{
			await Task.Run(() =>
			 {
				 string result = entry.Text;

				 if (result.Length <= 3)
				 {
					 result = "+7(";
				 }

				 if (result.Length > 16 | !result.GetLast().IsNumeric() & result.GetLast() != "(")
				 {
					 result = result.RemoveLast();
				 }

				 if (e?.OldTextValue?.Length > e?.NewTextValue?.Length & e?.NewTextValue?.Length > 3)
				 {
					 return result;
				 }

				 Regex regex = new Regex(@"^\+\d{1,1}$|^\+\d{1,1}\(\d{1,3}$|^\+\d{1,1}\(\d{1,3}\)\d{1,3}$|^\+\d{1,1}\(\d{1,3}\)\d{1,3}-\d{1,2}$|^\+\d{1,1}\(\d{1,3}\)\d{1,3}-\d{1,2}-\d{1,2}$");

				 var matches = regex.Match(result);
				 if (matches.Success)
				 {
					 switch (matches.Value.Length)
					 {
						 case 2: result += "("; break;
						 case 6: result += ")"; break;
						 case 10: result += "-"; break;
						 case 13: result += "-"; break;
						 case 16: break;
						 default: break;
					 }
				 }
				 else
				 {
					 var last = result.GetLast() != "(" ? result.GetLast() : "";

					 result = result.RemoveLast();
					 matches = regex.Match(result);
					 switch (matches.Value.Length)
					 {
						 case 2: result += "("; break;
						 case 6: result += ")"; break;
						 case 10: result += "-"; break;
						 case 13: result += "-"; break;
						 case 16: break;
						 default: break;
					 }
					 result += last;
				 }

				 entry.UpdateTextThreadSafe(result);
				 return result;
			 });
		}
	}

	public class Support
	{
		public static Task ConsoleLog(object data) => Task.Run(() =>
		{
			//System.Diagnostics.Debug.WriteLine(@$"\n>>>>>>>>>>>>>>>>>>>>>>>> {data}\n");
		});
	}
}