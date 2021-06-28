using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DeathCount
{
	class DeathCountMod : Mod
	{
		public DeathCountMod()
		{
			// By default, all Autoload properties are True. You only need to change this if you know what you are doing.
			//Properties = new ModProperties()
			//{
			//	Autoload = true,
			//	AutoloadGores = true,
			//	AutoloadSounds = true,
			//	AutoloadBackgrounds = true
			//};
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			DeathCountModMessageType msgType = (DeathCountModMessageType)reader.ReadByte();
			switch (msgType)
			{
				case DeathCountModMessageType.SyncPlayerDeath:
					byte playernumber = reader.ReadByte();
					ModPlayerDeathData modPlayer = Main.player[playernumber].GetModPlayer<ModPlayerDeathData>();
					modPlayer.deathCounter = reader.ReadInt32();
					// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
					break;

				case DeathCountModMessageType.DeathCountChanged:
					byte playernumber2 = reader.ReadByte();
					ModPlayerDeathData modPlayer2 = Main.player[playernumber2].GetModPlayer<ModPlayerDeathData>();
					modPlayer2.deathCounter = reader.ReadInt32();
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)DeathCountModMessageType.DeathCountChanged);
						packet.Write(playernumber2);
						packet.Write(modPlayer2.deathCounter);
						packet.Send(-1, playernumber2);
					}
					break;
				default:
					Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
	internal enum DeathCountModMessageType : byte
	{
		SyncPlayerDeath,
		DeathCountChanged,
	}
}
