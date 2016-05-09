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
   public class MenuBase : DrawableGameComponent, GameState
   {
      protected Options OptionJeu { get; set; }


      protected RessourcesManager<SpriteFont> GestionnaireFonts { get; set; }
      protected RessourcesManager<Texture2D> GestionnaireTextutes { get; set; }
      protected GameStateManager ManagerGamestate { get; set; }
      protected InformationGame InformationJeu { get; set; }
      protected MessageManager ManagerMessage { get; set; }
      protected IOManager GestionnaireInput { get; set; }
      protected ManagerAudio ManagerDeSons { get; set; }
      protected SpriteBatch spritebatch { get; set; }


      string NomFonts { get; set; }
      string NomImageBouton { get; set; }

      protected Vector2 Position { get; set; }
      protected Vector2 DimensionBouton { get; set; }
      protected List<Bouton> ListeBouton { get; set; }
      protected int IndexComposante { get; set; }
      //InputManager Input { get; set; }

      public bool EstActivé { get; set; }
      public bool EstDétruit { get; set; }

      float IntervalMaj { get; set; }
      float TempsDepuisMAJ { get; set; }


      public MenuBase(Game game, Vector2 position, InformationGame informationJeu)
         : base(game)
      {
         InformationJeu = informationJeu;

         Position = position;
         IndexComposante = 0;
         ListeBouton = new List<Bouton>();

         NomFonts = "ArialDebug";
         NomImageBouton = "buttonBack";
         DimensionBouton = new Vector2(200, 30);
      }
      public MenuBase(MenuBase m)
         : base(m.Game)
      {
         InformationJeu = m.InformationJeu;

         Position = m.Position;
         IndexComposante = 0;
         ListeBouton = new List<Bouton>();

         NomFonts = m.NomFonts;
         NomImageBouton = m.NomImageBouton;
         DimensionBouton = m.DimensionBouton;
      }

      void CréerBoutons(String[] textes, EventHandler[] fonctions)
      {
         for (int i = 0; i < textes.Length; ++i)
         {
            for (int j = 0; j < fonctions.Length; ++j)
            {
               AjouterBouton(textes[i], fonctions[j]);
            }
         }
      }

      public virtual void Cleanup()
      {
         ListeBouton.Clear();
      }
      public virtual void Initialiser()
      {
         OptionJeu = Game.Services.GetService(typeof(Options)) as Options;

         IntervalMaj = OptionJeu.IntervalMAJStandard;
         TempsDepuisMAJ = 0;

         GestionnaireInput = Game.Services.GetService(typeof(IOManager)) as IOManager;
         spritebatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         ManagerDeSons = Game.Services.GetService(typeof(ManagerAudio)) as ManagerAudio;
         ManagerMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
         ManagerGamestate = Game.Services.GetService(typeof(GameStateManager)) as GameStateManager;
         GestionnaireTextutes = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

         EstActivé = true;
         EstDétruit = false;

         base.Initialize();
      }
      public virtual void Pause()
      {
         EstActivé = false;

         foreach (Bouton b in ListeBouton)
         {
            b.Désactiver();
         }
      }
      public virtual void Résumer()
      {
         EstActivé = true;

         foreach (Bouton b in ListeBouton)
         {
            b.Activer();
         }
      }

      public void AjouterBouton(string texte, EventHandler deleguer)
      {
         Bouton b = new Bouton(Game, NomImageBouton, NomFonts, texte, Position + new Vector2(0, ListeBouton.Count * 30), DimensionBouton);
         b.Pressed += deleguer;
         b.Initialize();
         ListeBouton.Add(b);
      }

      public override void Update(GameTime gameTime)
      {
         TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;

         if (TempsDepuisMAJ >= IntervalMaj)
         {
            GérerBoutons();

            TempsDepuisMAJ = 0;
         }
      }

      void GérerBoutons()
      {
          if (EstActivé && ListeBouton.Count != 0)
          {
              if (InformationJeu.NBJoueur > 0)
              {
                  if (GestionnaireInput.EstMenuHaut(PlayerIndex.One))
                  {
                      if (IndexComposante > 0)
                      {
                          ListeBouton[IndexComposante].ChangerÉtat();
                          IndexComposante--;
                          ListeBouton[IndexComposante].ChangerÉtat();
                          ManagerDeSons.JouerSons("Click");
                      }
                  }
                  if (GestionnaireInput.EstMenuBas(PlayerIndex.One))
                  {
                      if (IndexComposante < ListeBouton.Count - 1)
                      {
                          ListeBouton[IndexComposante].ChangerÉtat();
                          IndexComposante++;
                          ListeBouton[IndexComposante].ChangerÉtat();
                          ManagerDeSons.JouerSons("Click");
                      }
                  }
                  if (GestionnaireInput.EstMenuSélectionner(PlayerIndex.One))
                  {
                      ListeBouton[IndexComposante].OnPressed();
                      ManagerDeSons.JouerSons("Button");
                  }
              }
          }
      }

      public virtual void Draw(GameTime gameTime, float ordre)
      {
         foreach (Bouton b in ListeBouton)
         {
            b.Draw(gameTime);
         }
      }
   }
}
