using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AtelierXNA
{
   public class MCubePhysiqueMobile : MCubePhysique
   {
      public MCubePhysiqueMobile(Game game, Vector3 position, Vector3 dimension, Vector3 vitesse, float masse)
         :base(game,position,dimension,vitesse,masse)
      {

      }
      public override void Initialize()
      {
         base.Initialize();
         EstImmuable = false;
      }
   }
}
