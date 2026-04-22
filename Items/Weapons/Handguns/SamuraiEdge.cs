using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Weapons.Handguns
{
    // Heredamos de NUESTRA clase.
    public class SamuraiEdge : BaseFirearm 
    {
        // El nombre y la descripción se manejan en archivos de localización (.hjson).
        public override int TargetAmmoType => ModContent.ItemType<Ammo.MagnumRounds>();
        public override bool EjectsCasingsOnFire => true;      // Escuper casquillos      
        public override int ShellsEjectedOnReload => 0;        // Al recargar, no caen balas vacías
        public override int BulletPenetration => 1;            // No atraviesa a enemigos
        
        // Al recargar un revólver, lo que caen son casquillos, no un cargador:
        public override int MagazineProjType => ModContent.ProjectileType<Projectiles.EmptyMagazineProj>();
        public override SoundStyle? ReloadSound => new SoundStyle("ResidentEvilRebirth/Sounds/Custom/magnumReload");


        public override void SafeSetDefaults()
        {
            // --- MAGNUM STATISTICS ---
            maxAmmo = 15;           // A traditional revolver cylinder holds 6 rounds.
            reloadTime = 120;       // 60fps (120 = 2 seconds)

            Item.damage = 14;      
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 32;

            // --- SHOOTING GAME FEEL ---
            Item.useTime = 20;      // Very slow. Almost a second between shots.
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1f;    // Beastly knockback that stops enemies dead.
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;   // Highest rarity color.

            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;  // Bullet travels speed

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