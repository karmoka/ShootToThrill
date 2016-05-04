using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjetPrincipal.Data;


namespace AtelierXNA
{
   /// <summary>
   /// Menu le plus bas, gère la transistion entre les différents sub menus et autre composantes du menu
   /// </summary>
   public class MenuState : MenuBase
   {
      const float LARGEUR_MENUS = 200;
      const int NB_JOUEURS_MAX = 4;
      const string MESSAGE_APPUYER_START = "Appuyer sur Start";

      Textbox[] TableauJoueurActif { get; set; }
      bool[] JoueursActif { get; set; }
      bool[] JoueursPrêt { get; set; }
      int NombreSubmenuOuvert { get; set; }

      MenuChoixMap MenuMap { get; set; }
      MenuChoixPersonnage MenuPerso { get; set; }
      MenuControles MenuAfficherControles { get; set; }

      DescriptionAvatar j { get; set; }
      Notification nGameStateChanged { get; set; }

      public MenuState(Game game, Vector2 position, InformationGame informationJeu)
         : base(game, position, informationJeu)
      {

      }


      public override void Initialiser()
      {
         base.Initialiser();

         NombreSubmenuOuvert = 0;

         AjouterBouton("Arcade", OnArcadePressed);
         AjouterBouton("Controles", OnControlesPressed);
         AjouterBouton("Quitter", OnQuitterPressed);

         nGameStateChanged = new Notification();
         nGameStateChanged.SetCallBack(GérerGamestatesMenu);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuMultiplayer, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_ChoixPersoMenu, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuChoixMap, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_SubMenuPop, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuControles, ManagerMessage);

         ListeBouton[IndexComposante].ChangerÉtat();

         CréerTableauJoueursActifs();

         MenuMap = new MenuChoixMap(Game, this.Position + new Vector2(LARGEUR_MENUS * 1, 0), InformationJeu);
         MenuPerso = new MenuChoixPersonnage(Game, this.Position + new Vector2(LARGEUR_MENUS * 2, 0), InformationJeu);
         MenuAfficherControles = new MenuControles(Game, Vector2.One * 20, InformationJeu);
      }

      public override void Cleanup()
      {
         for (int i = 0; i < NB_JOUEURS_MAX; ++i)
         {
            Game.Components.Remove(TableauJoueurActif[i]);
         }

         MenuMap.Cleanup();
         MenuPerso.Cleanup();
         MenuAfficherControles.Cleanup();

         base.Cleanup();
      }

      public override void Update(GameTime gameTime)
      {
         GérerConnectionManettes();

         base.Update(gameTime);
      }



      #region GestionManettes
      void GérerConnectionManettes()
      {
         for (int i = 0; i < 4; ++i)
         {
            bool[] AnciensÉtats = new bool[4] { JoueursActif[0], JoueursActif[1], JoueursActif[2], JoueursActif[3] };

            if (JoueursActif[i] && GestionnaireInput.EstNouveauBouton(Buttons.Start, (PlayerIndex)i))
            {
               JoueursPrêt[i] = true;
               TableauJoueurActif[i].ChangerTexte("Joueur " + i + " Prêt");
            }

            if (GamePad.GetState((PlayerIndex)i).IsConnected && GestionnaireInput.EstNouveauBouton(Buttons.Start, (PlayerIndex)i))
            {
               JoueursActif[i] = true;
            }
            else if (!GamePad.GetState((PlayerIndex)i).IsConnected)
            {
               JoueursActif[i] = false;
            }

            if (JoueursActif[i] && !AnciensÉtats[i])
            {
               InformationJeu.AjouterJoueur((PlayerIndex)i);
               TableauJoueurActif[i].Activer();
               TableauJoueurActif[i].ChangerTexte("Joueur " + i + " Prêt? (start)");
            }
            if (!JoueursActif[i] && AnciensÉtats[i])
            {
               InformationJeu.RetirerJoueur((PlayerIndex)i);
               TableauJoueurActif[i].Désactiver();
               TableauJoueurActif[i].ChangerTexte(MESSAGE_APPUYER_START);
            }
         }
      }
      void CréerTableauJoueursActifs()
      {
         JoueursActif = new bool[NB_JOUEURS_MAX] { false, false, false, false };
         JoueursPrêt = new bool[NB_JOUEURS_MAX] { false, false, false, false };
         TableauJoueurActif = new Textbox[NB_JOUEURS_MAX];

         Vector2 HuitièmeÉcran = new Vector2(OptionJeu.WindowWidth, OptionJeu.WindowHeight) / 8;

         for (int i = 0; i < NB_JOUEURS_MAX; ++i)
         {
            TableauJoueurActif[i] = new Textbox(Game, "buttonBack", "ArialDebug", MESSAGE_APPUYER_START, new Vector2(HuitièmeÉcran.X * (2 * i + 1), OptionJeu.WindowHeight - 20), new Vector2(HuitièmeÉcran.X * 2, 30));
            TableauJoueurActif[i].Initialize();
            TableauJoueurActif[i].Désactiver();
            TableauJoueurActif[i].ChangerCouleurs(InformationJeu.CouleursJoueurs[i]);
            Game.Components.Add(TableauJoueurActif[i]);
         }
      }

      public int NbJoueursActifs
      {
         get { return JoueursActif.Count(x => x == true); }
      }

      #endregion

      #region GestionGameStates

      public void OnArcadePressed(object sender, EventArgs eventArgs)
      {
         //InformationJeu.NBJoueur = 1;
         InformationJeu.IDMap = 1;

         ManagerMessage.AjouterÉvénement((int)Message.GameState_MenuChoixMap);
      }
      
      public void OnControlesPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.AjouterÉvénement((int)Message.GameState_MenuControles);
      }

      public void OnQuitterPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.AjouterÉvénement((int)Message.QuitterJeu);
      }

      public void GérerGamestatesMenu(int id)
      {
         switch (id)
         {
            case ((int)Message.GameState_MenuChoixMap):
               NombreSubmenuOuvert++;
               ManagerGamestate.Push(MenuMap);
               break;
            case ((int)Message.GameState_ChoixPersoMenu):
               NombreSubmenuOuvert++;
               ManagerGamestate.Push(MenuPerso);
               break;
            case((int)Message.GameState_MenuControles):
               NombreSubmenuOuvert++;
               ManagerGamestate.Push(MenuAfficherControles);
               break;
            case ((int)Message.GameState_SubMenuPop):
               NombreSubmenuOuvert--;
               ManagerMessage.AjouterÉvénement((int)Message.GameState_Pop);
               break;
         }
      }

      #endregion

   }
}
