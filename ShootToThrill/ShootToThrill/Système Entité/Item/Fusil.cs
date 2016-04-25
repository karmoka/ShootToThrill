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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Fusil : Item, IModele3d
    {
        const int ZERO = 0,
                  MAX_DEGRÉS = 360;
        const float DISTANCE_JOUEUR_FUSIL = 0.5f;
        int _angle;
        public Vector2 Direction { get; private set; }

        GameTime GameTime { get; set; }
        MessageManager ManagerDeMessage { get; set; }

        public ModelManager ManagerDeModèle { get; set; }

        List<DroiteColorée> ListeTrajectoires { get; set; }
        public bool TrajectoireExiste { get; set; }

        int AngleRotation
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = SetAngleRotation(value);
            }
        }
        public float TempsDepuisDernierTir { get; set; }
        float TempsDepuisDebutJeu { get; set; }
        int NbBallesParTir { get; set; }


        #region Munition
        int _munitionTotalRestant,
            _munitionRestantDansChargeur;

        const int MUNITION_MIN = 0,
                  UNE_MUNITION = 1;
        public bool AMunitionInfini { get; private set; }
        int MunitionTotalRestant
        {
            get
            {
                return _munitionTotalRestant;
            }
            set
            {
                _munitionTotalRestant = SetMunitionTotalRestant(value);
            }
        }
        public int MunitionTotalMax { get; private set; }
        public int MunitionMaxDansChargeur { get; private set; }
        public int MunitionRestantDansChargeur
        {
            get
            {
                return _munitionRestantDansChargeur;
            }
            private set
            {
                _munitionRestantDansChargeur = SetMunitionRestantDansChargeur(value);
            }
        }
        int MunitionDansCaisseMunitions { get; set; }
        int MunitionInitial { get; set; }
        float IntervalRechargement { get; set; }
        public string NomArme { get; private set; }
        public float Cadence { get; private set; }
        public int Dommage { get; private set; }
        public float Portée { get; private set; }
        float AngleDeTir { get; set; }
        bool Area { get; set; }
        public int NombreRechargeRestante
        {
            get
            {
                return (int)Math.Ceiling((float)MunitionTotalRestant / (float)MunitionMaxDansChargeur);
            }
        }
        public int NombreRechargeMax
        {
            get
            {
                return (int)Math.Ceiling((float)MunitionTotalMax / (float)MunitionMaxDansChargeur);
            }
        }
        public bool AucuneMunition
        {
            get
            {
                return MunitionRestantDansChargeur == MUNITION_MIN && MunitionTotalRestant == MUNITION_MIN;
            }
        }
        #endregion

        //public Fusil(Game jeu, DescriptionFusil description, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalMAJ)
        //    : base(jeu, description.NomModèle, échelleInitiale, rotationInitiale, positionInitiale, intervalMAJ)
        public Fusil(Game jeu, DescriptionFusil description, Vector3 positionInitiale, float rayon, float intervalMAJ)
            : base(jeu, positionInitiale, rayon, description.NomModèle, intervalMAJ)
        {
            AMunitionInfini = description.AMunitionInfini;
            NomArme = description.NomArme;
            Cadence = description.Cadence;
            Dommage = description.Dommage;
            MunitionTotalRestant = description.MunitionTotalRestant;
            MunitionTotalMax = description.MunitionTotalMax;
            MunitionMaxDansChargeur = description.MunitionMaxDansChargeur;
            MunitionDansCaisseMunitions = description.MunitionDansCaisseMunitions;
            MunitionRestantDansChargeur = description.MunitionRestantDansChargeur;
            //
            MunitionRestantDansChargeur = 20;
            MunitionInitial = description.MunitionInitial;
            Portée = description.Portée;
            AngleDeTir = description.AngleDeTir;
            Area = description.Area;
            NbBallesParTir = description.NbBallesParTir;
            IntervalRechargement = description.IntervalRechargement;
        }

        public override void Initialize()
        {
            EstImmuable = true;
            Direction = new Vector2(0, 1);
            TempsDepuisDernierTir = 0;
            TempsDepuisDebutJeu = 0;
            RechargeInitiale();
            ListeTrajectoires = new List<DroiteColorée>();
            TrajectoireExiste = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerDeModèle = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            ManagerDeMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            TempsDepuisDernierTir += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsDepuisDebutJeu += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TrajectoireExiste)
            {
                for (int i = 0; i < NbBallesParTir; ++i)
                {
                    ListeTrajectoires[i].Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        #region Munition
        int SetMunitionTotalRestant(int nombre)
        {
            return nombre < MUNITION_MIN ? MUNITION_MIN : nombre > MunitionTotalMax ? MunitionTotalMax : nombre;
        }

        int SetMunitionRestantDansChargeur(int nombre)
        {
            return nombre < MUNITION_MIN ? MUNITION_MIN : nombre > MunitionMaxDansChargeur ? MunitionMaxDansChargeur : nombre;
        }

        public void Tirer()
        {
            if (TempsDepuisDernierTir >= Cadence)
            {
                if (MunitionRestantDansChargeur > MUNITION_MIN)
                {
                    DroiteColorée trajectoire = new DroiteColorée(Game, this);
                    trajectoire.Initialize();
                    trajectoire.DroiteCollision.coupDeFeu();
                    ListeTrajectoires.Add(trajectoire);
                    GérerMunitions();
                    TrajectoireExiste = true;
                }
                else
                {
                    //Animation qui ne tire pas
                }
                TempsDepuisDernierTir = 0;
            }
        }

        public void ChangerPositionFusil(Vector3 position)
        {
            if (!float.IsNaN(Direction.X) && !float.IsNaN(Direction.Y))
            {
                Position = position + new Vector3(Direction.X, 0, -Direction.Y) * 2;
            }
        }

        public void ChangerDirection(Vector2 direction)
        {
            float distance = (float)Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2));
            if (distance < DISTANCE_JOUEUR_FUSIL)
            {
                Direction = direction * DISTANCE_JOUEUR_FUSIL / distance;
            }
            else
            {
                Direction = direction;
            }
        }

        void GérerMunitions()
        {
            MunitionRestantDansChargeur = !AMunitionInfini ? MunitionRestantDansChargeur - 1 : MunitionRestantDansChargeur;
            AnimationCoupDeFeu();
        }

        void AnimationCoupDeFeu()
        {
            if (Area)
            {

            }
        }

        public void RechargeInitiale()
        {
            AnimationRechargeInitiale();
            MunitionTotalRestant = MunitionInitial;
            MunitionRestantDansChargeur = MunitionInitial - MunitionMaxDansChargeur < MUNITION_MIN ? MunitionInitial : MunitionMaxDansChargeur;
            MunitionTotalRestant = MunitionInitial - MunitionMaxDansChargeur < MUNITION_MIN ? MUNITION_MIN : MunitionTotalRestant - MunitionMaxDansChargeur;
        }


        public void Recharger()
        {
            AnimationRecharge();
            if (MunitionRestantDansChargeur == MUNITION_MIN)
            {
                MunitionRestantDansChargeur = MunitionTotalRestant - MunitionMaxDansChargeur < MUNITION_MIN ? MunitionTotalRestant : MunitionMaxDansChargeur;
                MunitionTotalRestant = MunitionTotalRestant - MunitionMaxDansChargeur < MUNITION_MIN ? MUNITION_MIN : MunitionTotalRestant - MunitionMaxDansChargeur;
            }
            else
            {
                int munitionRestantDansChargeur = MunitionRestantDansChargeur;
                MunitionRestantDansChargeur = MunitionTotalRestant - (MunitionMaxDansChargeur - MunitionRestantDansChargeur) < MUNITION_MIN ? MunitionRestantDansChargeur + MunitionTotalRestant : MunitionMaxDansChargeur;
                MunitionTotalRestant = MunitionTotalRestant - (MunitionMaxDansChargeur - munitionRestantDansChargeur) < MUNITION_MIN ? MUNITION_MIN : MunitionTotalRestant - (MunitionMaxDansChargeur - munitionRestantDansChargeur);
            }
        }

        public void RécupérerMunitions()
        {
            MunitionTotalRestant += !AMunitionInfini ? MunitionDansCaisseMunitions : MUNITION_MIN;
        }

        #endregion

        void AnimationRechargeInitiale()
        {

        }

        void AnimationRecharge()
        {

        }

        public void Animer()
        {
            Position = new Vector3(Position.X, (float)Math.Sin(MathHelper.ToRadians(++AngleRotation)) - 5, Position.Z);
        }

        public void ChangerVitesse(Vector3 vitesse)
        {
            Vitesse += vitesse;
        }

        int SetAngleRotation(int angle)
        {
            return angle < ZERO ? ZERO : angle % MAX_DEGRÉS;// (float)MathHelper.ToDegrees(MathHelper.TwoPi);// ? angle - (float)MathHelper.TwoPi : angle;
        }

        public override void EnCollision(ObjetPhysique autre, InformationIntersection infoColli)
        {
            if (autre is Joueur)
            {
                ManagerDeMessage.AjouterÉvénement((int)Message.DésactivationSupportFusil);
            }
        }

        public new void SetCaméra(Caméra cam)
        {
            foreach (DroiteColorée ligneDeTir in ListeTrajectoires)
            {
                ligneDeTir.ChangerCaméra(cam);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DroiteColorée ligneDeTir in ListeTrajectoires)
            {
                ligneDeTir.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void EffacerDroite()
        {
            for (int i = NbBallesParTir - 1; i >= 0; --i)
            {
                ListeTrajectoires.RemoveAt(i);
            }

            TrajectoireExiste = false;
        }

        //public override string ToString()
        //{
        //    return "Total: " + MunitionTotalRestant + " / " + MunitionTotalMax + "     Chargeur: " + MunitionRestantDansChargeur + " / " + MunitionMaxDansChargeur + "      y: " + Position.Y;
        //}
    }
}