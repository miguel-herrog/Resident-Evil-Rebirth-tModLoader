using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Ammo
{   
    /// <summary>
    /// Represents Shotgun Shells ammo item for the Resident Evil Rebirth mod.
    /// This item serves as ammunition for shotgun firearms.
    /// </summary>
    public class ShotgunShells : ModItem
    {
        /// <summary>
        /// Sets the default properties for the Shotgun Shells item.
        /// This method is called by tModLoader to initialize the item's stats and behavior.
        /// </summary>
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999; 
            Item.consumable = true; 
            Item.value = Item.buyPrice(copper: 20);
            Item.rare = ItemRarityID.Blue;
            Item.ammo = Item.type; 
        }
    }
}