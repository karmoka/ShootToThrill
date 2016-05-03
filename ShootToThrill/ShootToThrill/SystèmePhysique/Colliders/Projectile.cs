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
    public class Projectile : Microsoft.Xna.Framework.GameComponent
    {
        const float TEMPS_APPARITION = 0.05f;

        protected float Portée { get; set; }
        protected Vector3 Direction { get; set; }
        protected int Dommage { get; set; }
        protected Vector3 PositionDépart { get; set; }

        protected DroiteCollision DroiteCollision { get; set; }
        protected DroiteColorée EffetProjectile { get; set; }

        public bool Existe { get; private set; }
        float TempsÉcoulé { get; set; }

        public Projectile(Game game, float portée, int dommage, Vector3 direction, Vector3 positionDépart)
            : base(game)
        {
            Portée = portée;
            Dommage = dommage;
            Direction = direction;
            PositionDépart = positionDépart;       
        }

        public new void Update(GameTime gameTime)
        {
            TempsÉcoulé += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TempsÉcoulé >= TEMPS_APPARITION)
            {
                Existe = false;
            }
        }

        public override void Initialize()
        {
            TempsÉcoulé = 0;

            DroiteCollision = new DroiteCollision(Game, Direction, PositionDépart, Portée, Dommage);
            DroiteCollision.CoupDeFeu();

            Portée = DroiteCollision.Longueur;

            Existe = true;
            base.Initialize();
        }

        public void Draw(GameTime gameTime)
        {
            EffetProjectile.Draw(gameTime);
        }

        public void ChangerCaméra(Caméra cam)
        {
            EffetProjectile.ChangerCaméra(cam);
        }
    }
}
