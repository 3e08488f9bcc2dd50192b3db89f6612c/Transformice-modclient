using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Transformice.tfmClientHook;
using Transformice.tfmClientHook.Messages;

namespace Transformice.tfmStandalone
{
	public class AnnouncementWindow : PinnableWindow, IComponentConnector
	{
		private ClientHook ClientHook { get; }
		internal TextBox RoomMinimum;
		internal TextBox Announcement;
		internal TextBox Rooms;
		private bool _contentLoaded;

		public AnnouncementWindow(ClientHook clientHook)
		{
			this.InitializeComponent();
			this.ClientHook = clientHook;
		}

		private void SendClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(this.Announcement.Text))
			{
				return;
			}
			this.ClientHook.SendToServer(ConnectionType.Main, new C_StaffChatMessage
			{
				Type = StaffChatType.RoomModeration,
				Message = this.Announcement.Text
			});
		}

		private void NextClick(object sender, RoutedEventArgs e)
		{
			int roomMinimum = -1;
			int.TryParse(this.RoomMinimum.Text, out roomMinimum);
			string[] array = this.Rooms.Text.Split(new string[]
			{
				"\r\n",
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			string nextRoom = this.GetNextRoom(array, roomMinimum, ref num);
			if (string.IsNullOrEmpty(nextRoom))
			{
				this.Rooms.Text = string.Empty;
				return;
			}
			this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
			{
				Command = string.Format("room* {0}", nextRoom)
			});
			this.Rooms.Text = ((num == array.Length - 1) ? string.Empty : string.Join("\n", array, num + 1, array.Length - num - 1));
		}

		private string GetNextRoom(string[] rooms, int roomMinimum, ref int index)
		{
			for (int i = 0; i < rooms.Length; i++)
			{
				Match match = Regex.Match(rooms[i], "(?<roomName>.+) \\((?<community>.{2}) / (?<bulle>[^\\s]+)\\) : (?<playerCount>\\d+)");
				if (match.Success && !match.Groups["roomName"].Value.Contains("\u0003") && (roomMinimum == -1 || int.Parse(match.Groups["playerCount"].Value) >= roomMinimum))
				{
					index = i;
					return match.Groups["roomName"].Value;
				}
			}
			return null;
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/Transformice;component/views/announcementwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.RoomMinimum = (TextBox)target;
				return;
			case 2:
				((Button)target).Click += this.NextClick;
				return;
			case 3:
				((Button)target).Click += this.SendClick;
				return;
			case 4:
				this.Announcement = (TextBox)target;
				return;
			case 5:
				this.Rooms = (TextBox)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
