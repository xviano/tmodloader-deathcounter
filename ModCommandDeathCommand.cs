using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;


namespace DeathCount
{
	public class ModCommandDeathCommand : ModCommand
	{
		public override CommandType Type
			=> CommandType.World;

		public override string Command
			=> "deaths";

		public override string Usage
			=> "/deaths playerName";

		public override string Description
			=> "Shows death count of player";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int player;
			int total = 0;
			int totalActive = 0;

			//command argument more than 1
			if (args.Length > 1)
			{
				throw new UsageException("Usage: /deaths playerName");
			}

			//command argument equals 1
			if (args.Length == 1)
			{
				//found player with name
				for (player = 0; player < Main.CurrentFrameFlags.ActivePlayersCount; player++)
				{
					if (Main.player[player].active && Main.player[player].name == args[0])
					{
						ModPlayerDeathData data = Main.player[player].GetModPlayer<ModPlayerDeathData>();
						caller.Reply(args[0] + " has died " + data.deathCounter + " times!");
						return;
					}
				}

				//can't find player
				if (player == Main.CurrentFrameFlags.ActivePlayersCount)
				{
					throw new UsageException("Could not find player: " + args[0]);
				}
			}

			//no command argument 
			for (int i = 0; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
			{
				ModPlayerDeathData data = Main.player[i].GetModPlayer<ModPlayerDeathData>();
				caller.Reply(Main.player[i].name + " has died " + data.deathCounter + " times!");
				totalActive += data.deathCounter;
			}
			caller.Reply("Total player deaths: " + totalActive);

			//foreach (Player p in Main.player)
			//{
			//	ModPlayerDeathData data = p.GetModPlayer<ModPlayerDeathData>();
			//	total += data.deathCounter;
			//}
			//caller.Reply("Total player deaths: " + total);
		}
	}
}
