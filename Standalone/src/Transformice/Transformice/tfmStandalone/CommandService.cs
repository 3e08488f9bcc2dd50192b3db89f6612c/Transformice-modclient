using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Threading;
using Newtonsoft.Json;
using Transformice.tfmClientHook;
using Transformice.tfmClientHook.Messages;

namespace Transformice.tfmStandalone
{
	public sealed class CommandService
	{
		private ClientHook ClientHook { get; }
		private GameInfo GameInfo { get; }
		private CustomCommandInterface CustomCommandInterface { get; }
		private WindowService WindowService { get; }

		public CommandService(ClientHook clientHook, MessageInterceptor messageInterceptor, GameInfo gameInfo, CustomCommandInterface customCommandInterface, WindowService windowService)
		{
			this.ClientHook = clientHook;
			this.GameInfo = gameInfo;
			this.CustomCommandInterface = customCommandInterface;
			this.WindowService = windowService;
			messageInterceptor.CommandSent = (EventHandler<MessageInterceptor.CommandSendEventArgs>)Delegate.Combine(messageInterceptor.CommandSent, new EventHandler<MessageInterceptor.CommandSendEventArgs>(delegate(object sender, MessageInterceptor.CommandSendEventArgs e)
			{
				e.SendToServer = this.ProcessCommand(e.Message.Command);
			}));
		}

		public bool ProcessCommand(string fullCommand)
		{
			List<string> commandParams = fullCommand.Split(' ').ToList();
			if (commandParams.Count == 0)
			{
				return true;
			}
			string text = commandParams[0].ToLowerInvariant();
			commandParams.RemoveAt(0);
			switch (text)
			{
				case "ban":
				case "iban":
				case "mute":
				case "imute":
				{
					int result = -1;
					List<string> list = new List<string>();
					while (commandParams.Count > 0)
					{
						string text3 = commandParams[0];
						commandParams.RemoveAt(0);
						if (int.TryParse(text3, out result))
						{
							break;
						}
						list.Add(text3);
					}
					string text4 = string.Join(" ", commandParams);
					if (result < 0 || list.Count < 1)
					{
						break;
					}
					foreach (string item in list)
					{
						this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
						{
							Command = text + " " + item + " " + result + " " + text4
						});
					}
					return false;
				}
				case "banhack":
				case "ibanhack":
				case "chercher":
				case "search":
				case "kick":
				case "relation":
				case "mumute":
				case "prison":
				case "roomkick":
				case "l":
				{
					if (commandParams.Count == 0)
					{
						return true;
					}
					if (int.TryParse(commandParams[commandParams.Count - 1], out var result2))
					{
						for (int j = 0; j < commandParams.Count - 1; j++)
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = text + " " + commandParams[j] + " " + result2
							});
						}
					}
					else
					{
						foreach (string item2 in commandParams)
						{
								this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = text + " " + item2
							});
						}
					}
					return false;
				}
				case "casier":
				{
					if (commandParams.Count == 0)
					{
						return true;
					}
					if (commandParams[commandParams.Count - 1].ToLowerInvariant() == "true")
					{
						for (int i = 0; i < commandParams.Count - 1; i++)
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = "casier " + commandParams[i] + " true"
							});
						}
					}
					else
					{
						foreach (string item3 in commandParams)
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = "casier " + item3
							});
						}
					}
					return false;
				}
				case "client":
                {
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/lc [comment] - Log comment directly to Log.txt"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/stalk [name] - Join a player, report for hacking, then ninja"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/tignore [name1] [name2] - Add/remove specified players to ignore whispers from. The list is cleared when the client is closed"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/lsign - List temporarily ignored players"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/igcommu [community1] [community2] - Add/remove specified communities to ignore whispers from"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/lsigcommu - List ignored communities"
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/wlcommu [community1] [community2] - Add/remove specified communities to the whitelist. Whitelisted communities are the only communities you can receive whispers from."
					});
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "/lswlcommu - List whitelisted communities"
					});
					return false;
				}
				case "announce":
				{
					TaskHelpers.UiInvoke(delegate
					{
						WindowService.ShowAnnouncementWindow();
					});
					return false;
				}
				case "lc":
				{
					string value = "• [" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat) + "] " + string.Join(" ", commandParams);
					using (StreamWriter streamWriter = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "Log.txt"))
					{
						streamWriter.WriteLine(value);
					}
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "Comment logged."
					});
					return false;
				}
				case "stalk":
                {
					if (commandParams.Count > 0)
					{
						TaskHelpers.UiInvoke(delegate
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = "ninja " + commandParams[0]
							});
							DispatcherTimer timer3 = new DispatcherTimer
							{
								Interval = TimeSpan.FromMilliseconds(1000.0)
							};
							timer3.Tick += delegate
							{
								this.ClientHook.SendToServer(ConnectionType.Main, new C_ReportPlayer
								{
									Type = ReportType.Hack,
									Name = commandParams[0]
								});
								timer3.Stop();
							};
							timer3.Start();
							DispatcherTimer timer = new DispatcherTimer
							{
								Interval = TimeSpan.FromMilliseconds(2000.0)
							};
							timer.Tick += delegate
							{
								this.ClientHook.SendToServer(ConnectionType.Main, new C_ChangeRoom
								{
									RoomName = $"*{new Random().Next(348729, int.MaxValue)} bootcamp"
								});
								timer.Stop();
							};
							timer.Start();
							DispatcherTimer timer2 = new DispatcherTimer
							{
								Interval = TimeSpan.FromMilliseconds(6000.0)
							};
							timer2.Tick += delegate
							{
								this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
								{
									Command = "ninja " + commandParams[0]
								});
								timer2.Stop();
							};
							timer2.Start();
						});
					}
					return false;
				}
				case "tignore":
                {
					foreach (string item4 in commandParams)
					{
						string text5 = item4.ToLowerInvariant();
						if (this.GameInfo.TemporaryIgnoreList.Contains(text5))
						{
							this.GameInfo.TemporaryIgnoreList.Remove(text5);
							this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
							{
								Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text5) + " unignored."
							});
						}
						else
						{
							this.GameInfo.TemporaryIgnoreList.Add(text5);
							this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
							{
								Message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text5) + " ignored."
							});
						}
					}
					return false;
				}
				case "lsign":
				{
					string text2 = string.Join(", ", this.GameInfo.TemporaryIgnoreList.Select((string n) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(n)));
					this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
					{
						Message = "Ignored Players: " + text2
					});
					return false;
				}
				case "modinfo":
                {
					if (commandParams.Count > 0)
					{
						string name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(commandParams[0].ToLowerInvariant());
						using WebClient webClient = new WebClient();
						webClient.Headers[HttpRequestHeader.Authorization] = "Null TnVsbGlmaWNhdG9yXyA2MzRAIz8=";
						webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
						webClient.Headers["From"] = GameInfo.Name;
						webClient.DownloadStringCompleted += delegate (object sender, DownloadStringCompletedEventArgs args)
						{
							if (args.Error == null)
							{
								ModInfo modInfo = JsonConvert.DeserializeObject<ModInfo>(args.Result);
								string text7 = string.Format("<BV>Mod Info for <N>{0}<BL>\n• ", name);
								foreach (string text8 in modInfo.community)
								{
									text7 += string.Format("[{0}] ", text8.ToUpperInvariant());
								}
								text7 += this.GetNameString(modInfo.main, modInfo.role);
								if (modInfo.alts != null && modInfo.alts.Count > 0)
								{
									text7 += "\n<BV>Alts: ";
									foreach (ModAlt modAlt in modInfo.alts)
									{
										text7 += string.Format("\n<BL>• {0}", this.GetNameString(modAlt.nick, modAlt.role));
									}
								}
								this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
								{
									Message = text7
								});
								return;
							}
							else if (args.Error is WebException)
							{
								Stream stream = ((WebException)args.Error).Response?.GetResponseStream();
								if (stream != null)
								{
									using (StreamReader streamReader = new StreamReader(stream))
									{
										switch (JsonConvert.DeserializeObject<ModInfoError>(streamReader.ReadToEnd()).code)
										{
											case "10014":
											case "10018":
											case "10019":
												this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
												{
													Message = "<R>• Error: Mod info access denied"
												});
												break;
											case "10020":
												this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
												{
													Message = "<R>• Error: Search term is not valid"
												});
												break;
											case "10017":
												this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
												{
													Message = "<R>• Error: Staff member not found"
												});
												break;
											default:
												this.ClientHook.SendToClient(ConnectionType.Main, new S_GenericChatMessage
												{
													Message = "<R>• Error: API unavailable"
												});
												break;
										}
									}
								}
							}
						};
						webClient.DownloadStringAsync(new Uri($"https://staff801.com/api/modinfo/{name}"));
					}
					return false;
				}
				case "test":
                {
					break;
                }
				default:
                {
					return this.CustomCommandInterface.Execute(text, fullCommand, delegate (string c)
					{
						if (c.Split(' ')[0] == "room")
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_ChangeRoom
							{
								RoomName = $"{c.Substring(5)}"
							});
						}
						else
						{
							this.ClientHook.SendToServer(ConnectionType.Main, new C_Command
							{
								Command = c
							});
						}
					});
				}
			}
			return true;
		}
		private string GetNameString(string name, string role)
		{
			string arg = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
			if (role == "admin")
			{
				return string.Format("<font color=\"#EB1D51\">{0} (Administrator)", arg);
			}
			if (role == "public")
			{
				return string.Format("<J>{0} (Public Moderator)", arg);
			}
			if (role == "private")
			{
				return string.Format("<BL>{0} (Private Moderator)", arg);
			}
			if (role == "trial")
			{
				return string.Format("<ROSE>{0} (Trial Moderator)", arg);
			}
			if (!(role == "arb"))
			{
				return string.Format("<N>{0} (Regular Player)", arg);
			}
			return string.Format("<V>{0} (Arbitre)", arg);
		}
	}
}
