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
    public class Éclair : Projectile
    {
        const int NB_CROISEMENT = 5;

        Vector3[] PositionsPoints { get; set; }
        VertexPositionColor[] PointsColorés { get; set; }

        public Éclair(Game game, Vector3 position, Vector3 direction, float portée, int dommage)
            : base(game, portée, dommage, direction, position)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            PositionsPoints = new Vector3[NB_CROISEMENT + 1];
            PointsColorés = new VertexPositionColor[NB_CROISEMENT + 1];

            InitialiserSommets();
            EffetProjectile = new DroiteColorée(Game, PointsColorés, PositionDépart);
            EffetProjectile.Initialize();
        }

        void InitialiserSommets()
        {
            PositionsPoints[0] = Vector3.Zero;
            PointsColorés[0] = new VertexPositionColor(PositionsPoints[0], Color.White);

            Random générateurAléatoire = new Random();

            for (int i = 1; i <= NB_CROISEMENT; ++i)
            {
                PositionsPoints[i] = PointAléatoire(générateurAléatoire, 10, PositionDépart + Direction * i * Portée / (NB_CROISEMENT)) - PositionDépart;
                PointsColorés[i] = new VertexPositionColor(PositionsPoints[i], Color.LightBlue);
            }
        }

        Vector3 PointAléatoire(Random générateurAléatoire, int rayon, Vector3 point)
        {
            Vector3 vecteurAléatoire = new Vector3(générateurAléatoire.Next(0,20), générateurAléatoire.Next(0,20),générateurAléatoire.Next(0,20));
            float rayonAléatoire = générateurAléatoire.Next(0, rayon) / 10.0f;

            Vector3 directionRayon = Vector3.Cross(Direction, vecteurAléatoire);
            directionRayon.Normalize();
            directionRayon *= rayonAléatoire;

            return directionRayon + point;
        }
    }
}
