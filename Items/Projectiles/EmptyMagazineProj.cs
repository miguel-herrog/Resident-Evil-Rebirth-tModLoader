using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ResidentEvilRebirth.Items.Projectiles
{
    public class EmptyMagazineProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 10;
            Projectile.friendly = false; // No hace daño a enemigos
            Projectile.hostile = false;  // No hace daño al jugador
            Projectile.penetrate = -1;   // No se destruye al atravesar entidades
            Projectile.timeLeft = 120;   // Desaparece mágicamente tras 2 segundos (60 frames * 2)
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true; // Queremos que choque con el suelo
            Projectile.hide = false;
        }

        // AI() se ejecuta 60 veces por segundo. Aquí controlamos la gravedad y rotación.
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f; // Gravedad constante
            
            // 1. FRICCIÓN: Frena progresivamente el movimiento horizontal
            Projectile.velocity.X *= 0.98f; 

            // 2. LÍMITE DE ROTACIÓN: Solo rotamos si el cargador se mueve a una velocidad notable.
            // Usamos System.Math.Abs para comprobar la velocidad sin importar si es negativa o positiva.
            if (System.Math.Abs(Projectile.velocity.X) > 0.05f)
            {
                Projectile.rotation += Projectile.velocity.X * 0.1f;
            }        
            }

        // Se ejecuta cuando el proyectil toca un bloque sólido
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Choque horizontal (Paredes)
            if (Projectile.velocity.X != oldVelocity.X) 
                Projectile.velocity.X = -oldVelocity.X * 0.4f; // Rebota perdiendo el 60% de su fuerza
                
            // Choque vertical (Suelo / Techo)
            if (Projectile.velocity.Y != oldVelocity.Y) 
            {
                // Pierde casi toda su fuerza vertical al chocar.
                Projectile.velocity.Y = -oldVelocity.Y * 0.2f;
                
                // Si la fuerza del rebote es minúscula, la forzamos a 0 para que no tiemble en el suelo.
                if (System.Math.Abs(Projectile.velocity.Y) < 1f)
                {
                    Projectile.velocity.Y = 0f;
                }
                
                // Fricción al arrastrar por el suelo
                Projectile.velocity.X *= 0.5f; 
            }
            // Sonidos Futuros
            return false; 
        }
    }
}