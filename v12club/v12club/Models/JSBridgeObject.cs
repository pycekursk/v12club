using System;
using System.Collections.Generic;
using System.Text;

namespace v12club.Models
{
	public enum Status
	{
		NotAuthorized = 0,
		SuccessfullyAuthorized,
		AuthorizationFailed,
		TryAuthorization
	}
	
	public class JSBridgeObject
	{
		public string EventType { get; set; }
		public bool IsLogined { get; set; }
		public Status ClientStatus { get; set; }
		public bool SaveSettings { get; set; } = true;
		public string Name { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
	}
}
