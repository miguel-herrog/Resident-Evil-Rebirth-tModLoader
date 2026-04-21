using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Ammo
{
    public class Ammo9mm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999; // Se pueden apilar hasta 999 balas
            Item.consumable = true; // Se gasta al usarse
            Item.value = Item.buyPrice(copper: 10);
            Item.rare = ItemRarityID.White;
            Item.ammo = Item.type; 
        }
    }
}