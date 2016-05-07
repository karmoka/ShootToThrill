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

namespace AtelierXNA
{
    class Soin : Item
    {
       int _soin;

        public int NombreSoin
       {
          get
          {
             Dispose();
             return _soin;
          }
          private set
          {
             _soin = value;
          }
       }


        public Soin(Game game, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ)
            : base(game, positionInitiale, rayon, nomModèle, intervalMAJ, true)
        {
            NombreSoin = 15;
        }
    }
}
