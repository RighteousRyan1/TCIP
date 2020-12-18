using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TCIPMod.Core.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Core.Projectiles
{
	public class Pentagram : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Pentagram");
		}
        public override void SetDefaults()
        {
            projectile.damage = 0;
            projectile.width = 80;
            projectile.height = 80;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.scale = 1f;
            drawOffsetX -= 10;
            drawOriginOffsetY -= 10;
        }
        public override void PostAI()
        {
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (projectile.scale < 0.8f)
            {
                hitbox.Width = 50;
                hitbox.Height = 50;
            }
        }
        // TODO: Continuous alpha depletion
        // TODO: On hit, generate a pentagram that engulfs enemies in "satanic scorch"
        // TODO: Make some dusts to make it seem clean
        // TODO: Fast-ish use-time.
        public override void AI()
        {
            projectile.ai[0]++;
            if (projectile.ai[0] > 60)
            {
                projectile.alpha += 5;
            }
            // projectile.rotation += 0.1f;
            for (int l = 0; l < 200; l++)
            {
                NPC target = Main.npc[l];
                if (target.Hitbox.Intersects(projectile.Hitbox))
                {
                    if (target.CanBeChasedBy())
                    {
                        target.AddBuff(ModContent.BuffType<SatanicScorch>(), 300);
                    }
                }
            }
            if (projectile.scale < 0f)
            {
                projectile.Kill();
            }
            projectile.rotation += 0.01f;
            projectile.scale -= 0.0025f;
        }
    }
}