using Terraria;
using Terraria.ModLoader;

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
                    currentAmmo = maxAmmo;
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
                StartReload();
                return false;
            }
            return true;
        }

        // UseItem se ejecuta solo cuando el arma dispara con éxito.
        public override bool? UseItem(Player player)
        {
            currentAmmo--;
            return true;
        }

        // Método custom para iniciar la recarga manual o automática
        public void StartReload()
        {
            if (!isReloading && currentAmmo < maxAmmo)
            {
                isReloading = true;
                reloadTimer = 0;
                // Aquí añadiremos el sonido de inicio de recarga
            }
        }
    }
}