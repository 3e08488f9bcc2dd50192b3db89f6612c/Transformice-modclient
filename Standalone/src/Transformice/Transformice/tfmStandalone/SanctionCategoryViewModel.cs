using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Transformice.tfmStandalone
{
	public sealed class SanctionCategoryViewModel : BindableBase
	{
		public ObservableCollection<SanctionViewModel> Sanctions { get; }
		private string _description;
		
		public SanctionCategoryViewModel()
		{
			this.Sanctions = new ObservableCollection<SanctionViewModel>();
		}

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
	}
}
