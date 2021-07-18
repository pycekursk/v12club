namespace v12club.Models
{
	public enum Status
	{
		NotAuthorized = 0,
		TryAuthorization,
		SuccessfullyAuthorized,
		AuthorizationFailed,
		OnRegistration
	}

	public class JSBridgeObject
	{
		public string EventType { get; set; }
		public bool IsLogined { get; set; }
		public Status ClientStatus { get; set; }
		public bool SaveSettings { get; set; } = true;
		public bool IsFirstLoad { get; set; } = true;
		public string Name { get; set; }
	}
}
