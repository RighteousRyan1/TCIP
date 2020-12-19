using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.MBenz.Projectiles
{
    public class FrogBomb
        : ModProjectile
    {
        #region Public Fields

        public const int _hitboxHeight = 26;

        public const int _hitboxWidth = 36;

        #endregion Public Fields

        #region Private Fields

        private const int _collideFuse = 61;
        private const float _deployTimerEnd = -90f;
        private const float _deployTimerStart = -100f;

        private const float _fallAccel = 0.05f;
        private const float _fuseTimerStart = 180f;
        private const float _hopDeccel = 0.05f;
        private const float _hopSpeed = 3f;
        private const float _jumpDeccel = 0.1f;
        private const float _jumpSpeed = 9f;
        private const float _maxFallSpeed = 6f;
        private const float _minHopSpeed = 0.5f;
        private const float _slideDeccel = 0.2f;
        private const float _slideSpeed = 1f;

        private const int _spriteHeight = 72;
        private const int _spriteWidth = 64;

        private readonly Dictionary<int, Point> _adjustmentPerFrame = new Dictionary<int, Point>()
        {
            { 1, new Point(0, 4) },
            { 2, new Point(0, -2) },
            { 3, new Point(0, -8) }
        };

        private bool _isBeeping = false;

        #endregion Private Fields

        #region Private Properties

        private FrogBombState _currentState
        {
            get
            {
                return (FrogBombState)projectile.ai[0];
            }
            set
            {
                _stateTimer = 0;
                projectile.ai[0] = (float)value;
            }
        }

        private float _stateTimer
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        #endregion Private Properties

        #region Public Methods

        public override void AI()
        {
            switch (_currentState)
            {
                case FrogBombState.Deployed:
                    OnDeployLogic();
                    break;

                case FrogBombState.Standing:
                    StandingLogic();
                    break;

                case FrogBombState.Jumping:
                    JumpingLogic();
                    AerialLogic();
                    break;

                case FrogBombState.Falling:
                    AerialLogic();
                    break;

                case FrogBombState.Landing:
                    LandingLogic();
                    break;

                case FrogBombState.JumpSquat:
                    JumpSquatLogic();
                    break;
            }

            if (projectile.timeLeft <= _fuseTimerStart)
            {
                int secondRatio = (int)(projectile.timeLeft % 60f);
                _isBeeping = (secondRatio == 0 || secondRatio > 45);

                if (projectile.timeLeft % 60f == 0)
                {
                    Main.PlaySound(SoundID.Frog, projectile.position);
                    CombatText.NewText(new Rectangle((int)projectile.Top.X, (int)projectile.Top.Y, 10, 15), Color.Red, $"{projectile.timeLeft / 60f}");
                }
            }

            if (_stateTimer >= _deployTimerEnd && _currentState != FrogBombState.Jumping)
            {
                if (projectile.velocity.Y < _maxFallSpeed)
                    projectile.velocity.Y = MathHelper.Lerp(projectile.velocity.Y, _maxFallSpeed, _fallAccel);
            }

            _stateTimer++;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!target.friendly && projectile.Colliding(projectile.Hitbox, target.Hitbox))
            {
                if (projectile.timeLeft > _collideFuse)
                    projectile.timeLeft = _collideFuse;
            }

            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.Y != 0 && (projectile.velocity.Y > -1 && projectile.velocity.Y < 1) && _currentState.Equals(FrogBombState.Falling))
            {
                _currentState = FrogBombState.Landing;
            }

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            projectile.spriteDirection = projectile.direction;
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = Main.projectileTexture[projectile.type];

            int frameHeight = _spriteHeight;
            int startY = frameHeight * projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, _spriteWidth, _spriteHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = sourceRectangle.Width / 2;

            Color drawColor = projectile.GetAlpha(lightColor);

            if (_isBeeping)
            {
                drawColor = Color.OrangeRed;
            }

            var adjustment = (_adjustmentPerFrame.ContainsKey(projectile.frame)) ? _adjustmentPerFrame[projectile.frame].ToVector2() : new Vector2(0, 0);

            Main.spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition + adjustment,
                sourceRectangle,
                drawColor,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            if (_currentState.Equals(FrogBombState.Standing))
                projectile.frame = 1;

            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);

            Projectile.NewProjectile(projectile.Center,
                Vector2.Zero,
                ModContent.GetModProjectile(ModContent.ProjectileType<FrogBombDie>()).projectile.type,
                projectile.damage,
                projectile.knockBack,
                projectile.owner,
                1);

            return true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Grenade);
            projectile.aiStyle = 0;
            projectile.timeLeft = 480;
            projectile.width = _hitboxWidth;
            projectile.height = _hitboxHeight;
            projectile.manualDirectionChange = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frog Bomb");
            Main.projFrames[projectile.type] = 8;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        #endregion Public Methods

        #region Protected Methods

        protected void AerialLogic()
        {
            projectile.frame = DetermineAerialFrame(projectile.velocity.Y);
            projectile.velocity.X = MathHelper.Lerp(projectile.velocity.X, _minHopSpeed, _hopDeccel);
        }

        protected int DetermineAerialFrame(float currentYVelocity)
        {
            if (currentYVelocity < -2f)
            {
                return 4;
            }
            else if (currentYVelocity > -2f && currentYVelocity < 0f)
            {
                projectile.height = _hitboxHeight;
                return 5;
            }
            else if (currentYVelocity < 1f && currentYVelocity > 0f)
            {
                projectile.height = _hitboxHeight;
                return 6;
            }
            else
            {
                return 7;
            }
        }

        protected void JumpingLogic()
        {
            if (_stateTimer == 1)
            {
                projectile.velocity.Y = -_jumpSpeed;
                projectile.velocity.X = _hopSpeed * projectile.direction;
            }

            projectile.velocity.Y = MathHelper.Lerp(projectile.velocity.Y, 0f, _jumpDeccel);

            if (projectile.velocity.Y > -0.5f)
            {
                _currentState = FrogBombState.Falling;
            }
        }

        protected void JumpSquatLogic()
        {
            var jumpSquat = SimpleAnimLib.PlayAnimationSimple(projectile, new int[3] { 1, 2, 3 }, 3);

            if (!jumpSquat)
                _currentState = FrogBombState.Jumping;
        }

        protected void LandingLogic()
        {
            if (_stateTimer == -6969f)
            {
                projectile.direction = Main.LocalPlayer.direction;
                _stateTimer = 0;
            }

            var playing = SimpleAnimLib.PlayAnimationSimple(projectile, new int[2] { 1, 2 }, 3);

            if (_stateTimer == 0f)
            {
                projectile.velocity.Y = 0f;
                projectile.velocity.X = _slideSpeed * projectile.direction;
                Main.PlaySound(SoundID.Splash, projectile.position);
            }

            SlideLogic();

            if (!playing)
                _currentState = FrogBombState.Standing;
        }

        protected void OnDeployLogic()
        {
            _currentState = FrogBombState.Falling;
            _stateTimer = _deployTimerStart;
            projectile.direction = Main.LocalPlayer.direction;
        }

        protected void StandingLogic()
        {
            SlideLogic();

            projectile.frame = 0;

            if (_stateTimer == 30)
            {
                _currentState = FrogBombState.JumpSquat;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void SlideLogic()
        {
            if (projectile.velocity.X != 0)
            {
                projectile.velocity.X = MathHelper.Lerp(projectile.velocity.X, 0f, _slideDeccel);
            }
        }

        #endregion Private Methods
    }
}