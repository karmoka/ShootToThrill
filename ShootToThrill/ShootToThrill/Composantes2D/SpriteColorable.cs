using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
   public class SpriteColorable : Sprite
   {
      public SpriteColorable(Game jeu, string nomImage, Vector2 position, Vector2 dimension, Color couleur)
         :base(jeu, nomImage, position, dimension)
      {
         Couleur = couleur;
      }
      public SpriteColorable(Game jeu, string nomImage, Vector2 position, Vector2 dimension)
         : base(jeu, nomImage, position, dimension)
      {
      }

      public override void Initialize()
      {
         base.Initialize();
      }

      public void ChangerCouleur(Color couleur)
      {
         Couleur = couleur;
      }

      protected override void LoadContent()
      {

         base.LoadContent();
      }

       /// <summary>
       /// Gère la logique qui suprime le composant du système, incluant des components
       /// </summary>
       public virtual void CleanUp()
       {
       }

      public override void Update(GameTime gameTime)
      {
            base.Update(gameTime);
      }
   }
}
