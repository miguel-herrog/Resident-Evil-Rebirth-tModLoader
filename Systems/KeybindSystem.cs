using Terraria.ModLoader;

namespace ResidentEvilRebirth.Systems
{
    // ModSystem se usa para lógicas globales que no pertenecen a un objeto específico
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ReloadKeybind { get; private set; }

        public override void Load()
        {
            // Registramos la tecla en el motor de tModLoader.
            // "Reload Weapon" es el nombre que verá el jugador en el menú de Controles.
            // "R" es la tecla por defecto.
            ReloadKeybind = KeybindLoader.RegisterKeybind(Mod, "Reload Weapon", "R");
        }

        public override void Unload()
        {
            ReloadKeybind = null; 
        }
    }
}