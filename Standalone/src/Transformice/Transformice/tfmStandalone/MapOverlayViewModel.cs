using System;
using Prism.Commands;
using Transformice.tfmClientHook;

namespace Transformice.tfmStandalone
{
	public sealed class MapOverlayViewModel : FolderViewModel
	{
		public DelegateCommand ToggleCollapsedCommand { get; }
		public DelegateCommand<FolderViewModel> SelectFolderCommand { get; }
		public static readonly string MapsFolder = AppDomain.CurrentDomain.BaseDirectory + "Maps";
		private bool _isCollapsed;
		private FolderViewModel _selectedFolder;
		private bool _isSelectedFolderRoot;
		private bool _isSelectedFolderParentRoot;
		
		public bool IsCollapsed
		{
			get
			{
				return this._isCollapsed;
			}
			set
			{
				this.SetProperty<bool>(ref this._isCollapsed, value, "IsCollapsed");
			}
		}

		public FolderViewModel SelectedFolder
		{
			get
			{
				return this._selectedFolder;
			}
			set
			{
				if (this.SetProperty<FolderViewModel>(ref this._selectedFolder, value, "SelectedFolder"))
				{
					this.IsSelectedFolderRoot = (value == this);
					this.IsSelectedFolderParentRoot = (((value != null) ? value.Parent : null) == this);
				}
			}
		}

		public bool IsSelectedFolderRoot
		{
			get
			{
				return this._isSelectedFolderRoot;
			}
			set
			{
				this.SetProperty<bool>(ref this._isSelectedFolderRoot, value, "IsSelectedFolderRoot");
			}
		}

		public bool IsSelectedFolderParentRoot
		{
			get
			{
				return this._isSelectedFolderParentRoot;
			}
			set
			{
				this.SetProperty<bool>(ref this._isSelectedFolderParentRoot, value, "IsSelectedFolderParentRoot");
			}
		}
		
		public MapOverlayViewModel(ClientHook clientHook) : base(null, MapOverlayViewModel.MapsFolder, clientHook)
		{
			this.IsCollapsed = true;
			this.SelectedFolder = this;
			this.ToggleCollapsedCommand = new DelegateCommand(delegate()
			{
				this.IsCollapsed = !this.IsCollapsed;
			});
			this.SelectFolderCommand = new DelegateCommand<FolderViewModel>(delegate(FolderViewModel folder)
			{
				this.SelectedFolder = folder;
			});
		}
	}
}
