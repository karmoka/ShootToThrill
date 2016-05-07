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
    public class Teslagun : Fusil
    {
        public Teslagun(Game game, DescriptionFusil description, Vector3 positionInitiale, float rayon, float intervalMAJ)
            : base(game, description, positionInitiale, rayon, intervalMAJ)
        {
           NomBruitFusil = "Electricity";
        }

        protected override void Tirer()
        {
            Vector3 direction = new Vector3(Direction.X, 0, -Direction.Y);
            Éclair éclair = new Éclair(Game, Position, direction, Portée, Dommage);
            éclair.Initialize();
            ListeProjectile.Add(éclair);

            base.Tirer();
        }
    }
}
