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
   /// Menu le plus bas, g�re la transistion entre les diff�rents sub menus et autre composantes du menu
   /// </summary>
   public class MenuState : MenuBase
   {
      const float LARGEUR_MENUS = 200;
      const int NB_JOUEURS_MAX = 4;
      const string MESSAGE_APPUYER_START = "Appuyer sur Start";

      Textbox[] TableauJoueurActif { get; set; }
      bool[] JoueursActif { get; set; }
      bool[] JoueursPr�t { get; set; }
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
         nGameStateChanged.SetCallBack(G�rerGamestatesMenu);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuMultiplayer, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_ChoixPersoMenu, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuChoixMap, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_SubMenuPop, ManagerMessage);
         nGameStateChanged.AjouterAuSystem((int)Message.GameState_MenuControles, ManagerMessage);

         ListeBouton[IndexComposante].Changer�tat();

         Cr�erTableauJoueursActifs();

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
         G�rerConnectionManettes();

         base.Update(gameTime);
      }



      #region GestionManettes
      void G�rerConnectionManettes()
      {
         for (int i = 0; i < 4; ++i)
         {
            bool[] Anciens�tats = new bool[4] { JoueursActif[0], JoueursActif[1], JoueursActif[2], JoueursActif[3] };

            if (JoueursActif[i] && GestionnaireInput.EstNouveauBouton(Buttons.Start, (PlayerIndex)i))
            {
               JoueursPr�t[i] = true;
               TableauJoueurActif[i].ChangerTexte("Joueur " + i + " Pr�t");
            }

            if (GamePad.GetState((PlayerIndex)i).IsConnected && GestionnaireInput.EstNouveauBouton(Buttons.Start, (PlayerIndex)i))
            {
               JoueursActif[i] = true;
            }
            else if (!GamePad.GetState((PlayerIndex)i).IsConnected)
            {
               JoueursActif[i] = false;
            }

            if (JoueursActif[i] && !Anciens�tats[i])
            {
               InformationJeu.AjouterJoueur((PlayerIndex)i);
               TableauJoueurActif[i].Activer();
               TableauJoueurActif[i].ChangerTexte("Joueur " + i + " Pr�t? (start)");
            }
            if (!JoueursActif[i] && Anciens�tats[i])
            {
               InformationJeu.RetirerJoueur((PlayerIndex)i);
               TableauJoueurActif[i].D�sactiver();
               TableauJoueurActif[i].ChangerTexte(MESSAGE_APPUYER_START);
            }
         }
      }
      void Cr�erTableauJoueursActifs()
      {
         JoueursActif = new bool[NB_JOUEURS_MAX] { false, false, false, false };
         JoueursPr�t = new bool[NB_JOUEURS_MAX] { false, false, false, false };
         TableauJoueurActif = new Textbox[NB_JOUEURS_MAX];

         Vector2 Huiti�me�cran = new Vector2(OptionJeu.WindowWidth, OptionJeu.WindowHeight) / 8;

         for (int i = 0; i < NB_JOUEURS_MAX; ++i)
         {
            TableauJoueurActif[i] = new Textbox(Game, "buttonBack", "ArialDebug", MESSAGE_APPUYER_START, new Vector2(Huiti�me�cran.X * (2 * i + 1), OptionJeu.WindowHeight - 20), new Vector2(Huiti�me�cran.X * 2, 30));
            TableauJoueurActif[i].Initialize();
            TableauJoueurActif[i].D�sactiver();
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

         ManagerMessage.Ajouter�v�nement((int)Message.GameState_MenuChoixMap);
      }
      
      public void OnControlesPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.Ajouter�v�nement((int)Message.GameState_MenuControles);
      }

      public void OnQuitterPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.Ajouter�v�nement((int)Message.QuitterJeu);
      }

      public void G�rerGamestatesMenu(int id)
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
               ManagerMessage.Ajouter�v�nement((int)Message.GameState_Pop);
               break;
         }
      }

      #endregion

   }
}
