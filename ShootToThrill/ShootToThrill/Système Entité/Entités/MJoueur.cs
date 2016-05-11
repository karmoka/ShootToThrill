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
       const float INTERVAL_DOMMAGE = 0.5f;
       public PlayerIndex IndexJoueur { get; private set; }
       IOManager ManagerDeControle { get; set; }
       Vector3 PositionInitiale { get; set; }
       JoueurScreenManager JoueurScreenManager { get; set; }
       public int Score { get; private set; }
       bool EstAttaquer { get; set; }
       float TempsDepuisDommage { get; set; }

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
            Score = 0;
            VitesseAvatarMaximum = 0.1f; //Arbitraire
            EstAttaquer = false;
            TempsDepuisDommage = 0;
            base.Initialize();
            TypeEnt = TypeEntité.Joueur;
           // Fusil fusil = new Pistol(Game, Game.Content.Load<DescriptionFusil>("Description/Pistol"), new Vector3(1, 3, 1) + Vector3.Up, 0.005f, 0.02f);
            //fusil.Initialize();
            //AjouterArme(fusil);
            Fusil fusil = new Machinegun(Game, Game.Content.Load<DescriptionFusil>("Description/Machinegun"), new Vector3(1, 3, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
            TournerSurY(Vector2.Zero);
            
            fusil = new Teslagun(Game, Game.Content.Load<DescriptionFusil>("Description/Teslagun"), new Vector3(1, 4, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
            TournerSurY(Vector2.Zero);

            fusil = new Shotgun(Game, Game.Content.Load<DescriptionFusil>("Description/Shotgun"), new Vector3(0,0,0) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
            TournerSurY(Vector2.Zero);
            
            fusil = new Lasergun(Game, Game.Content.Load<DescriptionFusil>("Description/Lasergun"), new Vector3(1, 4, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            AjouterArme(fusil);
            TournerSurY(Vector2.Zero);

            fusil = new Railgun(Game, Game.Content.Load<DescriptionFusil>("Description/Railgun"), new Vector3(1, 4, 1) + Vector3.Up, 0.005f, 0.02f);
            fusil.Initialize();
            TournerSurY(Vector2.Zero);
            //fusil.ChangerÉtatGraphique();
            AjouterArme(fusil);

            JoueurScreenManager = new JoueurScreenManager(Game, this);
            JoueurScreenManager.Initialize();
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
                    Vitesse += new Vector3(-VitesseAvatarMaximum, 0, 0);
                }
                if (ManagerDeControle.VersDroite(IndexJoueur))
                {
                    Vitesse += new Vector3(VitesseAvatarMaximum, 0, 0);
                }
                if (ManagerDeControle.VersHaut(IndexJoueur))
                {
                    Vitesse += new Vector3(0, 0, -VitesseAvatarMaximum);
                }
                if (ManagerDeControle.VersBas(IndexJoueur))
                {
                    Vitesse += new Vector3(0, 0, VitesseAvatarMaximum);
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
                ChangerArme(true);
            }
            if (ManagerDeControle.AChangéArmeBas(IndexJoueur))
            {
                ChangerArme(false);
            }
            if (ManagerDeControle.ASauté(IndexJoueur) && Math.Round(Position.Y * 100) / 100 == 1)
            {
                Vitesse += new Vector3(0, 4, 0);
            }
            if (ManagerDeControle.EstJoystickDroitActif(IndexJoueur) || ManagerDeControle.EstOrientationActif(IndexJoueur))
            {
               Vector2 direction = ManagerDeControle.EstJoystickDroitActif(IndexJoueur) ? ManagerDeControle.GetRightThumbStick(IndexJoueur) : ManagerDeControle.GetOrientation(IndexJoueur);
               Vector3 direction3 = Vector3.UnitY * CustomMathHelper.AngleDeVecteur2D(direction);
               TournerSurY(direction);
            }
            if (!ManagerDeControle.ATiré(IndexJoueur) && (ArmeSélectionnée.NomArme == "Railgun" || ArmeSélectionnée.NomArme == "Teslagun"))
            {
                ArmeSélectionnée.RechargerContinu();
            }

            base.BougerAvatar();
        }

        protected override void TournerSurY(Vector2 direction)
        {
            ArmeSélectionnée.SetRotation(CustomMathHelper.DéterminerRotationModeleBlender(direction) + new Vector3(MathHelper.PiOver2,0,0)); //new Vector3(CustomMathHelper.AngleDeVecteur2D(direction), MathHelper.Pi, MathHelper.Pi)
            ArmeSélectionnée.ChangerDirection(direction);
            base.TournerSurY(direction);
        }

        protected override void GérerCollisions()
        {
            foreach (IPhysique i in ComposantePhysique.ListeCollision)
            {
               if (i is Item && (i as Item).IdPropriétaire == ID_AUCUN_PROPRIÉTAIRE) //On ne veut pas acceder aux items qu'un autre joueur promene autour de lui
               {
                  GérerItems(i as Item);
               }

               else if (i is IFaitMal)
               {
                  RetirerVie((i as IFaitMal).Domage);
               }
                
            }

            base.GérerCollisions();
        }

       void GérerItems(Item i)
        {
           if (i is Fusil)
           {
              AjouterArme(i as Fusil);
           }
           else if (i is Munition)
           {
              AjouterMunition(i as Munition);
           }
           else if (i is Soin)
           {
              AjouterVie(i as Soin);
           }
           else if (i is Interrupteur)
           {
              (i as Interrupteur).ChangerGravité();
           }
        }

        public void Respawn()
        {
            Vie = VieMax;
            SetPosition(PositionInitiale);
        }

        public override void RetirerVie(int domageReçu)
        {
            if (!EstAttaquer)
            {
                EstAttaquer = true;
                base.RetirerVie(domageReçu);
            }
        }

        protected override void Mourir()
        {
            Fusil fusil = ListeArme.Find(x => x.NomArme == "Pistol");
            ListeArme.Clear();
            AjouterArme(fusil);
            base.Mourir();
        }

        public override void Update(GameTime gameTime)
        {
            JoueurScreenManager.Update(gameTime);
            UpdaterDommageReçu(gameTime);
            base.Update(gameTime);
        }

        void UpdaterDommageReçu(GameTime gameTime)
        {
            if (EstAttaquer)
            {
                TempsDepuisDommage += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsDepuisDommage >= INTERVAL_DOMMAGE)
                {
                    EstAttaquer = false;
                    TempsDepuisDommage = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            JoueurScreenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
