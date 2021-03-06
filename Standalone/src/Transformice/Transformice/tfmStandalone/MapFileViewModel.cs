using System;
using System.IO;
using Prism.Commands;
using Prism.Mvvm;
using Transformice.tfmClientHook;
using Transformice.tfmClientHook.Messages;

namespace Transformice.tfmStandalone
{
	public sealed class MapFileViewModel : BindableBase
	{
		public DelegateCommand NppMapCommand { get; }
		public DelegateCommand NpMapCommand { get; }
		private string _map;
		private string _maskMap;
		private string _filePath;
		
		public string Map
		{
			get
			{
				return this._map;
			}
			set
			{
				this.SetProperty<string>(ref this._map, value, "Map");
			}
		}

		public string MaskMap
		{
			get
			{
				return this._maskMap;
			}
			set
			{
				this.SetProperty<string>(ref this._maskMap, value, "MaskMap");
			}
		}

		public string FilePath
		{
			get
			{
				return this._filePath;
			}
			set
			{
				this.SetProperty<string>(ref this._filePath, value, "FilePath");
			}
		}

		public MapFileViewModel(string filePath, ClientHook clientHook)
		{
			MapFileViewModel mapFileViewModel = this;
			this.FilePath = filePath;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			if (fileNameWithoutExtension != null && fileNameWithoutExtension.Contains("_"))
			{
				string[] array = fileNameWithoutExtension.Split(new char[]
				{
					'_'
				});
				if (array.Length == 2)
				{
					this._map = array[0];
					this._maskMap = array[1];
				}
			}
			else
			{
				this._map = fileNameWithoutExtension;
			}
			this.NppMapCommand = new DelegateCommand(delegate()
			{
				string command = string.IsNullOrWhiteSpace(mapFileViewModel.MaskMap) ? string.Format("npp {0}", mapFileViewModel.Map) : string.Format("npp {0} {1}", mapFileViewModel.Map, mapFileViewModel.MaskMap);
				clientHook.SendToServer(ConnectionType.Main, new C_Command
				{
					Command = command
				});
			});
			this.NpMapCommand = new DelegateCommand(delegate()
			{
				string command = string.IsNullOrWhiteSpace(mapFileViewModel.MaskMap) ? string.Format("np {0}", mapFileViewModel.Map) : string.Format("np {0} {1}", mapFileViewModel.Map, mapFileViewModel.MaskMap);
				clientHook.SendToServer(ConnectionType.Main, new C_Command
				{
					Command = command
				});
			});
		}
	}
}
