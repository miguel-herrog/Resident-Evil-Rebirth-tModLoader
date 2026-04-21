using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ResidentEvilRebirth.Items.Projectiles
{
    public class PistolShellProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 3;
            Projectile.height = 5;
            Projectile.friendly = false; 
            Projectile.hostile = false;  
            Projectile.penetrate = -1;   
            Projectile.timeLeft = 120; // 2 segundos en el suelo
            Projectile.tileCollide = true; 
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.25f; // Gravedad un pelín más fuerte por ser pesado
            Projectile.velocity.X *= 0.99f; // Menos fricción en el aire para que vuele más
            
            // Rota súper rápido en el aire según su velocidad horizontal (¡está girando a 1000 RPM!)
            Projectile.rotation += Projectile.velocity.X * 0.3f; 
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) 
                Projectile.velocity.X = -oldVelocity.X * 0.5f; 
                
            if (Projectile.velocity.Y != oldVelocity.Y) 
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.4f; // Rebota un poco más que el cargador
                
                // Forzamos el reposo si la velocidad es muy baja
                if (System.Math.Abs(Projectile.velocity.Y) < 1f)
                    Projectile.velocity.Y = 0f;
                
                Projectile.velocity.X *= 0.8f; 
                
                // TODO: Aquí añadiremos el sonido custom de casquillo metálico chocando
            }
            
            return false; 
        }
    }
}