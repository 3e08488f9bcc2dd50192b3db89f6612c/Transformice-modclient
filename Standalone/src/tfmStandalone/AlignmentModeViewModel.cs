using System;
using Prism.Mvvm;

namespace tfmStandalone
{
	public sealed class AlignmentModeViewModel : BindableBase
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
		public FlashPlayer.AlignmentModeEnum Mode { get; }
		public string Display { get; }
		public AlignmentModeViewModel(FlashPlayer.AlignmentModeEnum mode)
		{
			this.Mode = mode;
			this.Display = mode.ToString();
		}
		private bool _isSelected;
	}
}
