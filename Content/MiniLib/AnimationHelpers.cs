using System;
using Terraria;

namespace TCIPMod.Content.MiniLib
{
    public static class AnimationHelper
    {
        #region Public Methods

        // Courtesy of Mercedes Benz
        public static bool PlayAnimationSimple(Projectile projectile, int[] frames, int framesBetween)
        {
            int totalFrames = framesBetween * frames.Length;

            int currentFrame = (projectile.frameCounter == 0) ? 0 : (int)Math.Floor((double)projectile.frameCounter / framesBetween);

            projectile.frame = frames[currentFrame];

            if (projectile.frameCounter < totalFrames - 1)
            {
                projectile.frameCounter++;
                return true;
            }
            else
            {
                projectile.frameCounter = 0;
                return false;
            }
        }
        #endregion Public Methods
    }
}