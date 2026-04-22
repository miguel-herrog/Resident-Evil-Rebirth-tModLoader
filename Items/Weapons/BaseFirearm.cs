using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
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
        public float reloadTime; // 60 frames = 1 segundo
        private int reloadTimer = 0;
        public virtual int TargetAmmoType => 0; // 0 por defecto (forzará error si el arma hija no lo define)
        public virtual int MagazineProjType => ModContent.ProjectileType<Projectiles.EmptyMagazineProj>();
        public virtual int ShellProjType => ModContent.ProjectileType<Projectiles.Shells.PistolShellProj>();
        public virtual bool EjectsCasingsOnFire => true;      // ¿Suelta casquillos al disparar?
        public virtual int ShellsEjectedOnReload => 1;       // ¿Cuántos objetos caen al recargar?
        public virtual int BulletPenetration => 1;
        public virtual float RecoilForce => 0f; // Por defecto no hay empuje
        public virtual SoundStyle? ReloadSound => null;

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
            if (player != null)
            {
                float directionFactor = player.direction;
                Vector2 barrelPosition = player.Center + new Vector2(directionFactor * 32f, 4f);

                // VFX de humo
                Dust.NewDust(barrelPosition, 4, 4, DustID.Smoke, directionFactor * 1f, -0.5f, 100, default, 0.8f);

                if (EjectsCasingsOnFire)
                {
                    Vector2 ejectionPosition = player.Center + new Vector2(directionFactor * 12f, 2f);
                    Vector2 shellVelocity = new Vector2(-directionFactor * 3f, -Main.rand.NextFloat(4f, 6f));
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), ejectionPosition, shellVelocity, ShellProjType, 0, 0, player.whoAmI);
                }
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

                    // Si encontramos munición
                    if (invItem.type == TargetAmmoType && invItem.stack > 0)                    
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
                    currentAmmo += bulletsFound;

                    if (ReloadSound.HasValue)
                    {
                        SoundEngine.PlaySound(ReloadSound.Value, player.Center);
                    }
                    
                    if (player != null)
                    {
                        // BUCLE DE EXPULSIÓN
                        for (int i = 0; i < ShellsEjectedOnReload; i++)
                        {
                            // Añadimos un poco de aleatoriedad a la velocidad para que no salgan todos pegados
                            Vector2 ejectVelocity = new Vector2(player.direction * Main.rand.NextFloat(-1f, 2f), Main.rand.NextFloat(1f, 4f));
                            Vector2 spawnPosition = player.Center + new Vector2(player.direction * 10f, 8f);

                            Projectile.NewProjectile(player.GetSource_ItemUse(Item), spawnPosition, ejectVelocity, MagazineProjType, 0, 0, player.whoAmI);
                        }
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

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Este método se ejecuta justo antes de crear la bala. 
        }

        // Usamos este hook para capturar la bala recién creada y cambiarle la penetración
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // --- FÍSICA DE RETROCESO 360 GRADOS ---
            if (RecoilForce > 0f)
            {
                // 'velocity' es la trayectoria exacta de la bala.
                Vector2 recoilDir = velocity;
                recoilDir.Normalize(); // Lo reducimos a longitud 1 para tener solo la dirección pura

                // Empujamos al jugador en la dirección opuesta al cursor (X e Y)
                player.velocity -= recoilDir * RecoilForce;
            }

            // --- CREACIÓN DE LA BALA ---
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].penetrate = BulletPenetration; 
            
            return false; 
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