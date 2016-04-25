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

      bool EstD�sactiver { get; set; }
      Color COULEUR_D�FAUT = Color.White;
      Color CouleurActiv� { get; set; }
      Color CouleurD�sactiv�
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
         Couleur = COULEUR_D�FAUT;
         EstD�sactiver = false;
         NomFonts = nomFont;
         TexteBrut = texte;
      }
      public Textbox(Game game, string nomImage, string nomFont, Vector2 position, Vector2 dimension)
         : base(game, nomImage, position, dimension)
      {
         Couleur = COULEUR_D�FAUT;
         EstD�sactiver = false;
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

#region G�rerTexte

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
         string texteWrapp� = "";
         string[] Mots = texte.Split(' ');

         foreach (string mot in Mots)
         {
            if (Fonts.MeasureString(ligne + mot).Length() > boxWidth)
            {
               texteWrapp� += ligne + "\n";
               ligne = "";
            }
            ligne += mot + " ";
         }
         texteWrapp� += ligne;

         return texteWrapp�;
      }



#endregion

#region G�rerCouleur

      public void D�sactiver()
      {
         Couleur = CouleurD�sactiv�;
         EstD�sactiver = true;
      }
      public void Activer()
      {
         Couleur = CouleurActiv�;
         EstD�sactiver = false;
      }
      public void ChangerCouleurs(Color couleurActiv�)
      {
         CouleurActiv� = couleurActiv�;
         Couleur = couleurActiv�;
      }

#endregion
   }
}
