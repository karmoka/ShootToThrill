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

        float Portée { get; set; }
        Vector3 Direction { get; set; }
        Vector3 PositionDépart { get; set; }

        List<DroiteCollision> DroitesCollision { get; set; }
        List<PrimitiveDeBaseAnimée2> EffetProjectiles { get; set; }

        string TypeProjectile { get; set; }

        public Projectiles(Game game, int nbProjectile, float portée, Vector3 direction, Vector3 positionDépart, string type)
            : base(game)
        {
            NbProjectile = nbProjectile;
            Portée = portée;
            Direction = direction;
            PositionDépart = positionDépart;
            TypeProjectile = type;
        }

        public override void Initialize()
        {
            DroitesCollision = new List<DroiteCollision>();
            EffetProjectiles = new List<PrimitiveDeBaseAnimée2>();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
