using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v12club.Views
{
	public class RegistrationViewFlyoutMenuItem
	{
		public RegistrationViewFlyoutMenuItem()
		{
			TargetType = typeof(RegistrationViewFlyoutMenuItem);
		}
		public int Id { get; set; }
		public string Title { get; set; }

		public Type TargetType { get; set; }
	}
}