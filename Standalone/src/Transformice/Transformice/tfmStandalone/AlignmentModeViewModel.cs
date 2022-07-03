using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class AlignmentModeViewModel : BindableBase
	{
		public FlashPlayer.AlignmentModeEnum Mode { get; }
		public string Display { get; }
		private bool _isSelected;
		
		public AlignmentModeViewModel(FlashPlayer.AlignmentModeEnum mode)
		{
			this.Mode = mode;
			this.Display = mode.ToString();
		}
		
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
	}
}
