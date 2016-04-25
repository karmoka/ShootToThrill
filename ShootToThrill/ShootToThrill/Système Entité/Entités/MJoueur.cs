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

    public class MJoueur : MAvatar
    {
        float VitesseJoueur { get; set; }

       PlayerIndex IndexJoueur { get; set; }
       IOManager ManagerDeControle { get; set; }

        public MJoueur(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, PlayerIndex indexJoueur)
            : base(game, composanteGraphique, composantePhysique)
        {
            IndexJoueur = indexJoueur;
        }
        public MJoueur(Game game, AnimatedModel ModèleADeBase, AnimatedModel ModèleAnimé, ObjetPhysique composantePhysique, PlayerIndex indexJoueur)
           : base(game, ModèleADeBase, ModèleAnimé, composantePhysique)
        {
           IndexJoueur = indexJoueur;
        }


        public override void Initialize()
        {
            VitesseJoueur = 0.1f; //Arbitraire
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerDeControle = Game.Services.GetService(typeof(IOManager)) as IOManager;

            base.LoadContent();
        }

        protected override void BougerAvatar()
        {
             EstEnMouvement = false;
            if(ManagerDeControle.EstJoystickGaucheActif(IndexJoueur) || true)
                {
                    if (ManagerDeControle.VersGauche(IndexJoueur))
                    {
                        EstEnMouvement = true;
                        Vitesse += new Vector3(-VitesseJoueur, 0, 0);
                    }
                    if (ManagerDeControle.VersDroite(IndexJoueur))
                    {
                        EstEnMouvement = true;
                        Vitesse += new Vector3(VitesseJoueur, 0, 0);
                    }
                    if (ManagerDeControle.VersHaut(IndexJoueur))
                    {
                        EstEnMouvement = true;
                        Vitesse += new Vector3(0, 0, -VitesseJoueur);
                    }
                    if (ManagerDeControle.VersBas(IndexJoueur))
                    {
                        EstEnMouvement = true;
                        Vitesse += new Vector3(0, 0, VitesseJoueur);
                    }
                }

                if (ManagerDeControle.ATiré(IndexJoueur))
                {
                    this.Attaquer();
                }
                if (ManagerDeControle.ARechargé(IndexJoueur))
                {
                    this.Recharger();
                }
                if (ManagerDeControle.AChangéArmeHaut(IndexJoueur))
                {
                    IndexArme = IndexArme + 1;
                }
                if (ManagerDeControle.AChangéArmeBas(IndexJoueur))
                {
                    IndexArme = IndexArme - 1;
                }
                if (ManagerDeControle.ASauté(IndexJoueur) && Math.Round(Position.Y * 100) / 100 == 1)
                {
                    Vitesse += new Vector3(0, 4, 0);
                }
                if (ManagerDeControle.EstJoystickDroitActif(IndexJoueur))
                {
                    Vector2 direction = ManagerDeControle.GetRightThumbStick(IndexJoueur);
                    //ComposanteGraphique.SetRotationY(CustomMathHelper.AngleDeVecteur2D(direction));
                }
        }

        protected override void GérerCollisions()
        {
            //foreach (ObjetPhysique o in ComposantePhysique.ListeObjetCollision)
            //{
            //    if (o is IArme)
            //    {
            //        AjouterArme(o as IArme);
            //    }
            //    //if (o is Munition)
            //    //{
            //    //    AjouterMunition(autre as Munition);
            //    //}
            //    //if (o is Soin)
            //    //{
            //    //    AjouterVie(autre as Soin);
            //    //}
            //}

            base.GérerCollisions();
        }
    }
}
