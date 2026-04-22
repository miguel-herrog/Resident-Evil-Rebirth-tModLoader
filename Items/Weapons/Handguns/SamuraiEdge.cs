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
        public override int TargetAmmoType => ModContent.ItemType<Ammo.Ammo9mm>();
        public override bool EjectsCasingsOnFire => true;      // Escuper casquillos      
        public override int ShellsEjectedOnReload => 1;        // Al recargar, no caen balas vacías
        public override int BulletPenetration => 1;            // No atraviesa a enemigos
        public override float RecoilForce => 1.5f;            // Retroceso muy ligero, fácil de controlar
        
        public override int MagazineProjType => ModContent.ProjectileType<Projectiles.EmptyMagazineProj>();
        public override int ShellProjType => ModContent.ProjectileType<Projectiles.Shells.PistolShellProj>();        
        public override SoundStyle? ReloadSound => new SoundStyle("ResidentEvilRebirth/Sounds/Custom/magnumReload");


        public override void SafeSetDefaults()
        {
            // --- MAGNUM STATISTICS ---
            maxAmmo = 15;           // A traditional revolver cylinder holds 6 rounds.
            reloadTime = 90;       // 60fps (90 = 1.5 seconds)

            Item.damage = 18;      
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 28;

            // --- SHOOTING GAME FEEL ---
            Item.useTime = 22;      // Mid-High.
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;    // Beastly knockback that stops enemies dead.
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;   // Highest rarity color.

            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;  // Bullet travels speed

            Item.useAmmo = AmmoID.None;
            Item.scale = 0.7f;     // Visual size adjustment

        }

        public override Vector2? HoldoutOffset()
        {
            // Eje X: Positivo aleja el arma hacia adelante. Negativo la echa hacia atrás (más pegada al cuerpo).
            // Eje Y: Positivo baja el arma. Negativo la sube.
            return new Vector2(-4f, 2f); 
        }
    }
}