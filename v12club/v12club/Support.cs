using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using v12club.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
	public static class Extensions
	{
		//static BackgroundWorker backgroundWorker = new BackgroundWorker();
		public static void SaveUserData(this Application app)
		{
			File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "cfg.json", Newtonsoft.Json.JsonConvert.SerializeObject(App.BridgeObject));
		}

		public static void LoadUserData(this Application app)
		{
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


		public static void InitAnimation(this ProgressRingControl.Forms.Plugin.ProgressRing progressRing, int deration)
		{
			progressRing.PropertyChanged += ProgressRing_PropertyChanged;
		}

		static async Task DoAnimation(this ProgressRingControl.Forms.Plugin.ProgressRing progressRing, int deration)
		{
			await Task.Run(async () =>
			 {
				 {
					 do
					 {
						 await progressRing.ProgressTo(1, (uint)deration, Easing.Linear).ContinueWith(t =>
						 {
							 var progressColor = progressRing.RingProgressColor.ToHex();
							 var ringBaseColor = progressRing.RingBaseColor.ToHex();
							 progressRing.RingBaseColor = Color.FromHex(progressColor);
							 progressRing.Progress = 0;
							 progressRing.RingProgressColor = Color.FromHex(ringBaseColor);
						 });
					 } while (progressRing.Opacity != 0);
				 }
			 });
		}

		static async void DoAnimationOnBackground(this ProgressRingControl.Forms.Plugin.ProgressRing progressRing, int deration)
		{
			do
			{
				await progressRing.ProgressTo(1, (uint)deration, Easing.Linear).ContinueWith(t =>
				{
					var progressColor = progressRing.RingProgressColor.ToHex();
					var ringBaseColor = progressRing.RingBaseColor.ToHex();
					progressRing.RingBaseColor = Color.FromHex(progressColor);
					progressRing.Progress = 0;
					progressRing.RingProgressColor = Color.FromHex(ringBaseColor);
				});
			} while (true);
		}


		private static void ProgressRing_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsEnabled")
			{
				var ring = sender as ProgressRingControl.Forms.Plugin.ProgressRing;
				if (ring.IsEnabled)
				{
					ring.FadeTo(1, 50);
					DoAnimation(ring, 500);
				}

				else
				{
					ring.FadeTo(0, 50);
					ring.Progress = 0;
				}
			}
		}

		private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			//var worker = sender as BackgroundWorker;

			DoAnimationOnBackground((ProgressRingControl.Forms.Plugin.ProgressRing)e.Argument, 500);
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
		public static Task ConsoleLog(object data) => Task.Run(() => System.Diagnostics.Debug.WriteLine(
			@$"\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n\n\n{data.GetType().Name}\n\n{data}\n\n\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"));

		async public static Task<string> GetTextFromFile()
		{
			using (var stream = await FileSystem.OpenAppPackageFileAsync("scripts.js"))
			{
				using (var reader = new StreamReader(stream))
				{
					//App.Scripts = await reader.ReadToEndAsync();
				}
			}
			using (var stream = await FileSystem.OpenAppPackageFileAsync("styles.css"))
			{
				using (var reader = new StreamReader(stream))
				{
					//App.Styles = await reader.ReadToEndAsync();
				}
			}
			return default;
		}
	}
}