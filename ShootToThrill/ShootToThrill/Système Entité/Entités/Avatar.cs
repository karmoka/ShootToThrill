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
    class Avatar : ModelPhysique, IModele3d
    {

        const float VITESSE_MAX = 10f; //Arbitraire
        int _Vie,
            _Index;
        const int MORT = 0,
                  INDEX_PREMIER_FUSIL = 0,
                  NOMBRE_FUSIL_MAX = 5,
                  AUCUNE_MUNITION = 0,
                  VARIATION_INDEX = 1;

        protected ModelManager ManagerModèle { get; set; }
        protected MoteurPhysique MoteurPhysique { get; set; }
        protected float VitesseJoueur { get; set; }
        float IntervalMAJ { get; set; }
        float TempsDepuisMAJ { get; set; }
        public int Vie
        {
            get
            {
                return _Vie;
            }
            protected set
            {
                _Vie = SetVie(value);
            }
        }
        public int VieMax { get; private set; }
        protected List<Fusil> ListeArmes { get; set; }
        public Fusil ArmeSélectionnée
        {
            get
            {
                return ListeArmes[Index];
            }
            private set
            {
                ListeArmes[Index] = value;
            }
        }
        int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = SetIndex(value);
            }
        }
        protected bool AUnFusil
        {
            get { return ListeArmes.Count == 1; }
        }
        bool EstMort
        {
            get { return Vie <= MORT; }
        }
        public int Score { get; private set; }

        public Avatar(Game game, Vector3 position, Vector3 vitesse, float masseInverse, float rayon, string nomModèle, int vie)
            : base(game, position, vitesse, masseInverse, rayon, nomModèle)
        {
            VieMax = GameConstants.ViePersonnageMax;
            Vie = vie;
            IntervalMAJ = GameConstants.INTERVAL_MAJ_STANDARD;
        }
        public Avatar(Game game, DescriptionAvatar description)
            : base(game, description)
        {
            VieMax = description.VieMax;
            Vie = description.VieMax;
            IntervalMAJ = GameConstants.INTERVAL_MAJ_STANDARD;
        }

        public override void Initialize()
        {
            TempsDepuisMAJ = 0;
            Score = 0;
            ListeArmes = new List<Fusil>(NOMBRE_FUSIL_MAX);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MoteurPhysique = Game.Services.GetService(typeof(MoteurPhysique)) as MoteurPhysique;
            ManagerModèle = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            base.LoadContent();
        }

        protected void AjouterFusilInitial(Fusil fusil)
        {
            ListeArmes.Add(fusil);
        }

        public override void Update(GameTime gameTime)
        {
            TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsDepuisMAJ >= IntervalMAJ)
            {
                if (!EstMort)
                {
                    BougerAvatar();
                    CorrigerVitesse();
                }
                TempsDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        protected virtual void BougerAvatar()
        {

        }

        void CorrigerVitesse()
        {
            float LongueurComposanteXZ = (float)Math.Sqrt(Vitesse.X * Vitesse.X + Vitesse.Z * Vitesse.Z);
            if (LongueurComposanteXZ >= VITESSE_MAX)
            {
                float ratio = LongueurComposanteXZ / VITESSE_MAX;
                Vitesse = new Vector3(Vitesse.X / ratio, Vitesse.Y, Vitesse.Z / ratio);
            }
        }

        public override void EnCollision(ObjetPhysique autre, InformationIntersection infoColli)
        {
            base.EnCollision(autre, infoColli);
        }

        #region Vie
        int SetVie(int vie)
        {
            return vie >= MORT ? vie <= VieMax ? vie : VieMax : MORT;
        }

        //void GérerVie()
        //{
        //    if (!EstMort)
        //    {
        //        GérerDomage();
        //    }
        //}

        //void GérerDomage()
        //{
        //    //S'il y a eu collision entre un bonhomme et une balle
        //    //Trouver le type d'arme
        //    int domage = 0; //Donner le dommage
        //    RetirerVie(domage);
        //}

        protected void AjouterVie(Soin soin)
        {
            if (!EstMort)
            {
                Vie += soin.NombreSoin;
            }
        }

        public void RetirerVie(int domageReçu)
        {
            Vie -= domageReçu;
            if (EstMort)
            {
                Mourir();
            }
        }

        protected virtual void Mourir()
        {
            ManagerModèle.EnleverModèle(this);
            MoteurPhysique.EnleverObjet(this);
        }
        #endregion

        #region Arme

        int SetIndex(int index)
        {
            return index < INDEX_PREMIER_FUSIL ? ListeArmes.Count - 1 : index > ListeArmes.Count - 1 ? INDEX_PREMIER_FUSIL : index;
        }

        protected void RetirerArme(Fusil fusil)
        {
            ListeArmes.Remove(fusil);
            Index = ListeArmes.FindIndex(x => x == ArmeSélectionnée);
        }

        protected void Tirer()
        {
            ArmeSélectionnée.Tirer();
        }

        protected void Recharger()
        {
            ArmeSélectionnée.Recharger();
        }

        protected void ChangerArme(bool monter)
        {
            ArmeSélectionnée.Visible = false;
            Index += monter ? VARIATION_INDEX : -VARIATION_INDEX;
            ArmeSélectionnée.Visible = true;
            if (ListeArmes.Exists(x => x != ArmeSélectionnée && x.AucuneMunition))
            {
                RetirerArme(ListeArmes.Find(x => x != ArmeSélectionnée && x.AucuneMunition));
            }
        }

        protected void AjouterArme(Fusil fusil)
        {
            if (fusil.Enabled)
            {
                if (ListeArmes == null || !ListeArmes.Exists(x => x == fusil))
                {
                    ListeArmes.Add(fusil);
                    Index = ListeArmes.FindIndex(x => x == fusil);
                    ManagerModèle.AjouterModele(fusil);
                }
            }
        }

        protected void AjouterMunition(Munition munition)
        {
            if (ListeArmes != null && ListeArmes.Exists(x => x.NomArme == munition.NomFusil))
            {
                if (ArmeSélectionnée.NomArme == munition.NomFusil)
                {
                    ArmeSélectionnée.RécupérerMunitions();
                    //if (ArmeSélectionnée.MunitionRestantDansChargeur == AUCUNE_MUNITION)
                    //{
                    //    ArmeSélectionnée.Recharger();
                    //}
                }
                else
                {
                    ListeArmes.Find(x => x.NomArme == munition.NomFusil).RécupérerMunitions();
                }
            }
        }

        public void ChangerPositionFusil(Vector3 position)
        {
            ArmeSélectionnée.ChangerPositionFusil(position);
        }

        #endregion

        public void SetCaméra(Caméra cam)
        {
            ArmeSélectionnée.SetCaméra(cam);
            base.SetCaméra(cam);
        }

        public override void Draw(GameTime gameTime)
        {
            ArmeSélectionnée.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}