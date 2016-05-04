using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Railgun : Fusil
    {
        public Railgun(Game game, DescriptionFusil description, Vector3 positionInitiale, float rayon, float intervalMAJ)
            : base(game, description, positionInitiale, rayon, intervalMAJ)
        {
        }

        protected override void Tirer()
        {
            Vector3 direction = new Vector3(Direction.X, 0, -Direction.Y);

            Balle balle = new Balle(Game, Position, direction, Portée, Dommage);
            balle.Initialize();
            ListeProjectile.Add(balle);

            base.Tirer();
        }
    }
}
