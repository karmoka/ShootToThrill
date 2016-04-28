using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
    class RandomItemDrop : GameComponent
    {
        public RandomItemDrop(Game game)
            : base(game)
        {

        }

        public Item GetRandomItem(int nombre, Vector3 position)
        {
            Item item = null;
            if (nombre == 1)
            {
                item = new Munition(Game, position, 1, "ObjetDrop", 0.05f, "Railgun");
            }
            else if (nombre == 2)
            {
                item = new Munition(Game, position, 1, "ObjetDrop", 0.05f, "Machinegun");
            }
            else if (nombre == 3)
            {
                item = new Munition(Game, position, 1, "ObjetDrop", 0.05f, "Shotgun");
            }
            else if (nombre == 4)
            {
                item = new Soin(Game, position, 1, "ObjetDrop", 0.05f);
            }
            else if (nombre == 20)
            {
                item = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Railgun"), position, 1, 0.02f);
            }
            else if (nombre == 30)
            {
                item = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Machinegun"), position, 1, 0.02f);
            }
            else if (nombre == 40)
            {
                item = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Shotgun"), position, 1, 0.02f);
            }
            return item;
        }
    }
}
