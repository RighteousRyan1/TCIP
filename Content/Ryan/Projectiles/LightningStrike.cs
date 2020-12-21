using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TCIPMod.Content.MiniLib;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Content.Ryan.Projectiles
{
	public class LightningStrike : ModProjectile
	{
        private readonly int[] _frameCount = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private readonly int _frameDelay = 3;

        #region Method Specific Variables


        #endregion

        /// <summary>
        /// 'Shorthand' for projectile.
        /// </summary>
        private float StrikeTimer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Lightning Strike");
            Main.projFrames[projectile.type] = 13;
		}
        public override void SetDefaults()
        {
            projectile.width = 258;
            projectile.height = 261;
            projectile.damage = 180;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = 100;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }
        public override bool CanHitPvp(Player target)
        {
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && target.CanBeChasedBy())
            {
                target.velocity.X = Main.rand.Next(-2, 2);
                target.velocity.Y = Main.rand.Next(-8, -4);
            }
        }
        public override void AI()
        {
            StrikeTimer++;
            if (StrikeTimer == 1)
            {
                int num = Main.rand.Next(0, 10);
                SoundEffectInstance thunderClap = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, $"Sounds/Custom/Ryan/Thunder{num}"), projectile.Bottom);
            }
            if (StrikeTimer > 12 && StrikeTimer < 25)
            {
                projectile.width = 258;
                projectile.height = 261;
            }    
            else
            {
                projectile.width = 0;
                projectile.height = 0;
            }
            AnimationHelper.PlayAnimationSimple(projectile, _frameCount, _frameDelay);
            if (projectile.frame >= 12)
            {
                projectile.Kill();
            }
        }
    }
}