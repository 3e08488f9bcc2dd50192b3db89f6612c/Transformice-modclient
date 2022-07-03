using System;
using System.Collections.Generic;

namespace Transformice.tfmClientHook.Messages
{
	public sealed class S_LoggedIn : S_Message
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public int TimePlayed { get; set; }
		public byte Community { get; set; }
		public int PlayerSessionId { get; set; }
		public bool IsPlayerAccount { get; set; }
		public List<byte> Roles { get; set; }
		public bool HasPublicAuthorization { get; set; }
		public const byte ArbitreRole = 3;
		public const byte ModeratorRole = 5;
		public const byte SentinelleRole = 7;
		public const byte AdministratorRole = 10;
		public const byte MapCrewRole = 11;
		public const byte LuaDevRole = 12;
		public const byte FunCorpRole = 13;
		public const byte FashionSquadRole = 15;
		
		public bool IsArbitre
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(ArbitreRole);
				}
				return false;
			}
		}

		public bool IsModerator
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(ModeratorRole);
				}
				return false;
			}
		}

		public bool IsSentinelle
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(SentinelleRole);
				}
				return false;
			}
		}

		public bool IsAdministrator
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(AdministratorRole);
				}
				return false;
			}
		}

		public bool IsMapCrew
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(MapCrewRole);
				}
				return false;
			}
		}

		public bool IsLuaDev
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(LuaDevRole);
				}
				return false;
			}
		}

		public bool IsFunCorp
		{
			get
			{
				if (Roles != null)
				{
					return Roles.Contains(FunCorpRole);
				}
				return false;
			}
		}

		public bool IsFashionSquad
		{
			get
			{
				if(Roles != null)
				{
					return Roles.Contains(FashionSquadRole);
				}
				return false;
			}
		}
		
		public override ByteBuffer GetBuffer()
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteInt(this.UserId);
			byteBuffer.WriteString(this.Name);
			byteBuffer.WriteInt(this.TimePlayed);
			byteBuffer.WriteByte(this.Community);
			byteBuffer.WriteInt(this.PlayerSessionId);
			byteBuffer.WriteBool(this.IsPlayerAccount);
			byteBuffer.WriteByte((byte)this.Roles.Count);
			this.Roles.ForEach(delegate(byte b)
			{
				byteBuffer.WriteByte(b);
			});
			byteBuffer.WriteBool(this.HasPublicAuthorization);
			return byteBuffer;
		}
	}
}
