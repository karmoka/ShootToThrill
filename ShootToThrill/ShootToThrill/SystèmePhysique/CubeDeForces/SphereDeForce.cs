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
    public class SphereDeForce : VolumeDeForce
    {
        Vector3 NouvelleGravité { get; set; }
        float Rayon { get; set; }

        public SphereDeForce(Game game, Vector3 position, float rayon, Vector3 nouvelleGravité)
            : base(game , position)
        {
            Rayon = rayon;
            NouvelleGravité = nouvelleGravité;
        }

        public override void Initialize()
        {
            ComposanteGraphique = new MObjetDeBase(Game, "Scene2", 1f, Vector3.Zero, this.Position);
            Collision = new SphereCollision(this.Position, this.Rayon);

            base.Initialize();
        }

        public override Vector3 GetForce(Vector3 position)
        {
            return NouvelleGravité;
        }


        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
