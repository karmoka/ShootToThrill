// Auteur :       Raphael Croteau
// Fichier :      CubeDeForce.cs
// Description :  Volude de collision appliquant une force aux objets qui le touchent
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;



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
            Collision = new MCubeCollision(this.Position, Dimension, Vector3.Zero);

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
