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
    /// Lit les inputs de chaque utilisateurs et créer un niveau d'abstraction pour certaine action
    /// </summary>
   public class IOManager : DrawableGameComponent
   {
       Options OptionsJeu { get; set; }

      const int NB_JOUEUR = 4;

      float IntervalMAJ { get; set; }
      float TempsDepuisMAJ { get; set; }

      DefinitionGamePad Contrôles { get; set; }
      //PlayerIndex IndexJoueur { get; set; }



      GamePadState[] NouveauStatePad { get; set; }
      GamePadState[] AncienStateGamepad { get; set; }
      KeyboardState[] NouveauKeyboardState { get; set; }
      KeyboardState[] AncienKeyboardState { get; set; }

      bool[] ÉtaitActivé { get; set; }

      public IOManager(Game game, float intervalMAJ)
         : base(game)
      {
         ÉtaitActivé = new bool[NB_JOUEUR];

         NouveauKeyboardState = new KeyboardState[NB_JOUEUR];
         AncienKeyboardState = new KeyboardState[NB_JOUEUR];

         NouveauStatePad = new GamePadState[NB_JOUEUR];
         AncienStateGamepad = new GamePadState[NB_JOUEUR];

         IntervalMAJ = intervalMAJ;
         //Contrôles = DefinitionGamePad.DistribuerCommandes(IndexJoueur);
      }

      public override void Initialize()
      {
         OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
         TempsDepuisMAJ = 0;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
          TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
          
          if(TempsDepuisMAJ > OptionsJeu.IntervalMAJStandard)
          {
              for (int i = 0; i < NB_JOUEUR; ++i)
              {
                  AncienKeyboardState[i] = NouveauKeyboardState[i];
                  AncienStateGamepad[i] = NouveauStatePad[i];

                  NouveauKeyboardState[i] = Keyboard.GetState((PlayerIndex)i);
                  NouveauStatePad[i] = GamePad.GetState((PlayerIndex)i);
              }

              TempsDepuisMAJ = 0;
          }

         base.Update(gameTime);
      }

      public bool EstNouveauBouton(Buttons b, PlayerIndex playerIndex)
      {
         return NouveauStatePad[(int)playerIndex].IsButtonDown(b) && AncienStateGamepad[(int)playerIndex].IsButtonUp(b);
      }
      public bool EstNouvelleTouche(Keys k, PlayerIndex playerIndex)
      {
         return NouveauKeyboardState[(int)playerIndex].IsKeyDown(k) && AncienKeyboardState[(int)playerIndex].IsKeyUp(k);
      }
      public bool EstBoutonEnfoncé(Buttons b, PlayerIndex playerIndex)
      {
          return NouveauStatePad[(int)playerIndex].IsButtonDown(b);
      } 
      public bool EstToucheEnfoncée(Keys k, PlayerIndex playerIndex)
      {
          return NouveauKeyboardState[(int)playerIndex].IsKeyDown(k);
      }

      public bool EstActif(PlayerIndex playerIndex)
      {
          return AnyButtonPress(playerIndex) || NouveauKeyboardState[(int)playerIndex].GetPressedKeys().Length != 0;
      }
      public bool EstJoystickDroitActif(PlayerIndex playerIndex)
      {
          return GetRightThumbStick(playerIndex) != Vector2.Zero;
      }
      public bool EstJoystickGaucheActif(PlayerIndex playerIndex)
      {
          return GetLeftThumbStick(playerIndex) != Vector2.Zero;
      }
      public bool EstGachetteDroiteActive(PlayerIndex playerIndex)
      {
          return NouveauStatePad[(int)playerIndex].Triggers.Right != 0;
      }
      public bool EstGachetteGaucheActive(PlayerIndex playerIndex)
      {
          return NouveauStatePad[(int)playerIndex].Triggers.Left != 0;
      }
      public bool EstDéplscementActif(PlayerIndex playerIndex)
      {
          return GetDéplacement(playerIndex) != Vector2.Zero;
      }
      public bool EstOrientationActif(PlayerIndex playerIndex)
      {
          return GetOrientation(playerIndex) != Vector2.Zero;
      }

      public bool AnyButtonPress(PlayerIndex playerIndex)
      {
          bool lol = false;
          if (EstBoutonEnfoncé(Buttons.A, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.B, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.Back, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.BigButton, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.LeftShoulder, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.RightShoulder, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.LeftStick, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.RightStick, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.Start, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.X, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.Y, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.DPadLeft, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.DPadDown, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.DPadRight, playerIndex))
          {
              lol = true;
          }
          else if (EstBoutonEnfoncé(Buttons.DPadUp, playerIndex))
          {
              lol = true;
          }
          else if (EstJoystickDroitActif(playerIndex))
          {
              lol = true;
          }
          else if (EstJoystickGaucheActif(playerIndex))
          {
              lol = true;
          }
          else if (EstGachetteDroiteActive(playerIndex))
          {
              lol = true;
          }
          else if (EstGachetteGaucheActive(playerIndex))
          {
              lol = true;
          }
          return lol;
      }
      


      public bool EstMenuBas(PlayerIndex playerIndex)
      {
         return EstNouvelleTouche(Keys.Down, playerIndex) || EstNouveauBouton(Buttons.DPadDown, playerIndex) || EstNouveauBouton(Buttons.LeftThumbstickDown, playerIndex);
      }
      public bool EstMenuHaut(PlayerIndex playerIndex)
      {
         return EstNouvelleTouche(Keys.Up, playerIndex) || EstNouveauBouton(Buttons.DPadUp, playerIndex) || EstNouveauBouton(Buttons.LeftThumbstickUp, playerIndex);
      }
      public bool EstMenuSélectionner(PlayerIndex playerIndex)
      {
         return EstNouvelleTouche(Keys.Enter, playerIndex) || EstNouveauBouton(Buttons.A, playerIndex);
      }

       public bool EstOptionSélectionner(PlayerIndex playerIndex)
      {
          return EstNouvelleTouche(Keys.Escape, playerIndex) || EstNouveauBouton(Buttons.Start, playerIndex);
      }

      public Vector2 GetLeftThumbStick(PlayerIndex playerIndex)
      {
         return NouveauStatePad[(int)playerIndex].ThumbSticks.Left;
      }
      public Vector2 GetRightThumbStick(PlayerIndex playerIndex)
      {
         return NouveauStatePad[(int)playerIndex].ThumbSticks.Right;
      }
      public Vector2 GetDéplacement(PlayerIndex playerIndex)
      {
          float x = VersDroite(playerIndex) ? 1 : VersGauche(playerIndex) ? -1 : 0;
          float y = VersHaut(playerIndex) ? 1 : VersBas(playerIndex) ? -1 : 0;
          return new Vector2(x, y);
      }
      public Vector2 GetOrientation(PlayerIndex playerIndex)
      {
          float x = EstToucheEnfoncée(Keys.L, playerIndex) ? 1 : EstToucheEnfoncée(Keys.J, playerIndex) ? -1 : 0;
          float y = EstToucheEnfoncée(Keys.I, playerIndex) ? 1 : EstToucheEnfoncée(Keys.K, playerIndex) ? -1 : 0;
          return new Vector2(x, y);
      }

      public bool VersDroite(PlayerIndex playerIndex)
      {
          return EstBoutonEnfoncé(Buttons.LeftThumbstickRight, playerIndex) || EstToucheEnfoncée(Keys.Right, playerIndex) || EstToucheEnfoncée(Keys.D, playerIndex);
      }
      public bool VersGauche(PlayerIndex playerIndex)
      {
          return EstBoutonEnfoncé(Buttons.LeftThumbstickLeft, playerIndex) || EstToucheEnfoncée(Keys.Left, playerIndex) || EstToucheEnfoncée(Keys.A, playerIndex);
      }
      public bool VersHaut(PlayerIndex playerIndex)
      {
          return EstBoutonEnfoncé(Buttons.LeftThumbstickUp, playerIndex) || EstToucheEnfoncée(Keys.Up, playerIndex) || EstToucheEnfoncée(Keys.W, playerIndex);
      }
      public bool VersBas(PlayerIndex playerIndex)
      {
          return EstBoutonEnfoncé(Buttons.LeftThumbstickDown, playerIndex) || EstToucheEnfoncée(Keys.Down, playerIndex) || EstToucheEnfoncée(Keys.S, playerIndex);
      }
      public bool ASauté(PlayerIndex playerIndex)
      {
          return EstNouveauBouton(Buttons.A, playerIndex) || EstNouvelleTouche(Keys.Space, playerIndex);
      }
      public bool ATiré(PlayerIndex playerIndex)
      {
          return EstBoutonEnfoncé(Buttons.RightTrigger, playerIndex) || EstToucheEnfoncée(Keys.NumPad1, playerIndex);
      }
      public bool AChangéArme(PlayerIndex playerIndex)
      {
          return EstNouveauBouton(Buttons.Y, playerIndex) || EstNouvelleTouche(Keys.NumPad8, playerIndex);
      }
      public bool ARechargé(PlayerIndex playerIndex)
      {
          return EstNouveauBouton(Buttons.X, playerIndex) || EstNouvelleTouche(Keys.NumPad4, playerIndex);
      }
      public bool AChangéArmeHaut(PlayerIndex playerIndex)
      {
          return EstNouveauBouton(Buttons.RightShoulder, playerIndex) || EstNouvelleTouche(Keys.E, playerIndex);
      }
      public bool AChangéArmeBas(PlayerIndex playerIndex)
      {
          return EstNouveauBouton(Buttons.LeftShoulder, playerIndex) || EstNouvelleTouche(Keys.Q, playerIndex);
      }
   }
}
