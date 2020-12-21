using Microsoft.Xna.Framework;
using TCIPMod.Content.Ryan.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCIPMod.Content.Ryan.Items
{
	public class SatanicShiv : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("'Engulf enemies with the satanist inside you!'");
		}
		public override void SetDefaults() 
		{
			item.damage = 35;
			item.thrown = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 10000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SatanicShivProj>();
			item.shootSpeed = 16;
			item.noMelee = true;
			item.noUseGraphic = true;
		}
        public override Vector2? HoldoutOffset()
        {
			return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
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