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
   public class VolumeDeForce : ObjetPhysique, IPhysique, IModele3d
   {
      protected IModele3d ComposanteGraphique { get; set; }
      protected Collider Collision { get; set; }

      public VolumeDeForce(Game game , Vector3 position)
         : base(game, position)
      {

      }

      public override void Initialize()
      {
         base.Initialize();

         EstTangible = false;
         EstImmuable = true;
         Collision.Initialize();
         ComposanteGraphique.Initialize();
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
