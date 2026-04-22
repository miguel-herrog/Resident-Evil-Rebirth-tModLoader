using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Weapons
{
    // Heredamos de NUESTRA clase.
    public class SilverSerpent : BaseFirearm 
    {
        // El nombre y la descripción se manejan en archivos de localización (.hjson).
        public override int TargetAmmoType => ModContent.ItemType<Ammo.MagnumRounds>();
        public override bool EjectsCasingsOnFire => false;      // Un revólver no escupe casquillos al disparar
        public override int ShellsEjectedOnReload => 6;        // Al recargar, caen las 6 balas vacías
        public override int BulletPenetration => 3;            // Cada bala atraviesa a 3 enemigos
        
        // Al recargar un revólver, lo que caen son casquillos, no un cargador:
        public override int MagazineProjType => ModContent.ProjectileType<Projectiles.PistolShellProj>();
        public override float RecoilForce => 5.5f;
        public override SoundStyle? ReloadSound => new SoundStyle("ResidentEvilRebirth/Sounds/Custom/magnumReload");

        public override void SafeSetDefaults()
        {
            // --- ESTADÍSTICAS DEL MAGNUM ---
            maxAmmo = 6;            // Un tambor de revólver tradicional tiene 6 balas.
            reloadTime = 192.96f;       // Es una recarga lenta y tensa.

            Item.damage = 85;       // Daño MASIVO (una pistola normal en este punto del juego tendría 15-20)
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 32;        
            Item.height = 32;       
            
            // --- GAME FEEL DEL DISPARO ---
            Item.useTime = 45;      // Muy lento. Tarda casi un segundo entre disparo y disparo.
            Item.useAnimation = 45; 
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.noMelee = true;    
            Item.knockBack = 9f;    // Empuje bestial que frena a los enemigos en seco.
            Item.value = Item.buyPrice(gold: 5); // Es un arma muy cara/rara.
            Item.rare = ItemRarityID.LightRed;   // Color de rareza más alto.
            
            Item.UseSound = SoundID.Item41; 
            Item.autoReuse = false; 

            Item.shoot = ProjectileID.Bullet; 
            Item.shootSpeed = 16f;  // La bala viaja muchísimo más rápido por la potencia de la pólvora.
            
            Item.useAmmo = AmmoID.None; 
            Item.scale = 0.75f;     // Tu ajuste de tamaño visual
        }

        public override Vector2? HoldoutOffset()
        {
            // Eje X: Positivo aleja el arma hacia adelante. Negativo la echa hacia atrás (más pegada al cuerpo).
            // Eje Y: Positivo baja el arma. Negativo la sube.
            return new Vector2(-6f, 2f); 
        }
    }
}