using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


//TODO: ajouter le comportement du clic


namespace AtelierXNA
{
   public class Bouton : Sprite
   {
      public event EventHandler Pressed;
      public void OnPressed()
      {
         if(Pressed != null)
         {
            Pressed(this, EventArgs.Empty);
         }
      }

      bool EstActivé { get; set; }

      InputManager GestionnaireInput { get; set; }
      RessourcesManager<SpriteFont> GestionnaireFonts { get; set; }

      SpriteFont Fonts { get; set; }

      string NomFont { get; set; }
      string Texte { get; set; }

      public bool EstSelectionné { get; private set; }

      Color CouleurTexte { get; set; }
      Vector2 Échelle { get; set; }
      Vector2 PositionTexte { get; set; } //position centre
      Vector2 OrigineTexte { get; set; } //centre

      public Bouton(Game game, string nomImage, string nomFont, string texte, Vector2 position, Vector2 dimension)
         : base(game, nomImage,position,dimension)
      {
         Texte = texte;
         NomFont = nomFont;
         PositionTexte = position;
      }

      public override void Initialize()
      {
         EstActivé = true;
         EstSelectionné = false;
         CouleurTexte = Color.White;
         base.Initialize();
      }
      protected override void LoadContent()
      {
         GestionnaireInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         GestionnaireFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;

         Fonts = GestionnaireFonts.Find(NomFont);
         Vector2 dimensionTexte = Fonts.MeasureString(Texte);

         Échelle = new Vector2(Dimension.X / dimensionTexte.X, Dimension.Y / dimensionTexte.Y);
         OrigineTexte = new Vector2(dimensionTexte.X / 2, dimensionTexte.Y / 2);

         base.LoadContent();
      }

      public void ChangerÉtat()
      {
         EstSelectionné = !EstSelectionné;
         CouleurTexte = EstSelectionné ? Color.Red : Color.White;
      }

      public void Désactiver()
      {
         EstActivé = false;
         CouleurTexte = Color.Gray;
      }
      public void Activer()
      {
         EstActivé = true;
         CouleurTexte = EstSelectionné ? Color.Red : Color.White;
      }

      public override void Update(GameTime gameTime)
      {


         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
         spriteBatch.DrawString(Fonts, Texte, PositionTexte, CouleurTexte, 0, OrigineTexte, 1, SpriteEffects.None, 1);

      }
   }
}
