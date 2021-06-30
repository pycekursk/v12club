using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace v12club.ViewModels
{
	public class BaseViewModel : BindableObject, INotifyPropertyChanged
	{
		string title = string.Empty;
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		protected bool SetProperty<T>(ref T backingStore, T value,
				[CallerMemberName] string propertyName = "",
				Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
