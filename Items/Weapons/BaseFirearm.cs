using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ResidentEvilRebirth.Items.Ammo;
using Terraria.ID;

namespace ResidentEvilRebirth.Items.Weapons
{
    // 'abstract' significa que esta clase es una "plantilla". 
    public abstract class BaseFirearm : ModItem
    {
        // --- PROPIEDADES DE ESTADO ---
        public int maxAmmo;
        public int currentAmmo;
        public bool isReloading = false;
        public int reloadTime; // 60 frames = 1 segundo
        private int reloadTimer = 0;

        // Sellamos el SetDefaults de tModLoader para proteger nuestra lógica.
        // Obligamos a las armas hijas a usar SafeSetDefaults().
        public sealed override void SetDefaults()
        {
            SafeSetDefaults(); 
            currentAmmo = maxAmmo; 
        }

        // Método virtual que las armas específicas sobreescribirán para definir su daño, sprite, etc.
        public virtual void SafeSetDefaults() { }

        // HoldItem se ejecuta cada frame mientras el jugador tiene el arma en la mano.
        public override void HoldItem(Player player)
        {
            if (isReloading)
            {
                reloadTimer++;

                if (reloadTimer >= reloadTime)
                {
                    isReloading = false;
                    reloadTimer = 0;
                    
                    // Aquí añadiremos el sonido de finalización de recarga
                }
            }
        }

        // CanUseItem determina si el arma se dispara este frame.
        public override bool CanUseItem(Player player)
        {
            if (isReloading) return false;
            if (currentAmmo <= 0)
            {
                StartReload(player);
                return false;
            }
            return true;
        }

        // UseItem se ejecuta solo cuando el arma dispara con éxito.
        public override bool? UseItem(Player player)
        {
            currentAmmo--;
            // --- VFX DE DISPARO ---
            if (player != null)
            {
                // Dirección del jugador (-1 izquierda, 1 derecha)
                float directionFactor = player.direction;
                
                // OFFSET 1: Punta del cañón (Muzzle Flash).
                // Aproximadamente 32 píxeles adelante (cañón largo) y 4 hacia abajo (mano).
                Vector2 barrelPosition = player.Center + new Vector2(directionFactor * 32f, 4f);

                for (int i = 0; i < 1; i++) // Generamos 3 partículas de fuego
                {
                    Dust.NewDust(barrelPosition, 4, 4, DustID.Torch, directionFactor * 2f, -1f, 100, default, 1.5f);
                }
                // Añadimos humo ligero también en el cañón
                Dust.NewDust(barrelPosition, 4, 4, DustID.Smoke, directionFactor * 1f, -0.5f, 100, default, 0.8f);

                // OFFSET 2: Recámara de expulsión (Casquillo).
                // Aproximadamente 12 píxeles adelante (centro del cuerpo del arma) y 2 hacia abajo.
                Vector2 ejectionPosition = player.Center + new Vector2(directionFactor * 12f, 2f);

                // Velocidad del casquillo: Salta hacia arriba (-4f a -6f) y hacia atrás (-direction * 3f)
                Vector2 shellVelocity = new Vector2(-directionFactor * 3f, -Main.rand.NextFloat(4f, 6f));

                // Instanciamos el casquillo físico
                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item), 
                    ejectionPosition, 
                    shellVelocity, 
                    ModContent.ProjectileType<Projectiles.PistolShellProj>(),
                    0, 0, player.whoAmI
                );
            }
            return true;
        }

        // Método custom para iniciar la recarga manual o automática
        public bool StartReload(Player player)
        {
            // Solo recargamos si no estamos ya recargando y nos faltan balas
            if (!isReloading && currentAmmo < maxAmmo)
            {
                // 1. Calculamos cuántas balas necesitamos para llenar el cargador
                int bulletsNeeded = maxAmmo - currentAmmo;
                int bulletsFound = 0;

                // 2. Escaneamos el inventario del jugador (los 58 huecos estándar)
                for (int i = 0; i < 58; i++)
                {
                    Item invItem = player.inventory[i];

                    // Si encontramos nuestra munición de 9mm
                    if (invItem.type == ModContent.ItemType<Ammo.Ammo9mm>() && invItem.stack > 0)
                    {
                        // Si el montón tiene más o igual de las que necesitamos
                        if (invItem.stack >= (bulletsNeeded - bulletsFound))
                        {
                            invItem.stack -= (bulletsNeeded - bulletsFound);
                            bulletsFound = bulletsNeeded; // Ya tenemos todas
                            break; // Salimos del bucle, no necesitamos buscar más
                        }
                        // Si el montón tiene menos de las que necesitamos (ej. quedan 3 balas)
                        else
                        {
                            bulletsFound += invItem.stack;
                            invItem.TurnToAir(); // Vaciamos ese hueco del inventario
                        }
                    }
                }

                // 3. Si encontramos AL MENOS una bala, procedemos a recargar
                if (bulletsFound > 0)
                {
                    isReloading = true;           
                    reloadTimer = 0;
                    currentAmmo += bulletsFound;  // Sumamos las balas que hemos robado del inventario
                    Color textColor = new Color(255, 200, 0); 
                    CombatText.NewText(player.Hitbox, textColor, "Reloading!");
                    // --- FÍSICA DEL CARGADOR ---
                    if (player != null)
                    {
                        Vector2 ejectVelocity = new Vector2(player.direction * Main.rand.NextFloat(0.2f, 0.8f), Main.rand.NextFloat(1f, 3f));
                        // Aplicamos el Offset mejorado para que no lo tape el jugador
                        Vector2 spawnPosition = player.Center + new Vector2(player.direction * 24f, 8f);

                        // Efecto de humo
                        Dust.NewDust(spawnPosition, 4, 4, Terraria.ID.DustID.Smoke, 0f, -1f, 100, default, 1f);

                        Projectile.NewProjectile(
                            player.GetSource_ItemUse(Item), 
                            spawnPosition,                  
                            ejectVelocity,                  
                            ModContent.ProjectileType<Projectiles.EmptyMagazineProj>(), 
                            0,                              
                            0,                              
                            player.whoAmI                   
                        );
                    }
                    return true;
                }
            }
            return false; 
        }

        // Este método nos permite dibujar cosas "encima" del icono del arma en el inventario/barra
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            float ammoScale = 0.5f; // Escala por defecto (en mochila)

            // Comprobamos si el ítem que se está dibujando (this.Item) 
            // es el mismo ítem que el jugador local tiene seleccionado en su barra.
            if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] == Item)
            {
                ammoScale = 0.8f; // Escala grande (en mano)
            }

            string ammoText = currentAmmo.ToString();
            Color textColor = currentAmmo == 0 ? Color.Red : Color.White;
            
            // Ajuste leve de posición para la escala más grande
            Vector2 textPos = position + new Vector2(-12f, 4f); 

            Terraria.Utils.DrawBorderString(spriteBatch, ammoText, textPos, textColor, ammoScale);
        }

        // Este método nos permite modificar el texto que sale al pasar el ratón por el arma
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Creamos una nueva línea de texto con nuestras variables
            TooltipLine ammoLine = new TooltipLine(Mod, "AmmoCount", $"Munición: {currentAmmo} / {maxAmmo}");
            
            // Le damos un poco de color (Game Feel de UI)
            if (isReloading)
            {
                ammoLine.Text = "[ RECARGANDO ]";
                ammoLine.OverrideColor = Color.Orange;
            }
            else if (currentAmmo == 0)
            {
                ammoLine.OverrideColor = Color.Red;
            }
            else
            {
                ammoLine.OverrideColor = Color.LightGreen;
            }

            // Añadimos nuestra línea a la lista de textos del arma
            tooltips.Add(ammoLine);
        }
    }
}