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
    /// <summary>
    /// Classe de débug, permet d'affiché de l'information sur certaine composantes à l'écran
    /// </summary>
   class ScreenManager : DrawableGameComponent
   {
       Options OptionJeu { get; set; }

      InformationGame InfoJeu { get; set; }
      Texture2D DummyTexture { get; set; }
      Rectangle DivisionHorizontale { get; set; }
      Rectangle DivisionVerticale { get; set; }

      int InfoWidth;
      int HeigthByItem = 20;

      float IntervalMAJ { get; set;}
      float TempsDepuisMAJ { get; set; }
      int NbFoisDepuisUpdate {get;set;}
      string FPS { get; set; }

      RessourcesManager<Texture2D> GestionnaireTexture { get; set; }
      RessourcesManager<SpriteFont> GestionnaiteFonts { get; set; }
      SpriteBatch spriteBatch { get; set; }
      List<Object> ListeObjet { get; set; }
      List<DrawableGameComponent> ListeDrawableGameComponent { get; set; }

      Texture2D ImageBack { get; set; }
      SpriteFont Fonts { get; set; }

      Rectangle RectFenetre { get; set; }
      Vector2 Position { get; set; }

      public ScreenManager(Game game, Vector2 position, InformationGame infoJeu)//, float intervalMAJ)
         : base(game)
      {
         InfoJeu = infoJeu;
         Position = position;
      }

      public override void Initialize()
      {
         OptionJeu = Game.Services.GetService(typeof(Options)) as Options;
         ListeObjet = new List<object>();
         ListeDrawableGameComponent = new List<DrawableGameComponent>();

         NbFoisDepuisUpdate = 0;
         TempsDepuisMAJ = 0;
         FPS = "0";

         InfoWidth = OptionJeu.WindowWidth / 2;
         DivisionVerticale = new Rectangle(OptionJeu.WindowWidth / 2, 0, 5,OptionJeu.WindowHeight);
         DivisionHorizontale = new Rectangle(0, OptionJeu.WindowHeight / 2, OptionJeu.WindowWidth, 5);

         base.Initialize();
      }

      protected override void LoadContent()
      {
         GestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         GestionnaiteFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
         spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

         ImageBack = GestionnaireTexture.Find("tile_12");
         Fonts = GestionnaiteFonts.Find("ArialDebug");

         RectFenetre = new Rectangle((int)Position.X, (int)Position.Y, OptionJeu.WindowWidth/2, 0);

         DummyTexture = new Texture2D(GraphicsDevice, 1, 1);
         DummyTexture.SetData(new Color[] { Color.White });

         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
         TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
         ++NbFoisDepuisUpdate;

         if(TempsDepuisMAJ >=1)
         {
            
            FPS = (NbFoisDepuisUpdate).ToString();
            NbFoisDepuisUpdate = 0;
            TempsDepuisMAJ = 0;
         }

         foreach (DrawableGameComponent d in ListeDrawableGameComponent)
         {
             d.Update(gameTime);
         }

         base.Update(gameTime);
      }

      public void AjouterItem(Object o)
      {
         ListeObjet.Add(o);
      }
      public void RetireItem(Object o)
      {
         ListeObjet.Remove(o);
      }
      public void AjouterDrawableGameComponent(DrawableGameComponent d)
      {
          ListeDrawableGameComponent.Add(d);
      }
      public void RetirerDrawableGameComponent(DrawableGameComponent d)
      {
          ListeDrawableGameComponent.Remove(d);
      }

      public override void Draw(GameTime gameTime)
      {
          GraphicsDevice.Viewport = OptionJeu.ViewportDéfaut;

         RectFenetre = new Rectangle((int)Position.X, (int)Position.Y, InfoWidth, (int)((ListeObjet.Count + 1) * HeigthByItem));

         spriteBatch.Draw(ImageBack, RectFenetre, Color.White);
         spriteBatch.DrawString(Fonts, FPS, new Vector2(Position.X, Position.Y + 0 * HeigthByItem), Color.Black);
         for (int i = 0; i < ListeObjet.Count;++i )
         {
            spriteBatch.DrawString(Fonts, ListeObjet[i].ToString(), new Vector2(Position.X, Position.Y + (i+1) * HeigthByItem), Color.Black);
         }

         if(InfoJeu.NBJoueur >1)
            spriteBatch.Draw(DummyTexture, DivisionVerticale, Color.Black);
         if (InfoJeu.NBJoueur > 2)
            spriteBatch.Draw(DummyTexture, DivisionHorizontale, Color.Black);

         foreach (DrawableGameComponent d in ListeDrawableGameComponent)
         {
             d.Draw(gameTime);
         }

            base.Draw(gameTime);
      }

   }
}
