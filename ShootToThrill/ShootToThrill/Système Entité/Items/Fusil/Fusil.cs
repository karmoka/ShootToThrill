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
    public class Fusil : Item, IModele3d, IArme
    {
       const string NOM_BRUIT_DÉFAUT = "Pistol";
       ManagerAudio ManagerDeSons { get; set; }
       protected string NomBruitFusil { get; set; }

        const int ZERO = 0,
                  MAX_DEGRÉS = 360;
        const float DISTANCE_JOUEUR_FUSIL = 0.5f;
        int _angle;
        public Vector2 Direction { get; private set; }
        GameTime GameTime { get; set; }
        MessageManager ManagerDeMessage { get; set; }
        ModelManager ManagerDeModèle { get; set; }
        protected List<DroiteColorée> ListeTrajectoires { get; set; }
        protected List<Projectile> ListeProjectile { get; set; }
        bool TrajectoireExiste { get; set; }

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
        float TempsDepuisDernierTir { get; set; }
        float TempsDepuisDebutJeu { get; set; }
        float TempsDepuisRecharge { get; set; }

        int Score { get; set; }

        #region Munition
        int _munitionTotalRestant,
            _munitionRestantDansChargeur;

        const int MUNITION_MIN = 0,
                  UNE_MUNITION = 1;
        protected int NbBallesParTir { get; set; }
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
        protected int AngleDeTir { get; set; }
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

        public Fusil(Game jeu, DescriptionFusil description, Vector3 positionInitiale, float rayon, float intervalMAJ)
            : base(jeu, positionInitiale, rayon, description.NomModèle, intervalMAJ, true)
        {
           NomBruitFusil = NOM_BRUIT_DÉFAUT;

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

            ComposantePhysique.EstImmuable = true;
        }

        public override void Initialize()
        {
            ComposantePhysique.EstImmuable = true;
            Direction = new Vector2(0, 1);
            TempsDepuisDernierTir = 0;
            TempsDepuisDebutJeu = 0;
            TempsDepuisRecharge = 0;
            RechargeInitiale();
            Score = 0;
            ListeTrajectoires = new List<DroiteColorée>();
            ListeProjectile = new List<Projectile>();
            TrajectoireExiste = false;
            Visible = false;
            base.Initialize();
            ChangerÉtatGraphique();
        }

        protected override void LoadContent()
        {
            ManagerDeSons = Game.Services.GetService(typeof(ManagerAudio)) as ManagerAudio;
            ManagerDeModèle = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            ManagerDeMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            float tempsDepuisMAJ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsDepuisDernierTir += tempsDepuisMAJ;
            TempsDepuisDebutJeu += tempsDepuisMAJ;
            TempsDepuisRecharge += tempsDepuisMAJ;

            foreach(Projectile projectile in ListeProjectile)
            {
                projectile.Update(gameTime);
            }

            ListeProjectile.RemoveAll(x => x.Existe == false);

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

        public void Attaquer()
        {
            if (TempsDepuisDernierTir >= Cadence)
            {
                if (MunitionRestantDansChargeur > MUNITION_MIN)
                {
                    for (int i = 0; i < NbBallesParTir; ++i)
                    {
                        Tirer();
                    }
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

        protected virtual void Tirer()
        {
           ManagerDeSons.JouerSons(NomBruitFusil);
        }

        protected Vector3 DirectionAléatoire(Vector3 axe, Random générateurAléatoire)
        {
            double nouvelAngleX = générateurAléatoire.Next(-AngleDeTir, AngleDeTir);
            double nouvelAngleY = générateurAléatoire.Next(-AngleDeTir, AngleDeTir);

            nouvelAngleX *= Math.PI / 180;
            nouvelAngleY *= Math.PI / 180;

            double angleX = Math.Atan2(axe.Z, axe.X);
            double angleY = Math.Atan2(axe.Y, Math.Sqrt(Math.Pow(axe.X, 2) + Math.Pow(axe.Z, 2)));

            double x = Math.Cos(nouvelAngleX + angleX);
            double y = Math.Sin(nouvelAngleY + angleY);
            double z = Math.Sin(nouvelAngleX + angleX);

            return new Vector3((float)x, 0, (float)z);
        }

        public void SetPosition(Vector3 position)
        {
            if (!float.IsNaN(Direction.X) && !float.IsNaN(Direction.Y))
            {
               base.SetPosition(position + new Vector3(Direction.X, 0, -Direction.Y) * 2);
            }
        }

        public void SetPosition(IPositionable i)
        {
            if (!float.IsNaN(Direction.X) && !float.IsNaN(Direction.Y))
            {
                base.SetPosition(i.Position + new Vector3(Direction.X, 0, -Direction.Y) * 2);
            }
        }

        public void SetRotation(IPositionable i)
        {
            //base.SetRotation(i.Rotation);
        }

        public void ChangerDirection(Vector2 direction)
        {
            float distance = direction.Length();// (float)Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2));
            if (distance > DISTANCE_JOUEUR_FUSIL)
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

        public void RechargerContinu()
        {
            if(TempsDepuisRecharge >= IntervalRechargement)
            {
                MunitionRestantDansChargeur += MunitionRestantDansChargeur > ZERO ? UNE_MUNITION : ZERO;
                TempsDepuisRecharge = 0;
            }
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

        //public void AjouterPropriétaire(MJoueur joueur)
        //{
        //    Propriétaire = joueur;
        //}

        public void Animer()
        {
           this.SetPosition(new Vector3(Position.X, (float)Math.Sin(MathHelper.ToRadians(++AngleRotation)) - 5, Position.Z));
        }

        public void ChangerVitesse(Vector3 vitesse)
        {
           ComposantePhysique.SetVitesse(ComposantePhysique.Vitesse + vitesse);
        }

        int SetAngleRotation(int angle)
        {
            return angle < ZERO ? ZERO : angle % MAX_DEGRÉS;// (float)MathHelper.ToDegrees(MathHelper.TwoPi);// ? angle - (float)MathHelper.TwoPi : angle;
        }

        //public override void EnCollision(IPhysique autre)
        //{
        //    if (autre is MJoueur)
        //    {
        //        ManagerDeMessage.AjouterÉvénement((int)Message.DésactivationSupportFusil);
        //    }
        //}

        public new void SetCaméra(Caméra cam)
        {
            foreach (Projectile projectile in ListeProjectile)
            {
                projectile.ChangerCaméra(cam);
            }

            base.SetCaméra(cam);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Projectile projectile in ListeProjectile)
            {
                projectile.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        //public override string ToString()
        //{
        //    return "Total: " + MunitionTotalRestant + " / " + MunitionTotalMax + "     Chargeur: " + MunitionRestantDansChargeur + " / " + MunitionMaxDansChargeur + "      y: " + Position.Y;
        //}
    }
}