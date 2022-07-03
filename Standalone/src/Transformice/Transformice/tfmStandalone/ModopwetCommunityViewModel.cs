using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class ModopwetCommunityViewModel : BindableBase
	{
		public string Community { get; set; }
		private int _reportCount;
		private bool _isMonitored;

		public int ReportCount
		{
			get
			{
				return this._reportCount;
			}
			set
			{
				this.SetProperty<int>(ref this._reportCount, value, "ReportCount");
			}
		}

		public bool IsMonitored
		{
			get
			{
				return this._isMonitored;
			}
			set
			{
				this.SetProperty<bool>(ref this._isMonitored, value, "IsMonitored");
			}
		}
	}
}
