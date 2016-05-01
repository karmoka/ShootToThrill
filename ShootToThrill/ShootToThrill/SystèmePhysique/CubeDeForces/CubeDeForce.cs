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
        Vector3 NouvelleGravit� { get; set; }
        Vector3 Dimension { get; set; }

        public CubeDeForce(Game game, Vector3 position, Vector3 dimension, Vector3 nouvelleGravit�)
            : base(game , position)
        {
            Dimension = dimension;
            NouvelleGravit� = nouvelleGravit�;
        }

        public override void Initialize()
        {
            ComposanteGraphique = new CubeColor�(Game, 1f, Vector3.Zero, this.Position, Color.White, Dimension, 1/60f);
            Collision = new CubeCollision(this.Position, Dimension, Vector3.Zero);

            base.Initialize();
        }

        public override Vector3 GetForce(Vector3 position)
        {
            return NouvelleGravit�;
        }


        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
