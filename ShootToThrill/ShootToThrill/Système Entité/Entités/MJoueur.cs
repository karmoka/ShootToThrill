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
       public PlayerIndex IndexJoueur { get; private set; }
       IOManager ManagerDeControle { get; set; }
       Vector3 PositionInitiale { get; set; }

        public MJoueur(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, PlayerIndex indexJoueur)
            : base(game, composanteGraphique, composantePhysique)
        {
            IndexJoueur = indexJoueur;
            PositionInitiale = composantePhysique.Position;
        }
        public MJoueur(Game game, DescriptionAvatar description, Vector3 position, PlayerIndex indexJoueur)
            : base(game, description, position)
        {
            IndexJoueur = indexJoueur;
            PositionInitiale = position;
        }

        public override void Initialize()
        {
            VitesseJoueur = 0.1f; //Arbitraire
            base.Initialize();
            TypeEnt = TypeEntité.Joueur;
            Fusil fusil = new Pistol(Game, Game.Content.Load<DescriptionFusil>("Description/Pistol"), new Vector3(1, 3, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerDeControle = Game.Services.GetService(typeof(IOManager)) as IOManager;

            base.LoadContent();
        }

        protected override void BougerAvatar()
        {
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
                    Vector3 direction3 = Vector3.UnitY * CustomMathHelper.AngleDeVecteur2D(direction);
                    ModifierDirection(direction);
                //ComposanteGraphique.SetRotation(CustomMathHelper.AngleDeVecteur2D(direction));
            }
            base.BougerAvatar();
        }

        protected override void ModifierDirection(Vector2 direction)
        {
            ArmeSélectionnée.SetRotation(Vector3.UnitY * CustomMathHelper.AngleDeVecteur2D(direction));
            ArmeSélectionnée.ChangerDirection(direction);
            base.ModifierDirection(direction);
        }

        protected override void GérerCollisions()
        {
            foreach (InformationIntersection i in ComposantePhysique.ListeCollision)
            {
                //Vitesse = new Vector3(Vitesse.X, Vitesse.Y * MasseInverse, Vitesse.Z); //TODO CHANGER CA
                if (i.ObjetB is Fusil)
                {
                    AjouterArme(i.ObjetB as Fusil);
                }
                else if (i.ObjetB is Munition)
                {
                    AjouterMunition(i.ObjetB as Munition);
                }
                else if (i.ObjetB is Soin)
                {
                    AjouterVie(i.ObjetB as Soin);
                }
                else if (i.ObjetB is MEnnemi)
                {
                    RetirerVie((i.ObjetB as MEnnemi).Domage);
                }
                else if (i.ObjetB is Interrupteur)
                {
                    (i.ObjetB as Interrupteur).ChangerGravité();
                }
            }

            base.GérerCollisions();
        }

        public void Respawn()
        {
            Vie = VieMax;
            SetPosition(PositionInitiale);
        }
    }
}
