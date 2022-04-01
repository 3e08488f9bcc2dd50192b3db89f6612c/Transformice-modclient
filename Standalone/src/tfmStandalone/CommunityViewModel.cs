using System;
using Prism.Mvvm;

namespace tfmStandalone
{
	private bool _isSelected;
	private string _community;
	public sealed class CommunityViewModel : BindableBase
	{
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this.SetProperty<bool>(ref this._isSelected, value, "IsSelected");
			}
		}
		public string Community
		{
			get
			{
				return this._community;
			}
			set
			{
				this.SetProperty<string>(ref this._community, value, "Community");
			}
		}
	}
}
