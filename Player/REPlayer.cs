using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using ResidentEvilRebirth.Items.Weapons;
using Microsoft.Xna.Framework;

namespace ResidentEvilRebirth.Players
{
    // ModPlayer nos permite inyectar código en el personaje del jugador
    public class REPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            // Si la tecla acaba de ser presionada exactamente en este frame...
            if (Systems.KeybindSystem.ReloadKeybind.JustPressed)
            {
                // 1. Miramos qué objeto tiene el jugador seleccionado en su barra de acción
                Item heldItem = Player.inventory[Player.selectedItem];

                // 2. Pattern Matching: Comprobamos si ese objeto (ModItem) es una de nuestras armas (BaseFirearm).
                // Si lo es, C# automáticamente lo guarda en la variable temporal 'firearm'.
                if (heldItem.ModItem is BaseFirearm firearm)
                {
                    firearm.StartReload(Player);

                }                      
            }
        }
    }
}