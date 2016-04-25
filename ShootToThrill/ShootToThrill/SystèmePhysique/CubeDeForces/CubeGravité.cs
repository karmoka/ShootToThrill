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
    public class CubeGravit� : CubeDeForce
    {
        Vector3 NouvelleGravit� { get; set; }

        public CubeGravit�(Game game, IModele3d composanteGraphique, Vector3 position, Vector3 dimension, Vector3 nouvelleGravit�)
            : base(game,composanteGraphique, position, dimension)
        {
            NouvelleGravit� = nouvelleGravit�;
        }

        public override void Initialize()
        {
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
