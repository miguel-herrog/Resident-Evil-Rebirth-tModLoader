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
            // --- MAGNUM STATISTICS ---
            maxAmmo = 6;            // A traditional revolver cylinder holds 6 rounds.
            reloadTime = 192.96f;       // Slow and tense reload.

            Item.damage = 85;       // MASSIVE damage (a normal pistol at this game stage would have 15-20)
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 32;        
            Item.height = 32;       
            
            // --- SHOOTING GAME FEEL ---
            Item.useTime = 45;      // Very slow. Almost a second between shots.
            Item.useAnimation = 45; 
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.noMelee = true;    
            Item.knockBack = 9f;    // Beastly knockback that stops enemies dead.
            Item.value = Item.buyPrice(gold: 5); 
            Item.rare = ItemRarityID.LightRed;   // Highest rarity color.
            
            Item.UseSound = SoundID.Item41; 
            Item.autoReuse = false; 

            Item.shoot = ProjectileID.Bullet; 
            Item.shootSpeed = 16f;  // Bullet travels much faster due to powder power.
            
            Item.useAmmo = AmmoID.None; 
            Item.scale = 0.75f;     // Visual size adjustment
     
        }

        public override Vector2? HoldoutOffset()
        {
            // Eje X: Positivo aleja el arma hacia adelante. Negativo la echa hacia atrás (más pegada al cuerpo).
            // Eje Y: Positivo baja el arma. Negativo la sube.
            return new Vector2(-6f, 2f); 
        }
    }
}