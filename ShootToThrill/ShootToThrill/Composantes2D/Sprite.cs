using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
   public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
   {
      Color couleur_;

      protected Color AncienneCouleur { get; set; }
      protected Color Couleur
      {
         get { return couleur_; }
         set
         {
            AncienneCouleur = Couleur;
            couleur_ = value;
         }
      }

      RessourcesManager<Texture2D> GestionnaireTexture { get; set; }
      protected SpriteBatch spriteBatch { get; set; }
      Texture2D Image { get; set; }

      string NomImage { get; set; }

      Vector2 Échelle { get; set; } //Déformation du sprite
      public Vector2 Position {  get; protected set; } //Position du centre du sprite !!! marche mal avec l'origine
      public Vector2 Dimension { get; protected set; }//Hauteur et largeur du sprite
      protected Vector2 Origine { get; set; } //centre du sprite

      public Sprite(Game jeu, string nomImage, Vector2 position, Vector2 dimension)
         :base(jeu)
      {
         Couleur = Color.White;
         NomImage = nomImage;
         Position = position;
         Dimension = dimension;
      }

      public override void Initialize()
      {
         base.Initialize();
      }

      protected override void LoadContent()
      {
         GestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

         Image = GestionnaireTexture.Find(NomImage);
         Échelle = new Vector2(Dimension.X / Image.Width, Dimension.Y / Image.Height);
         Origine = new Vector2(Dimension.X / 2, Dimension.Y / 2);

         base.LoadContent();
      }

       /// <summary>
       /// Gère la logique qui suprime le composant du système, incluant des components
       /// </summary>
       public virtual void CleanUp()
       {
           Game.Components.Remove(this);
       }

      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }

       public void Déplacer(Vector2 nouvellePosition)
      {
          this.Position = nouvellePosition;
      }

      public override void Draw(GameTime gameTime)
      {
         spriteBatch.Draw(Image, Position - Origine, null, Couleur, 0, Vector2.Zero, Échelle, SpriteEffects.None, 1); 

         base.Draw(gameTime);
      }
   }
}
