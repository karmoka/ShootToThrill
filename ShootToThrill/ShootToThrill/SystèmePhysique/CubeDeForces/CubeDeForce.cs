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
    public class CubeDeForce : VolumeDeForce
    {
        Vector3 NouvelleGravité { get; set; }
        Vector3 Dimension { get; set; }

        public CubeDeForce(Game game, Vector3 position, Vector3 dimension, Vector3 nouvelleGravité)
            : base(game , position)
        {
            Dimension = dimension;
            NouvelleGravité = nouvelleGravité;
        }

        public override void Initialize()
        {
            ComposanteGraphique = new CubeColoré(Game, 1f, Vector3.Zero, this.Position, Color.White, Dimension, 1/60f);
            Collision = new CubeCollision(this.Position, Dimension, Vector3.Zero);

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
