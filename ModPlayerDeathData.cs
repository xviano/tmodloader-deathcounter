using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace DeathCount
{
	public class  ModPlayerDeathData : ModPlayer
	{
		public int deathCounter = 0;
		//public override void OnRespawn(Player player)
		//{
		//    deathCounter++;
		//}
		public ModPlayerDeathData() { 
		}

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			deathCounter++;
		}

		public override void OnEnterWorld(Player player)
		{
			// We can refresh UI using OnEnterWorld. OnEnterWorld happens after Load, so nonStopParty is the correct value.
			deathCounter = ModContent.GetInstance<ModPlayerDeathData>().deathCounter;
		}

		public override TagCompound Save() {
			return new TagCompound {
				{"died", deathCounter}
			};
        }

        public override void Load(TagCompound tag) {
			deathCounter = tag.GetInt("died");
		}

        public override void clientClone(ModPlayer clientClone) {
			ModPlayerDeathData clone = clientClone as ModPlayerDeathData;
			clone.deathCounter = deathCounter;
		}
		
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(deathCounter); 
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ModPlayerDeathData clone = clientPlayer as ModPlayerDeathData;
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(deathCounter);
			packet.Send();
		}
    }
}