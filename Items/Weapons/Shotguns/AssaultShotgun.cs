using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ResidentEvilRebirth.Items.Weapons.Shotguns
{
    public class AssaultShotgun : BaseFirearm 
    {
        // 1. CONECTAMOS LA MUNICIÓN
        public override int TargetAmmoType => ModContent.ItemType<Ammo.ShotgunShells>();
        
        // 2. CONFIGURACIÓN TÁCTICA
        public override bool EjectsCasingsOnFire => true;     // La escopeta de corredera expulsa el cartucho al disparar
        public override int ShellsEjectedOnReload => 0;       // Se recargan a mano, no cae ningún cargador vacío al suelo
        public override int BulletPenetration => 1;           // Los perdigones no suelen atravesar
        public override float RecoilForce => 2f;              // Buen retroceso, pero menos que el Magnum
        
        // Reutilizamos el casquillo de pistola por ahora (puedes crear uno rojo más adelante)
        public override int ShellProjType => ModContent.ProjectileType<Projectiles.PistolShellProj>();

        public override void SafeSetDefaults()
        {
            maxAmmo = 10;            // Magazine capacity
            reloadTime = 150;       // Reload duration in frames

            Item.damage = 12;       // Base damage per pellet (multiplied by 5 pellets for total ~60)
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 56;        // Sprite width
            Item.height = 22;       // Sprite height
            
            Item.useTime = 55;      // Time between shots (slow for pump action)
            Item.useAnimation = 55; 
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.noMelee = true;    
            Item.knockBack = 7f;    // Knockback force
            Item.value = Item.buyPrice(gold: 2); // Item value
            Item.rare = ItemRarityID.Orange;   // Rarity color
            
            Item.UseSound = SoundID.Item38; // Native shotgun sound
            Item.autoReuse = false; // No auto-fire

            Item.shoot = ProjectileID.Bullet; // Projectile type (standard bullet)
            Item.shootSpeed = 8f;  // Projectile speed (slower spread than magnum)
            
            Item.useAmmo = AmmoID.None; // Ammo handled manually
            Item.scale = 0.85f;     // Sprite scale 
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, 0f); // Ajuste para armas largas
        }

        // --- DISPERSIÓN ---
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 1. Call base class to handle recoil and fire the first pellet straight
            base.Shoot(player, source, position, velocity, type, damage, knockback);

            // 2. Fire 4 additional pellets with random angular spread
            int extraPellets = 1;
            for (int i = 0; i < extraPellets; i++)
            {
                // Rotate the original velocity by a random angle between -15 and 15 degrees
                Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                
                // Create the projectile with the spread velocity
                int p = Projectile.NewProjectile(source, position, spreadVelocity, type, damage, knockback, player.whoAmI);
                Main.projectile[p].penetrate = BulletPenetration; 
            }

            return false; // Return false since we've manually created all projectiles
        }
    }
}