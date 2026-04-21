using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Weapons
{
    // Heredamos de NUESTRA clase.
    public class ColtSAA : BaseFirearm 
    {
        // El nombre y la descripción se manejan en archivos de localización (.hjson).
        
        public override void SafeSetDefaults()
        {
            // Configuración de nuestra lógica personalizada (BaseFirearm)
            maxAmmo = 15; 
            reloadTime = 90;        // 90 frames = 1.5 segundos

            // Configuración nativa de Terraria (Item)
            Item.damage = 18;       // Daño base
            Item.DamageType = DamageClass.Ranged; // Daño a distancia
            Item.width = 32;        // Ancho del hitbox del sprite
            Item.height = 32;       // Alto del hitbox del sprite
            Item.useTime = 20;      // Cuántos frames tarda en disparar
            Item.useAnimation = 20; // Cuánto dura la animación (suele ser igual a useTime)
            Item.useStyle = ItemUseStyleID.Shoot; // Estilo de uso: pistola
            Item.noMelee = true;    // No hace daño cuerpo a cuerpo
            Item.knockBack = 5f;    // Empuje ligero
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item4; // Sonido de disparo estándar
            Item.autoReuse = false; // Hay que hacer clic por cada tiro

            // Configuración de proyectiles
            Item.shoot = ProjectileID.Bullet; // Dispara balas normales por ahora
            Item.shootSpeed = 12f;  // Velocidad de la bala
            
            // ILe decimos a Terraria que NO consuma balas del inventario,
            // porque estamos controlando la munición internamente con currentAmmo.
            Item.useAmmo = AmmoID.None; 
        }
    }
}