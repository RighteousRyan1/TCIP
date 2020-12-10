using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Items
{
	public class CoolBow : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("This is a basic modded sword.");
		}

		public override void SetDefaults() 
		{
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 1;
			item.useAnimation = 1;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item3;
			item.autoReuse = true;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 10;
		}
        public override Vector2? HoldoutOffset()
        {
			return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for (int projectieLoaderProjectileCountButItsBootlegAsFuckBecauseISaidSo = 0; projectieLoaderProjectileCountButItsBootlegAsFuckBecauseISaidSo < 10; projectieLoaderProjectileCountButItsBootlegAsFuckBecauseISaidSo++)
			{
				Projectile.NewProjectile(position + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100)), new Vector2(speedX, speedY), Main.rand.Next(0, ProjectileLoader.ProjectileCount), Main.rand.Next(0, 25), Main.rand.Next(0, 10));
			}
			return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return base.AltFunctionUse(player);
        }
        public override bool AllowPrefix(int pre)
        {
            return base.AllowPrefix(pre);
        }
        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            base.AnglerQuestChat(ref description, ref catchLocation);
        }
        public override bool UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}