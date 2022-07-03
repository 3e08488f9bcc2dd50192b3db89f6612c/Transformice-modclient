using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class QualityViewModel : BindableBase
	{
		public FlashPlayer.QualityEnum Quality { get; }
		public string Display { get; }
		private bool _isSelected;
		
		public QualityViewModel(FlashPlayer.QualityEnum quality)
		{
			this.Quality = quality;
			this.Display = quality.ToString();
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
