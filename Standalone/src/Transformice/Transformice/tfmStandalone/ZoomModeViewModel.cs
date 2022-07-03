using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class ZoomModeViewModel : BindableBase
	{
		public FlashPlayer.ZoomModeEnum Mode { get; }
		public string Display { get; }
		private bool _isSelected;
		
		public ZoomModeViewModel(FlashPlayer.ZoomModeEnum mode)
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
