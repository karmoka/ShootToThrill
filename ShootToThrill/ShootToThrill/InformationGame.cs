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
    /// Informations nécessaires pour construire un jeu.
    /// </summary>
   public class InformationGame
   {
       const int NB_JOUEURS = 4;
       const int VALEUR_DÉFAUT = -1;

       //Id de la map sélectionné
       int idMap;

       //Id des avatars pour chaque joueurs
       public int[] idPlayers { get; private set; }
       //Quels joueurs sont actifs pour le jeu
       List<PlayerIndex> JoueursActivés { get; set; }
       public Color[] CouleursJoueurs = new Color[4] { Color.Red, Color.Blue, Color.Green, Color.Yellow };

      public InformationGame(int nbJoueur, int idMap)
      {
         //NBJoueur = nbJoueur;
         IDMap = idMap;
         idPlayers = new int[NB_JOUEURS];
         JoueursActivés = new List<PlayerIndex>();
      }

      public InformationGame()
      {
          //NBJoueur = VALEUR_DÉFAUT;
          IDMap = VALEUR_DÉFAUT;
          idPlayers = new int[NB_JOUEURS];
          JoueursActivés = new List<PlayerIndex>();
      }

      public int NBJoueur
      {
          get 
          {
              return JoueursActivés.Count; 
          }
          //set 
          //{ 
          //   nbJoueur = value;

          //   if(value != VALEUR_DÉFAUT)
          //     idPlayers = new int[value];
          //}
      }
      public int IDMap
          {
          get 
          {
              if (idMap == VALEUR_DÉFAUT) // une map doit etre sélectionné
                  throw new ArgumentNullException();

              return idMap;
          }
          set 
          { 
             idMap = value;
          }
      }

       /// <summary>
       /// enregistre l'id de l'avatar sélectionner par un joueur
       /// </summary>
       /// <param name="idPlayer"></param>
       /// <param name="idAvatar"></param>
       public void SetPlayerAvatar(int idPlayer, int idAvatar)
      {
          if (idPlayers.Length < idPlayer)
              throw new IndexOutOfRangeException();

          idPlayers[idPlayer] = idAvatar;
      }

       /// <summary>
       /// Ajouter un joueur actif a la partie
       /// </summary>
       /// <param name="p"></param>
      public void AjouterJoueur(PlayerIndex p)
       {
         if(JoueursActivés.FindIndex(x=> x==p) < 0)
         {
            JoueursActivés.Add(p);
         }
       }

       /// <summary>
       /// retirer un joueur de la liste de joueurs actifs
       /// </summary>
       /// <param name="p"></param>
      public void RetirerJoueur(PlayerIndex p)
      {
         JoueursActivés.RemoveAll(x => x == p);
      }

      
   }
}
