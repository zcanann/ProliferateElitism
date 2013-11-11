using System;

namespace River
{
#if WINDOWS || XBOX
    static class Entry
    {
        /* 
         * Bugs/Incomplete:
         * Fix draw bugs (or don't)
         * Perhaps call attack animation from emitter classes (so buffs don't do attacks and such. Or don't)
         * No (or small) wander CD on fear/blind
         * Maintain agro on hit for x amount of time (or be lazy and make it indefinite)
         * Finish Minimap
         * double select delay (don't equip -- clear selection)
         * 
         * --------------------------------------------------------
         * 
         * Implement:
         *   - Particle effects (corpse sparkles that depend on loot, skill effects, etc)
         *   - Heal out of combat
         *   - UI display for Health/(mana/energy/fury)
         *   - Elite enemies w/ skills DPS wells, ranged, etc
         *   - Saving (Stats, items, storage)
         *   - Loading saves
         */

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Main game = new Main())
            {
                game.Run();
            }
        }


    }
#endif
}

