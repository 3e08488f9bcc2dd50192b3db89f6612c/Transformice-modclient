using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Transformice.tfmStandalone
{
	public class ChatWindow : PinnableWindow, IComponentConnector
	{
		public bool IsApplicationClosing { get; set; }
		private static Regex UsernameRegex { get; set; }
		private static Regex GuestNameRegex { get; set; }
		internal ChatWindow Window;
		internal ChatTextBox ChatTextBox;
		private bool _contentLoaded;	
		
		[Dependency]
		public ChatWindowViewModel ChatWindowViewModel
		{
			get
			{
				return base.DataContext as ChatWindowViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public ChatWindow()
		{
			this.InitializeComponent();
			ChatWindow.UsernameRegex = new Regex("^\\+?[a-zA-Z0-9_]{3,}(#[\\d]{4})?$");
			ChatWindow.GuestNameRegex = new Regex("^\\*[a-zA-Z0-9_]{1,}$");
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = !this.IsApplicationClosing;
			base.Hide();
			this.ChatWindowViewModel.ChatHidden();
		}

		private void HandleLoaded(object sender, RoutedEventArgs e)
		{
			Random random = new Random();
			PropertyKey propertyKey = new PropertyKey(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
			WindowProperties.SetWindowProperty(this, propertyKey, "tfmChatWindow" + random.Next(1, int.MaxValue));
			ChatWindowViewModel chatWindowViewModel = this.ChatWindowViewModel;
			chatWindowViewModel.NewMessageReceived = (EventHandler)Delegate.Combine(chatWindowViewModel.NewMessageReceived, new EventHandler(delegate(object o, EventArgs args)
			{
				if ((!base.IsActive && !base.Topmost) || base.WindowState == WindowState.Minimized)
				{
					WindowFlash.FlashWindow(this);
				}
			}));
		}

		private void HandleActivated(object sender, EventArgs e)
		{
			WindowFlash.StopFlashing(this);
		}

		private void HandleRightMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void ChatBoxContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			string selectedText = this.ChatTextBox.Selection.Text;
			if (string.IsNullOrWhiteSpace(selectedText))
			{
				this.ChatTextBox.ContextMenu = null;
				return;
			}
			string[] array = selectedText.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Replace("[", string.Empty);
				array[i] = array[i].Replace("]", string.Empty);
			}
			System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
			System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem
			{
				Header = "Copy"
			};
			menuItem.Click += delegate(object o, RoutedEventArgs args)
			{
				SendKeys.SendWait("^c");
			};
			contextMenu.Items.Add(menuItem);
			if (array.Length == 1)
			{
				if (ChatWindow.UsernameRegex.IsMatch(array[0]))
				{
					string playerName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(array[0].ToLowerInvariant());
					contextMenu.Items.Add(new Separator());
					System.Windows.Controls.MenuItem menuItem2 = new System.Windows.Controls.MenuItem
					{
						Header = "/profile " + playerName.Replace("_", "__")
					};
					menuItem2.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("profile " + playerName);
					};
					contextMenu.Items.Add(menuItem2);
					System.Windows.Controls.MenuItem menuItem3 = new System.Windows.Controls.MenuItem
					{
						Header = "/casier " + playerName.Replace("_", "__")
					};
					menuItem3.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("casier " + playerName);
					};
					contextMenu.Items.Add(menuItem3);
					System.Windows.Controls.MenuItem menuItem4 = new System.Windows.Controls.MenuItem
					{
						Header = "/l " + playerName.Replace("_", "__")
					};
					menuItem4.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("l " + playerName);
					};
					contextMenu.Items.Add(menuItem4);
					System.Windows.Controls.MenuItem menuItem5 = new System.Windows.Controls.MenuItem
					{
						Header = "/relation " + playerName.Replace("_", "__")
					};
					menuItem5.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("relation " + playerName);
					};
					contextMenu.Items.Add(menuItem5);
					contextMenu.Items.Add(new Separator());
					System.Windows.Controls.MenuItem menuItem6 = new System.Windows.Controls.MenuItem
					{
						Header = "/search " + playerName.Replace("_", "__")
					};
					menuItem6.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("search " + playerName);
					};
					contextMenu.Items.Add(menuItem6);
					System.Windows.Controls.MenuItem menuItem7 = new System.Windows.Controls.MenuItem
					{
						Header = "/join " + playerName.Replace("_", "__")
					};
					menuItem7.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("join " + playerName);
					};
					contextMenu.Items.Add(menuItem7);
					System.Windows.Controls.MenuItem menuItem8 = new System.Windows.Controls.MenuItem
					{
						Header = "/ninja " + playerName.Replace("_", "__")
					};
					menuItem8.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("ninja " + playerName);
					};
					contextMenu.Items.Add(menuItem8);
					System.Windows.Controls.MenuItem menuItem9 = new System.Windows.Controls.MenuItem
					{
						Header = "/stalk " + playerName.Replace("_", "__")
					};
					menuItem9.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendClientCommand("stalk " + playerName);
					};
					contextMenu.Items.Add(menuItem9);
				}
				else if (ChatWindow.GuestNameRegex.IsMatch(array[0]))
				{
					string playerName = array[0];
					contextMenu.Items.Add(new Separator());
					System.Windows.Controls.MenuItem menuItem10 = new System.Windows.Controls.MenuItem
					{
						Header = "/l " + playerName.Replace("_", "__")
					};
					menuItem10.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("l " + playerName);
					};
					contextMenu.Items.Add(menuItem10);
					System.Windows.Controls.MenuItem menuItem11 = new System.Windows.Controls.MenuItem
					{
						Header = "/relation " + playerName.Replace("_", "__")
					};
					menuItem11.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("relation " + playerName);
					};
					contextMenu.Items.Add(menuItem11);
					contextMenu.Items.Add(new Separator());
					System.Windows.Controls.MenuItem menuItem12 = new System.Windows.Controls.MenuItem
					{
						Header = "/search " + playerName.Replace("_", "__")
					};
					menuItem12.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("search " + playerName);
					};
					contextMenu.Items.Add(menuItem12);
					System.Windows.Controls.MenuItem menuItem13 = new System.Windows.Controls.MenuItem
					{
						Header = "/join " + playerName.Replace("_", "__")
					};
					menuItem13.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("join " + playerName);
					};
					contextMenu.Items.Add(menuItem13);
					System.Windows.Controls.MenuItem menuItem14 = new System.Windows.Controls.MenuItem
					{
						Header = "/ninja " + playerName.Replace("_", "__")
					};
					menuItem14.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendCommand("ninja " + playerName);
					};
					contextMenu.Items.Add(menuItem14);
					System.Windows.Controls.MenuItem menuItem15 = new System.Windows.Controls.MenuItem
					{
						Header = "/stalk " + playerName.Replace("_", "__")
					};
					menuItem15.Click += delegate(object o, RoutedEventArgs args)
					{
						this.ChatWindowViewModel.SendClientCommand("stalk " + playerName);
					};
					contextMenu.Items.Add(menuItem15);
				}
			}
			contextMenu.Items.Add(new Separator());
			System.Windows.Controls.MenuItem menuItem16 = new System.Windows.Controls.MenuItem
			{
				Header = "Translate"
			};
			menuItem16.Click += delegate(object o, RoutedEventArgs args)
			{
				string str = WebUtility.UrlEncode(selectedText);
				Process.Start("https://translate.google.com/#auto/en/" + str);
			};
			contextMenu.Items.Add(menuItem16);
			this.ChatTextBox.ContextMenu = contextMenu;
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
			Uri resourceLocator = new Uri("/Transformice;component/views/chatwindow.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.Window = (ChatWindow)target;
				return;
			case 2:
				((System.Windows.Controls.ListBox)target).PreviewMouseRightButtonDown += this.HandleRightMouseDown;
				return;
			case 3:
				this.ChatTextBox = (ChatTextBox)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

	}
}
