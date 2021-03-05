using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace v12club.UWP
{
	public class UserAgentHelper
	{
		[DllImport("urlmon.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

		private const int URLMON_OPTION_USERAGENT = 0x10000001;

		public static void SetDefaultUserAgent(string userAgent)
		{
			UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, userAgent, userAgent.Length, 0);
		}
	}
}
