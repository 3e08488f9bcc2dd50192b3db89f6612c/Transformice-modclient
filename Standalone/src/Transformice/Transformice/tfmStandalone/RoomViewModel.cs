using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class RoomViewModel : BindableBase
	{
		public string FullName { get; set; }
		public string Community { get; set; }
		private int _count;
		public string RoomName { get; set; }
		public ObservableCollection<RoomMemberViewModel> Members { get; }
		
		public RoomViewModel()
		{
			this.Members = new ObservableCollection<RoomMemberViewModel>();
		}
		
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this.SetProperty<int>(ref this._count, value, "Count");
			}
		}
		
		public bool IsInternational
		{
			get
			{
				return this.Community == "xx";
			}
		}
	}
}
