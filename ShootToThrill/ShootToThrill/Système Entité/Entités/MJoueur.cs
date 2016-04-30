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
using ProjetPrincipal.Data;

namespace AtelierXNA
{

    class MJoueur : MAvatar
    {
       float VitesseJoueur { get; set; }

       PlayerIndex IndexJoueur { get; set; }
       IOManager ManagerDeControle { get; set; }

        public MJoueur(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, PlayerIndex indexJoueur)
            : base(game, composanteGraphique, composantePhysique)
        {
            IndexJoueur = indexJoueur;
        }
        public MJoueur(Game game, string ModèleDeBase, string ModèleAnimé, ObjetPhysique composantePhysique, PlayerIndex indexJoueur)
           : base(game, ModèleDeBase, ModèleAnimé, composantePhysique)
        {
           IndexJoueur = indexJoueur;
        }


        public override void Initialize()
        {
            VitesseJoueur = 0.1f; //Arbitraire
            base.Initialize();
            TypeEnt = TypeEntité.Joueur;
            Fusil fusil = new Pistol(Game, Game.Content.Load<DescriptionFusil>("Description/Pistol"), new Vector3(1, 3, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
        }

        protected override void LoadContent()
        {
            ManagerDeControle = Game.Services.GetService(typeof(IOManager)) as IOManager;

            base.LoadContent();
        }

        protected override void BougerAvatar()
        {
            EstEnMouvement = false;
            if(ManagerDeControle.EstJoystickGaucheActif(IndexJoueur) || ManagerDeControle.EstDéplscementActif(IndexJoueur))
            {
                if (ManagerDeControle.VersGauche(IndexJoueur))
                {
                    Vitesse += new Vector3(-VitesseJoueur, 0, 0);
                }
                if (ManagerDeControle.VersDroite(IndexJoueur))
                {
                    Vitesse += new Vector3(VitesseJoueur, 0, 0);
                }
                if (ManagerDeControle.VersHaut(IndexJoueur))
                {
                    Vitesse += new Vector3(0, 0, -VitesseJoueur);
                }
                if (ManagerDeControle.VersBas(IndexJoueur))
                {
                    Vitesse += new Vector3(0, 0, VitesseJoueur);
                }
                EstEnMouvement = true;
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
            if (ManagerDeControle.EstJoystickDroitActif(IndexJoueur) || ManagerDeControle.EstOrientationActif(IndexJoueur))
                {
                    Vector2 direction = ManagerDeControle.EstJoystickDroitActif(IndexJoueur) ? ManagerDeControle.GetRightThumbStick(IndexJoueur) : ManagerDeControle.GetOrientation(IndexJoueur);
                    Vector3 direction3 = new Vector3(direction.X, 0, direction.Y);
                //ComposanteGraphique.SetRotation(CustomMathHelper.AngleDeVecteur2D(direction));
            }
            base.BougerAvatar();
        }

        protected override void GérerCollisions()
        {
            //foreach (ObjetPhysique o in ComposantePhysique.ListeObjetCollision)
            //{
            //    Vitesse = new Vector3(Vitesse.X, Vitesse.Y * MasseInverse, Vitesse.Z); //TODO CHANGER CA
            //    if (o is Fusil)
            //    {
            //        AjouterArme(o as Fusil);
            //    }
            //    else if (o is Munition)
            //    {
            //        AjouterMunition(o as Munition);
            //    }
            //    else if (o is Soin)
            //    {
            //        AjouterVie(o as Soin);
            //    }
            //    else if (o is Ennemi)
            //    {
            //        RetirerVie((o as Ennemi).Domage);
            //    }
            //    else if (o is Interrupteur)
            //    {
            //        (o as Interrupteur).ChangerGravité();
            //    }
            //}

            base.GérerCollisions();
        }
    }
}
