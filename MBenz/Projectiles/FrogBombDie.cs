using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TCIPMod.MBenz.Projectiles
{
    public class FrogBombDie
        : ModProjectile
    {
        #region Private Fields

        private readonly int[] _animFrames = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        private readonly int _frameTiming = 4;

        #endregion Private Fields

        #region Public Methods

        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            SimpleAnimLib.PlayAnimationSimple(projectile, _animFrames, _frameTiming);
        }

        // Overriding draw because terraria default sheet management is making me want to jump off a cliff.
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Texture2D texture = Main.projectileTexture[projectile.type];

            int startY = (projectile.height / (int)projectile.scale) * projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, projectile.width / (int)projectile.scale, projectile.height / (int)projectile.scale);

            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = sourceRectangle.Width / 2;

            Main.spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                Color.White,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = 64;
            projectile.height = 68;

            projectile.timeLeft = _animFrames.Count() * _frameTiming;

            projectile.penetrate = -1;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.ignoreWater = true;
            projectile.thrown = true;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            projectile.scale = 1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frog Bomb");
            Main.projFrames[projectile.type] = 9;
        }

        #endregion Public Methods
    }
}