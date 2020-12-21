using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Content.Ryan.Projectiles
{
	public class SatanicShivProj : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Satanic Shiv");
		}
        public override void SetDefaults()
        {
            projectile.thrown = true;
            projectile.damage = 35;
            projectile.width = 10;
            projectile.height = 15;
            projectile.hostile = false;
            projectile.friendly = true;
            drawOffsetX -= 3;
            drawOriginOffsetY -= 0;
        }
        public virtual void RunProjectileHitHandling()
        {
            Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<Pentagram>(), 0, 0, Main.player[Main.myPlayer].whoAmI);
            Main.PlaySound(SoundID.Item84, projectile.oldPosition);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            RunProjectileHitHandling();
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            RunProjectileHitHandling();
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            RunProjectileHitHandling();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center, 2);
            for (int count = 0; count < 20; count++)
            {
                if (Main.rand.NextFloat() < 0.25f)
                {
                    Vector2 position = projectile.Center;
                    Dust.NewDust(position, 30, 30, DustID.Stone, 0f, 0f, projectile.alpha);
                }

            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void PostAI()
        {
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }
        }
        // TODO: Continuous alpha depletion
        // TODO: On hit, generate a pentagram that engulfs enemies in "satanic scorch"
        // TODO: Make some dusts to make it seem clean
        // TODO: Fast-ish use-time.
        public override void AI()
        {
            projectile.alpha += 3;
            projectile.ai[0]++;
            if (projectile.ai[0] > 50)
            {
                projectile.rotation += 0.4f;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            projectile.velocity.Y += 0.25f;
            base.AI();
        }
    }
}