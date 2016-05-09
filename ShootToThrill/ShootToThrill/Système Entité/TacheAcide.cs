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
      public int Domage { get; private set; }

      public TacheAcide(Game game, TuileTexturée composanteGraphique, Vector3 position, int domage)
         : base(game,composanteGraphique, new ObjetPhysique(game,position))
      {
         Domage = domage;
      }

      public override void Initialize()
      {
         base.Initialize();

         ComposantePhysique.EstImmuable = true;
         ComposantePhysique.EstTangible = false;
      }
      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }
   }
}
