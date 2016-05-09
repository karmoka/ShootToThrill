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
   public class TacheAcide : EntitéGraphiqueEtPhysique, IFaitMal
   {
      const float GROSSEUR_VERTICALE = 1f;

      CubeCollision Collision { get; set; }
      public int Domage { get; private set; }
      Vector2 Dimension { get; set; }

      public TacheAcide(Game game, string nomTextureTuile, Vector3 position, Vector2 dimension, int domage)
         : base(game, new TuileTexturée(game, 1, new Vector3(MathHelper.PiOver2, MathHelper.PiOver2, 0), position, dimension, nomTextureTuile, 1 / 60f), new ObjetPhysique(game, position))
      {
         Domage = domage;
         Dimension = dimension;
      }

      public override void Initialize()
      {
         base.Initialize();

         ComposantePhysique.EstImmuable = true;
         ComposantePhysique.EstTangible = false;

         Collision = new CubeCollision(this.Position, new Vector3(Dimension.X, GROSSEUR_VERTICALE, Dimension.Y), Vector3.Zero);
      }
      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }

      public override Collider GetCollider()
      {
         return Collision;
      }
   }
}
