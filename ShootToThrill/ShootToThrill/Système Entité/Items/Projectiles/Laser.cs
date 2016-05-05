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


namespace AtelierXNA
{
    public class Laser : Projectile
    {
        Vector3[] PositionsPoints { get; set; }
        VertexPositionColor[] Points { get; set; }

        public Laser(Game game, Vector3 position, Vector3 direction, float portée, int dommage)
            : base(game, portée, dommage, direction, position)
        {
        }

        public override void Initialize()
        {
            Points = new VertexPositionColor[2];
            PositionsPoints = new Vector3[2];
            base.Initialize();
            InitialiserSommets();
            EffetProjectile = new DroiteColorée(Game, Points, PositionDépart);
            EffetProjectile.Initialize();
        }

        void InitialiserSommets()
        {
            PositionsPoints[0] = Vector3.Zero;
            PositionsPoints[1] = Direction * Portée;

            Points[0] = new VertexPositionColor(PositionsPoints[0], Color.Red);
            Points[1] = new VertexPositionColor(PositionsPoints[1], Color.Red);
        }
    }
}
