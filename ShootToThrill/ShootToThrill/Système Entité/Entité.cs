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

   public class Entité : Microsoft.Xna.Framework.DrawableGameComponent
   {
      EntitySystem système { get; set; }
      public int UniqueId { get; private set; }
      public int ObjectId { get; private set; }

      public Entité(Game game)
         : base(game)
      {
      }

      public override void Initialize()
      {
         système = Game.Services.GetService(typeof(EntitySystem)) as EntitySystem;
         UniqueId = système.ObtenirId(this);

         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }
   }
}
