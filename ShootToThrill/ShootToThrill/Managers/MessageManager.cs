using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    /// <summary>
    /// Décrit tout les messages qui peuvent être envoyé à travers le programme
    /// </summary>
    public enum Message { AvatarFusil, ActivationSupportFusil, DésactivationSupportFusil, QuitterJeu, GameState_GamePlay, GameState_Menu, GameState_Splash, GameState_TransistionGamePlay, GameState_Pop,
    GameState_MenuMultiplayer, GameState_MenuChoixMap, GameState_Option, GameState_TransitionMenu, GameState_ChoixPersoMenu, GameState_SubMenuPop
    }


    /// <summary>
    /// Paire d'une Notification et du id d'un message
    /// </summary>
    public struct NotificationPair
    {
        public int ID { get; set; }
        public Notification notification { get; set; }
    }

    /// <summary>
    /// Store une liste d'id de message ainsi qu'une liste de notification souscrit a un certain id.
    /// Appel la notification lorsque l'id apparait
    /// </summary>
    public class MessageManager : GameComponent
    {
        List<NotificationPair> ListePaireNotification { get; set; }
        List<int> ListeÉvénement { get; set; }

        public MessageManager(Game game)
            : base(game)
        {
            ListePaireNotification = new List<NotificationPair>();
            ListeÉvénement = new List<int>();
        }

       /// <summary>
       /// Ajoute une notification a la liste en tant que paire id/notification
       /// </summary>
       /// <param name="id"></param>
       /// <param name="notification"></param>
        public void InscrireNotification(int id, Notification notification)
        {
            NotificationPair not = new NotificationPair();
            not.ID = id;
            not.notification = notification;
            ListePaireNotification.Add(not);
        }

       /// <summary>
       /// Désinsctit la notification du systeme
       /// </summary>
       /// <param name="iD"></param>
       /// <param name="notification"></param>
        public void DésinscrireNotification(int iD, Notification notification)
        {
            ListePaireNotification.RemoveAll(x => x.ID == iD && x.notification == notification);
        }

       /// <summary>
       /// Ajoute un évenement/message au systeme pour que les classes concernés soit informer.
       /// </summary>
       /// <param name="id"></param>
        public void AjouterÉvénement(int id)
        {
            ListeÉvénement.Add(id);
        }

        public override void Update(GameTime gameTime)
        {
           for (int i = 0; i < ListeÉvénement.Count; ++i )
           {
              for (int e = 0; e < ListePaireNotification.Count; ++e)
              {
                 if (ListePaireNotification[e].ID == ListeÉvénement[i])
                 {
                    ListePaireNotification[e].notification.Callback(ListeÉvénement[i]);
                 }
              }
           }

           //La liste des évènements est vidée apres chaque boucle.
           //Amélioration possible, ajouter un temps aux évènements
            ListeÉvénement.Clear();

            base.Update(gameTime);
        }

    }
}
