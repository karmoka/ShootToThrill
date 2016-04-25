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
    public class CubeGravité : CubeDeForce
    {
        Vector3 NouvelleGravité { get; set; }

        public CubeGravité(Game game, IModele3d composanteGraphique, Vector3 position, Vector3 dimension, Vector3 nouvelleGravité)
            : base(game,composanteGraphique, position, dimension)
        {
            NouvelleGravité = nouvelleGravité;
        }

        public override void Initialize()
        {
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
