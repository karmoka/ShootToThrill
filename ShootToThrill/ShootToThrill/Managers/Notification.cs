using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    public class Notification
    {
       //Méthode appelé lorsque l'id est détecter dans le systeme, agit en fonction de cette id
        public Action<int> Callback { get; set; }

        public void SetCallBack(Action<int> fonction)
        {
            Callback = fonction;
        }

       /// <summary>
       /// Ajoute la notification au systeme
       /// </summary>
       /// <param name="IDMessage"></param>
       /// <param name="eventManager"></param>
        public void AjouterAuSystem(int IDMessage, MessageManager eventManager)
        {
            eventManager.InscrireNotification(IDMessage, this);
        }
        public void AjouterAuSystem(Message IDMessage, MessageManager eventManager)
        {
           eventManager.InscrireNotification((int)IDMessage, this);
        }
    }
}
