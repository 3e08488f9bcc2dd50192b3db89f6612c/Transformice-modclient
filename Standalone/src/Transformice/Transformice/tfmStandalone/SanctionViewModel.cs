using System;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class SanctionViewModel : BindableBase
	{
		private string _description;
		private SanctionOccurenceViewModel _firstOccurence;
		private SanctionOccurenceViewModel _secondOccurence;
		private SanctionOccurenceViewModel _thirdOccurence;
		private SanctionOccurenceViewModel _fourthOccurence;
		private string _additionalInformation;
		private bool _importantAdditionalInfo;
		
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this.SetProperty<string>(ref this._description, value, "Description");
			}
		}
		
		public SanctionOccurenceViewModel FirstOccurence
		{
			get
			{
				return this._firstOccurence;
			}
			set
			{
				this.SetProperty<SanctionOccurenceViewModel>(ref this._firstOccurence, value, "FirstOccurence");
			}
		}

		public SanctionOccurenceViewModel SecondOccurence
		{
			get
			{
				return this._secondOccurence;
			}
			set
			{
				this.SetProperty<SanctionOccurenceViewModel>(ref this._secondOccurence, value, "SecondOccurence");
			}
		}

		public SanctionOccurenceViewModel ThirdOccurence
		{
			get
			{
				return this._thirdOccurence;
			}
			set
			{
				this.SetProperty<SanctionOccurenceViewModel>(ref this._thirdOccurence, value, "ThirdOccurence");
			}
		}

		public SanctionOccurenceViewModel FourthOccurence
		{
			get
			{
				return this._fourthOccurence;
			}
			set
			{
				this.SetProperty<SanctionOccurenceViewModel>(ref this._fourthOccurence, value, "FourthOccurence");
			}
		}

		public string AdditionalInformation
		{
			get
			{
				return this._additionalInformation;
			}
			set
			{
				this.SetProperty<string>(ref this._additionalInformation, value, "AdditionalInformation");
			}
		}

		public bool ImportantAdditionalInfo
		{
			get
			{
				return this._importantAdditionalInfo;
			}
			set
			{
				this.SetProperty<bool>(ref this._importantAdditionalInfo, value, "ImportantAdditionalInfo");
			}
		}
	}
}
