using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class ÉtatPhysique : Microsoft.Xna.Framework.GameComponent
   {
      float? DuréÉtat { get; set; }
      float TempsDepuisCréation { get; set; }

      public float Charge { get; set; }

      public ÉtatPhysique(Game game, float charge, float? duré)
         :base(game)
      {
         DuréÉtat = duré;
         Charge = charge;
         TempsDepuisCréation = 0;
      }

      public override void Update(GameTime gameTime)
      {
         if(DuréÉtat != null)
         {
            TempsDepuisCréation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsDepuisCréation >= DuréÉtat)
               this.Dispose();
         }

         base.Update(gameTime);
      }
   }
}
