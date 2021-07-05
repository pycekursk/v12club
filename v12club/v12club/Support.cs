using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
	public class PressingTriggerAction : TriggerAction<ImageButton>
	{
		protected override void Invoke(ImageButton sender)
		{
			if (DeviceInfo.Platform != DevicePlatform.UWP) Vibration.Vibrate(50);

			if (sender.Source.ToString() == "File: eye.png") return;

			var buttons = App.Current.MainPage.FindByName<Grid>("Buttons_grid").Children;

			var activeButton = buttons.Where(b => b.Opacity == 1).FirstOrDefault() as ImageButton;

			if (activeButton != null && activeButton.Source == sender.Source) return;

			if (sender.Opacity == 1)
			{
				sender.ScaleTo(1, 150).ContinueWith(t => sender.FadeTo(0.5, 150));
			}
			else
			{
				buttons.ForEach(b => b.ScaleTo(1, 150).ContinueWith(t => b.FadeTo(0.5, 150)));
				sender.ScaleTo(1.2, 150).ContinueWith(t => sender.FadeTo(1, 150));
			}
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
			else if ((string)parameter == "version")
			{
				return $"v.{(value as string)}";
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

		public static string RemoveLast(this string str) => str?.Length > 0 ? str.Remove(str.Length - 1) : str;
		
		public static void UpdateTextThreadSafe(this Entry entry, string value)
		{
			if (entry.Dispatcher.IsInvokeRequired) entry.Dispatcher.BeginInvokeOnMainThread(() => entry.Text = value);
			else entry.Text = value;
		}

		public static string GetLast(this string str)
		{
			return str.Last().ToString();
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection) action.Invoke(item);
		}

		public static Task ForEachAsync<T>(this IEnumerable<T> collection, Action<T> action) => Task.Run(() => { foreach (var item in collection) action.Invoke(item); });

		public static void ParallelForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			Parallel.ForEach(collection, action);
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
}