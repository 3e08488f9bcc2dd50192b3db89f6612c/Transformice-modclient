using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class SanctionOccurenceViewModel : BindableBase
	{
		private string _mute;
		private string _ban;
		private string _other;
		
		public string Mute
		{
			get
			{
				return this._mute;
			}
			set
			{
				this.SetProperty<string>(ref this._mute, value, "Mute");
			}
		}

		public string Ban
		{
			get
			{
				return this._ban;
			}
			set
			{
				this.SetProperty<string>(ref this._ban, value, "Ban");
			}
		}

		public string Other
		{
			get
			{
				return this._other;
			}
			set
			{
				this.SetProperty<string>(ref this._other, value, "Other");
			}
		}
	}
}
