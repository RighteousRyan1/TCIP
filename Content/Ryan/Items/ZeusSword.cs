using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TCIPMod.Content.Ryan.Projectiles;
using TCIPMod.Content.MiniLib;

namespace TCIPMod.Content.Ryan.Items
{
	public class ZeusSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Zeus' Lightningblade");
			Tooltip.SetDefault("'Lightning is a man's best friend.' - Zeus");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 7));
		}
		public override void SetDefaults()
		{
			item.damage = 180;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 12381;
			item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<ZeusSwordProj>();
			item.shootSpeed = 10;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.holdStyle = TCIPUseStyleID.Holstered;
		}
        public override void HoldStyle(Player player)
        {
			player.itemLocation = player.Center;
        }
        public override Vector2? HoldoutOffset()
        {
			return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true;
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