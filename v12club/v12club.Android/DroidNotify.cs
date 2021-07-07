
using v12club.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Notify))]
namespace v12club.Droid
{
	class Notify : INotify
	{
		public void Touch()
		{
			Xamarin.Essentials.Vibration.Vibrate(5);
		}
	}
}