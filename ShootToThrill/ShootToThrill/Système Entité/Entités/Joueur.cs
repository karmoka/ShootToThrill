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
using ProjetPrincipal.Data;

namespace AtelierXNA
{
    class Joueur : Avatar
    {
        PlayerIndex IndexJoueur { get; set; }
        IOManager ManagerDeControle { get; set; }
        public Joueur(Game game, Vector3 position, Vector3 vitesse, float masseInverse, float rayon, string nomModèle, int vie, PlayerIndex playerIndex)
            : base(game, position, vitesse, masseInverse, rayon, nomModèle, vie)
        {
            IndexJoueur = playerIndex;

        }
        //public Joueur(Game game, DescriptionJoueur description, PlayerIndex playerIndex)
        //    : base(game, description)
        //{
        //    IndexJoueur = playerIndex;
        //}

        public override void Initialize()
        {
            VitesseJoueur = 0.1f;
            base.Initialize();
            
            Fusil fusil = new Pistol(Game, Game.Content.Load<DescriptionFusil>("Description/Pistol"), new Vector3(1, 3, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
        }

        protected override void LoadContent()
        {
            ManagerDeControle = Game.Services.GetService(typeof(IOManager)) as IOManager;
            base.LoadContent();            
        }

        public override void Update(GameTime gameTime)
        {
            ManagerDeControle.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void BougerAvatar()
        {
            if (ManagerDeControle.EstActif(IndexJoueur))
            {
                if (ManagerDeControle.EstJoystickGaucheActif(IndexJoueur)|| ManagerDeControle.EstDéplscementActif(IndexJoueur))
                {
                    if (ManagerDeControle.VersGauche(IndexJoueur))
                    {
                        Vitesse += new Vector3(-VitesseJoueur, 0, 0);
                        //ArmeSélectionnée.ChangerVitesse(new Vector3(-VitesseJoueur, 0, 0));
                    }
                    if (ManagerDeControle.VersDroite(IndexJoueur))
                    {
                        Vitesse += new Vector3(VitesseJoueur, 0, 0);
                        //ArmeSélectionnée.ChangerVitesse(new Vector3(VitesseJoueur, 0, 0));
                    }
                    if (ManagerDeControle.VersHaut(IndexJoueur))
                    {
                        Vitesse += new Vector3(0, 0, -VitesseJoueur);
                        //ArmeSélectionnée.ChangerVitesse(new Vector3(0, 0, -VitesseJoueur));
                    }
                    if (ManagerDeControle.VersBas(IndexJoueur))
                    {
                        Vitesse += new Vector3(0, 0, VitesseJoueur);
                        //ArmeSélectionnée.ChangerVitesse(new Vector3(0, 0, VitesseJoueur));
                    }
                }

                #region Fusil
                if (AUnFusil)
                {
                    if (ManagerDeControle.ATiré(IndexJoueur))
                    {
                        Tirer();
                    }
                    if (ManagerDeControle.ARechargé(IndexJoueur))
                    {
                        Recharger();
                    }
                    if (ManagerDeControle.AChangéArmeHaut(IndexJoueur))
                    {
                        ChangerArme(true);
                    }
                    if (ManagerDeControle.AChangéArmeBas(IndexJoueur))
                    {
                        ChangerArme(false);
                    }
                    if (ManagerDeControle.EstNouveauBouton(Buttons.B, IndexJoueur))
                        ArmeSélectionnée.Recharger();

                    Game.Window.Title = ArmeSélectionnée.MunitionRestantDansChargeur.ToString();

                }
                #endregion

                if (ManagerDeControle.ASauté(IndexJoueur) && Math.Round(Position.Y * 100) / 100 == 1)
                {
                    Vitesse += new Vector3(0, 4, 0);
                    ArmeSélectionnée.ChangerVitesse(new Vector3(0, 4, 0));
                }
                if (ManagerDeControle.EstJoystickDroitActif(IndexJoueur) || ManagerDeControle.EstOrientationActif(IndexJoueur))
                {
                    Vector2 direction = ManagerDeControle.EstJoystickDroitActif(IndexJoueur) ? ManagerDeControle.GetRightThumbStick(IndexJoueur) : ManagerDeControle.GetOrientation(IndexJoueur);
                    ModifierDirection(direction);
                }
            }
            base.BougerAvatar();
        }

        protected override void ModifierDirection(Vector2 direction)
        {
            ArmeSélectionnée.RotationSurY(CustomMathHelper.AngleDeVecteur2D(direction));
            ArmeSélectionnée.ChangerDirection(direction);
            base.ModifierDirection(direction);
        }

        public override void EnCollision(ObjetPhysique autre, InformationIntersection infoColli)
        {
            Vitesse = new Vector3(Vitesse.X, Vitesse.Y * MasseInverse, Vitesse.Z); //TODO CHANGER CA
            if (autre is Fusil)
            {
                AjouterArme(autre as Fusil);
            }
            else if (autre is Munition)
            {
                AjouterMunition(autre as Munition);
            }
            else if (autre is Soin)
            {
                AjouterVie(autre as Soin);
            }
            else if (autre is Ennemi)
            {
                RetirerVie((autre as Ennemi).Domage);
            }
            else if (autre is Interrupteur)
            {
                (autre as Interrupteur).ChangerGravité();
            }
        }

        public override string ToString()
        {
            return " x: " + (float)(Math.Round(Position.X * 100) / 100) + "      y: " + (float)(Math.Round(Position.Y * 100) / 100) + "      z: " + (float)(Math.Round(Position.Z * 100) / 100);
        }
    }
}