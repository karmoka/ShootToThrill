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
   public class CubeDeForce : ObjetPhysique, IPhysique, IModele3d
   {
      IModele3d ComposanteGraphique { get; set; }
      MCubeCollision Collision { get; set; }
      Vector3 Dimension { get; set; }

      public CubeDeForce(Game game, IModele3d composanteGraphique, Vector3 position, Vector3 dimension)
         : base(game, position)
      {
         Dimension = dimension;
         ComposanteGraphique = composanteGraphique;
      }

      public override void Initialize()
      {
         ComposanteGraphique.Initialize();
         Collision = new MCubeCollision(this.Position, Dimension, Vector3.Zero);
         base.Initialize();

         EstTangible = false;
      }

      public virtual Vector3 GetForce(Vector3 position)
      {
         return Vector3.Zero;
      }

      public override Collider GetCollider()
      {
         return Collision;
      }

      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }
      public ObjetPhysique GetObjetPhysique()
      {
         return this as ObjetPhysique;
      }
      public void SetCaméra(Caméra cam)
      {
         ComposanteGraphique.SetCaméra(cam);
      }
      public override void Draw(GameTime gameTime)
      {
         ComposanteGraphique.Draw(gameTime);
         base.Draw(gameTime);
      }

   }
}
