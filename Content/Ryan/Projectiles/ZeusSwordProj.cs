using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Content.Ryan.Projectiles
{
	public class ZeusSwordProj : ModProjectile
	{
        #region Vars

        private Rectangle _frame = new Rectangle(0, 0, 34, 42);
        #endregion
        public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Lightningblade");
            Main.projFrames[projectile.type] = 7;
		}
        public override void SetDefaults()
        {
            projectile.damage = 35;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.tileCollide = true;
            drawOriginOffsetY -= 30;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Main.GameUpdateCount % 4 == 0)
            {
                _frame.Y += 42;
            }
            if (_frame.Y > 42 * 6)
            {
                _frame.Y = 0;
            }
            // new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.frame * 42)
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, _frame, lightColor, projectile.rotation, _frame.Size() / 2, projectile.scale, SpriteEffects.None, 1f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item66, projectile.Bottom);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight, oldVelocity.X, oldVelocity.Y);
            }
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
            }
            return base.OnTileCollide(oldVelocity);
        }
        // TODO: Perhaps align this a bit more :fear:
        public override void PostAI()
        {
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = 8;
            hitbox.Height = 10;
        }
        public override void AI()
        {
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            bool pointsMet = Collision.SolidCollision(projectile.Center, 10, 1);
            projectile.ai[0]++;
            if (projectile.ai[0] > 80 && !pointsMet)
            {
                Dust.NewDustPerfect(projectile.Center, DustID.AncientLight);
                projectile.extraUpdates = 3;
                projectile.velocity.Y += 0.1f;
                projectile.velocity.X = 0;
                projectile.tileCollide = false;
            }
            if (pointsMet && projectile.ai[0] > 80)
            {
                projectile.velocity = Vector2.Zero;
                projectile.rotation = MathHelper.PiOver2 + MathHelper.PiOver4;
                Projectile.NewProjectile(projectile.Center - new Vector2(0, 130), Vector2.Zero, ModContent.ProjectileType<LightningStrike>(), 180, 20, Main.player[projectile.owner].whoAmI);
                projectile.Kill();
            }
            if (projectile.ai[0] > 60 && projectile.ai[0] < 80)
            {
                projectile.velocity = Vector2.Zero;
                if (projectile.ai[0] == 70 || projectile.ai[0] == 75)
                {
                    // This is quite possibly the must cluttered code I have ever made...
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(2, -8), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(4, -6), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(6, -4), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(8, -2), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(10, 0), fadeScale);

                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-2, 8), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-4, 6), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-6, 4), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-8, 2), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-10, 0), fadeScale);

                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(2, 8), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(4, 6), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(6, 4), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(8, 2), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(10, 0), fadeScale);

                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-2, -8), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-4, -6), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-6, -4), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-8, -2), fadeScale);
                    Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, new Vector2(-10, 0), fadeScale);
                }
                if (projectile.ai[0] == 70)
                {
                    // SoundID.Item102 is good
                    Main.PlaySound(SoundID.Item45, projectile.Center);
                }
                if (projectile.ai[0] == 75)
                {
                    Main.PlaySound(SoundID.Item102, projectile.Center);
                }
                projectile.rotation = MathHelper.PiOver2 + MathHelper.PiOver4;
            }
            if (projectile.ai[0] <= 60)
            {
                if (Main.GameUpdateCount % 2 == 0)
                {
                    Dust Lightning = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WitherLightning);
                    Lightning.velocity = Vector2.Zero;
                    Lightning.shader = Terraria.Graphics.Shaders.GameShaders.Armor.GetSecondaryShader(54, Main.LocalPlayer);
                }
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
                projectile.velocity.Y += 0.175f;
            }
        }
        private static int fadeScale = 1;
    }
}