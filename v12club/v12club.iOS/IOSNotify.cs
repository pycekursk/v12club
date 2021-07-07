
using UIKit;

using v12club.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(Notify))]
namespace v12club.iOS
{
	public class Notify : INotify
	{
		public void Touch()
		{
			// Initialize feedback
			var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
			impact.Prepare();

			// Trigger feedback
			impact.ImpactOccurred();
		}
	}
}