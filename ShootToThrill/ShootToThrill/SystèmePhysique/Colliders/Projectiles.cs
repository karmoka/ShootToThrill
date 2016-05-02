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
    public class Projectiles : Microsoft.Xna.Framework.GameComponent
    {
        int NbProjectile { get; set; }
        int NbEffet { get; set; }

        float Portée { get; set; }
        Vector3 Direction { get; set; }
        Vector3 PositionDépart { get; set; }

        List<DroiteCollision> DroitesCollision { get; set; }
        List<PrimitiveDeBaseAnimée2> EffetProjectiles { get; set; }

        string TypeProjectile { get; set; }

        float TempsDepuisTir { get; set; }
        float TempsAffichage { get; set; }
        float IntervalleMAJ { get; set; }

        public Projectiles(Game game, int nbProjectile, int nbEffet, float portée, Vector3 direction, Vector3 positionDépart, string type, float tempsAffichage, float intervalleMAJ)
            : base(game)
        {
            NbProjectile = nbProjectile;
            NbEffet = NbEffet;
            Portée = portée;
            Direction = direction;
            PositionDépart = positionDépart;
            TypeProjectile = type;
            TempsAffichage = tempsAffichage;
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            DroitesCollision = new List<DroiteCollision>();
            EffetProjectiles = new List<PrimitiveDeBaseAnimée2>();
            TempsDepuisTir = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            TempsDepuisTir += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(TempsDepuisTir >= TempsAffichage)
            {
                EffacerProjectiles();
            }

            if(TempsDepuisTir >= IntervalleMAJ)
            {
                EffectuerMiseÀJour(gameTime);
            }

            base.Update(gameTime);
        }

        void EffacerProjectiles()
        {
            for (int i = 0; i < NbProjectile; ++i)
            {
                DroitesCollision.RemoveAt(i);
            }

            for (int i = 0; i < NbEffet; ++i)
            {
                EffetProjectiles.RemoveAt(i);
            }
        }

        void EffectuerMiseÀJour(GameTime gameTime)
        {
            if(TypeProjectile == "flame")
            {
                foreach(PrimitiveDeBase2 flame in EffetProjectiles)
                {
                    flame.Update(gameTime);
                }
            }
        }
    }
}
