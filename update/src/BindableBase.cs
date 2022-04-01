using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Update
{
	public abstract class BindableBase : INotifyPropertyChanged
	{
		// Member Variables
		public event PropertyChangedEventHandler PropertyChanged;
		// Member Functions
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if(object.Equals(storage, value)) return false;
			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if(propertyChanged == null) return;
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
