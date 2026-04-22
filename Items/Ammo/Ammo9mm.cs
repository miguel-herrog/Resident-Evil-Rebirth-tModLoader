using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Ammo
{
    /// <summary>
    /// Represents Magnum Rounds ammo item for the Resident Evil Rebirth mod.
    /// This item serves as ammunition for magnum firearms.
    /// </summary> 
    public class Ammo9mm : ModItem
    {
        /// <summary>
        /// Sets the default properties for the Magnum Rounds item.
        /// This method is called by tModLoader to initialize the item's stats and behavior.
        /// </summary>
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999; 
            Item.consumable = true; 
            Item.value = Item.buyPrice(copper: 10);
            Item.rare = ItemRarityID.White;
            Item.ammo = Item.type; 
        }
    }
}