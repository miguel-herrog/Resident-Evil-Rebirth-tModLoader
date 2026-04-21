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
                    // Solo si StartReload devuelve 'true', generamos el texto visual
                    if (firearm.StartReload()){
                        // Creamos un texto flotante sobre la hitbox del jugador.
                        // Color(R, G, B). En este caso: 255, 200, 0 es un Amarillo/Naranja de alerta.
                        Color textColor = new Color(255, 200, 0); 
                        CombatText.NewText(Player.Hitbox, textColor, "Reloading!");
                    }
                }                      
            }
        }
    }
}