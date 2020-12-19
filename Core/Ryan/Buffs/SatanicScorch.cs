using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Core.Ryan.Buffs
{
    public class SatanicScorch : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Satanic Scorch");
            Description.SetDefault("The flames of hell engulf you");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegen -= 60;
            if (Main.rand.NextFloat() < 0.1f)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.Stone);
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegen -= 60;
            if (Main.rand.NextFloat() < 0.1f)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone);
            }
        }
    }
}