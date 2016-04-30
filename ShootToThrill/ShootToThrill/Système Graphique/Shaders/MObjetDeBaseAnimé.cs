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
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class MObjetDeBaseAnimé : MObjetDeBase
   {
      public MObjetDeBaseAnimé(Game jeu, string nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         :base(jeu,nomModèle,échelleInitiale,rotationInitiale,positionInitiale)
      {
         
      }

      public bool Lacet { get; set; }
      public bool Roulis { get; set; }
      public bool Tangage { get; set; }

      public virtual void EffectuerMiseÀJour()
      {

      }
      public override void Initialize()
      {
         base.Initialize();
      }
      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }
   }
}
