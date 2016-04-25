using System;
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
    class Munition : Item
    {
        public string NomFusil { get; private set; }
        public Munition(Game game, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ, string nomFusil)
            : base(game, positionInitiale, rayon, nomModèle, intervalMAJ)
        {
            NomFusil = nomFusil;
        }
    }
}
