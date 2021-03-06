using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class LogViewModel : BindableBase
	{
		public bool IsPlayer { get; set; }
		public string Key { get; set; }
		public byte FontStyle { get; set; }
		public string WindowKey { get; set; }
		public string OriginalText { get; set; }
		public List<LoginViewModel> Logins { get; }
		private bool _isSelected;
		private string _keyColor;
		
		public LogViewModel()
		{
			this.Logins = new List<LoginViewModel>();
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

		public string KeyColor
		{
			get
			{
				return this._keyColor;
			}
			set
			{
				this.SetProperty<string>(ref this._keyColor, value, "KeyColor");
			}
		}
	}
}
