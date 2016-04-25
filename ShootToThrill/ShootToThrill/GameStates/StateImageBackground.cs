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
   class StateImageBackground : GameComponent, GameState
   {
      Options OptionsJeu { get; set; }
      bool EstRectangleCouleur { get; set; }
      public bool EstActivé { get; set; }
      public bool EstDétruit { get; set; }

      Point Position { get; set; }
      Color couleur { get; set; }
      string NomTexture { get; set; }

       RessourcesManager<Texture2D> GestionnaireTextures { get; set; }
      SpriteBatch GestionnaireSprites { get; set; }
      Texture2D DummyTexture { get; set; }
      Texture2D ImageBack { get; set; }

      public StateImageBackground(Game game, Point position)
         :base(game)
      {
         EstRectangleCouleur = true;
         Position = position;
      }
      public StateImageBackground(Game game)
          : base(game)
      {
          EstRectangleCouleur = true;
          Position = Point.Zero;
      }
      public StateImageBackground(Game game, string nomTexture)
          : base(game)
      {
          EstRectangleCouleur = false;
          NomTexture = nomTexture;
      }

      public void Initialiser()
      {
         EstActivé = false;
         EstDétruit = false;

         OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
         GestionnaireTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         GestionnaireSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         ImageBack = GestionnaireTextures.Find(NomTexture);

         DummyTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
         DummyTexture.SetData(new Color[] { Color.White });
         couleur = Color.Red;
      }

      public void Cleanup()
      {

      }

      public void Pause()
      {
         EstActivé = false;
         couleur = Color.DarkRed;
      }

      public void Résumer()
      {
         EstActivé = true;
         couleur = Color.Red;
      }

      public void Update(GameTime gametime)
      {

      }

      public void Draw(GameTime gametime, float ordre)
      {
          if(EstRectangleCouleur)
          {
              GestionnaireSprites.Draw(DummyTexture, new Rectangle(Position.X, Position.Y, OptionsJeu.WindowWidth, OptionsJeu.WindowHeight), couleur);
          }
          else
          {
              GestionnaireSprites.Draw(ImageBack, new Rectangle(Position.X, Position.Y, OptionsJeu.WindowWidth, OptionsJeu.WindowHeight), Color.White);
          }
      }
   }
}
