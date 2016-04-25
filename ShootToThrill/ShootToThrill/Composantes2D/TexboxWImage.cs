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
   public class TextboxWImage : Sprite
   {
      RessourcesManager<SpriteFont> GestionnaireFonts { get; set; }
      SpriteFont Fonts { get; set; }
      string NomFonts { get; set; }
      string TexteBrut { get; set; }
      string Texte { get; set; }
      string NomImage { get; set; }
      Sprite Image { get; set; }

      public TextboxWImage(Game game, string nomImageBack, string nomImage, string nomFont, string texte, Vector2 position, Vector2 dimension)
          : base(game, nomImageBack, position, dimension)
      {
         NomFonts = nomFont;
         NomImage = nomImage;
         TexteBrut = texte;
      }
      public TextboxWImage(Game game, string nomImageBack, string nomImage, string nomFont, Vector2 position, Vector2 dimension)
          : base(game, nomImageBack, position, dimension)
      {
         NomFonts = nomFont;
         NomImage = nomImage;
         TexteBrut = "";
      }

      public void LoaderDocumentTxt(string CheminDocument)
      {
         TexteBrut = System.IO.File.ReadAllText(CheminDocument);
         //WarpTexte();
      }
      public void LoaderImage(string nomImage)
      {
          Image.CleanUp();
          Image = new Sprite(Game, nomImage, this.Position + new Vector2(0, Dimension.Y), this.Dimension);
          Image.Initialize();
          Game.Components.Add(Image);

          //WarpTexte();
      }

      public override void Initialize()
      {
         base.Initialize();
         //WarpTexte();
      }

      public string WarpTexte()
      {
         float boxWidth = Dimension.X;
         string line = "";
         string texte = "";
         string[] Mots = TexteBrut.Split(' ');

         foreach(string mot in Mots)
         {
            if(Fonts.MeasureString(line + mot).Length() > boxWidth)
            {
               texte += line + "\n";
               line = "";
            }
            line += mot + " ";
         }
         texte += line;

         return texte;
      }

      protected override void LoadContent()
      {
         GestionnaireFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
         Fonts = GestionnaireFonts.Find(NomFonts);
         Image = new Sprite(Game, NomImage, this.Position + new Vector2(0, Dimension.Y), this.Dimension);
         Image.Initialize();

         Game.Components.Add(Image);

         base.LoadContent();
      }

      public override void CleanUp()
      {
          Image.CleanUp();
          base.CleanUp();
      }


      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);

         spriteBatch.DrawString(Fonts, WarpTexte(), Position,Color.White,0,Dimension /2,1,SpriteEffects.None,0.1f);
      }
   }
}
