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
   public class Textbox : Sprite
   {
      RessourcesManager<SpriteFont> GestionnaireFonts { get; set; }

      bool EstDésactiver { get; set; }
      Color COULEUR_DÉFAUT = Color.White;
      Color CouleurActivé { get; set; }
      Color CouleurDésactivé
      {
         get { return new Color(Couleur.R + 255, Couleur.G + 255,Couleur.B + 255, 100); }
      }

      SpriteFont Fonts { get; set; }
      string NomFonts { get; set; }
      string TexteBrut { get; set; }
      string Texte { get; set; }

      public Textbox(Game game, string nomImage, string nomFont, string texte, Vector2 position, Vector2 dimension)
         : base(game,nomImage,position,dimension)
      {
         Couleur = COULEUR_DÉFAUT;
         EstDésactiver = false;
         NomFonts = nomFont;
         TexteBrut = texte;
      }
      public Textbox(Game game, string nomImage, string nomFont, Vector2 position, Vector2 dimension)
         : base(game, nomImage, position, dimension)
      {
         Couleur = COULEUR_DÉFAUT;
         EstDésactiver = false;
         NomFonts = nomFont;

         TexteBrut = "";
      }

      public override void Initialize()
      {
         base.Initialize();
      }

      protected override void LoadContent()
      {
         GestionnaireFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
         Fonts = GestionnaireFonts.Find(NomFonts);

         Texte = WarpTexte(TexteBrut);

         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);

         spriteBatch.DrawString(Fonts, Texte, Position, Couleur, 0, Dimension / 2, 1, SpriteEffects.None, 0.1f);
      }

#region GérerTexte

      public void LoaderDocumentTxt(string CheminDocument)
      {
         Texte = WarpTexte(System.IO.File.ReadAllText(CheminDocument));
      }
      public void ChangerTexte(string texte)
      {
         Texte = WarpTexte(texte);
      }

      public string WarpTexte(string texte)
      {
         float boxWidth = Dimension.X;
         string ligne = "";
         string texteWrappé = "";
         string[] Mots = texte.Split(' ');

         foreach (string mot in Mots)
         {
            if (Fonts.MeasureString(ligne + mot).Length() > boxWidth)
            {
               texteWrappé += ligne + "\n";
               ligne = "";
            }
            ligne += mot + " ";
         }
         texteWrappé += ligne;

         return texteWrappé;
      }



#endregion

#region GérerCouleur

      public void Désactiver()
      {
         Couleur = CouleurDésactivé;
         EstDésactiver = true;
      }
      public void Activer()
      {
         Couleur = CouleurActivé;
         EstDésactiver = false;
      }
      public void ChangerCouleurs(Color couleurActivé)
      {
         CouleurActivé = couleurActivé;
         Couleur = couleurActivé;
      }

#endregion
   }
}
