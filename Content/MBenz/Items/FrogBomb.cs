using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Content.MBenz.Items
{
    public sealed class FrogBomb
        : ModItem
    {
        #region Public Methods

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Frog, 1);
            recipe.AddIngredient(ItemID.Grenade, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Grenade);
            item.damage = (int)(item.damage * 2.5);
            item.width = 36;
            item.height = 32;
            item.knockBack *= 2;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = new LegacySoundStyle(SoundID.Frog, 1, Terraria.Audio.SoundType.Sound);
            item.autoReuse = false;
            item.value = 50000;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This frog is willing to croak to serve his country.");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<MBenz.Projectiles.FrogBomb>();
            position = player.MountedCenter;
            Point posReference = player.Bottom.ToTileCoordinates();

            foreach (var bomb in Main.projectile.Where(x => x.type.Equals(ModContent.ProjectileType<MBenz.Projectiles.FrogBomb>()) && x.owner.Equals(Main.LocalPlayer.whoAmI)))
            {
                bomb.Kill();
            }

            if ((Main.tile[posReference.X - 1, posReference.Y - 1].collisionType.Equals(1) && speedX < 0f)
                    || (Main.tile[posReference.X + 1, posReference.Y - 1].collisionType.Equals(1) && speedX > 0f)
                    || (Main.tile[posReference.X, posReference.Y].collisionType != 0 && speedY > 0f))
            {
                Projectile.NewProjectile(position + new Vector2(0, 2f), Vector2.Zero, type, damage, knockBack, player.whoAmI, 4f, -6969f);
                return false;
            }

            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        #endregion Public Methods
    }
}