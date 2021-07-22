using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace v12club
{
	public class ClickTriggerAction : TriggerAction<ImageButton>
	{
		protected override void Invoke(ImageButton sender)
		{
			DependencyService.Get<INotify>().Touch();
			sender.Padding = new Thickness(7, 5, 7, 9);
			sender.Opacity = 1;
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
		public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
		{
			foreach (var item in array) action(item);
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
}