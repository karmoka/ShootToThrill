﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AnimationAux;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
    class MAvatar : Entité, IPositionable, IModele3d, IPhysique
    {
        int _vie,
            _indexArme;
        const int MORT = 0,
                  INDEX_PREMIER_FUSIL = 0,
                  NOMBRE_FUSIL_MAX = 5,
                  AUCUNE_MUNITION = 0,
                  VARIATION_INDEX = 1,
                  DISTANCE_MAX = 1000;
        protected float VitesseJoueur { get; set; }
        float IntervalMAJ { get; set; }
        float TempsDepuisMAJ { get; set; }
        protected bool AUnFusil
        {
            get { return ListeArme.Count >= 1; }
        }
        public bool EstMort
        {
            get { return Vie <= MORT; }
        }
        public int VieMax { get; protected set; }

        public IModele3d ComposanteGraphique { get; private set; }
        public ObjetPhysique ComposantePhysique { get; private set; }
        protected List<Fusil> ListeArme { get; set; }
        protected MMoteurPhysique MMoteurPhysique { get; set; }
        protected ModelManager ManagerModèle { get; set; }

        /// <summary>
        /// Avatar dont le graphique ne bouge pas
        /// </summary>
        /// <param name="game"></param>
        /// <param name="composanteGraphique"></param>
        /// <param name="composantePhysique"></param>
        public MAvatar(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique)
            : base(game)
        {
            ComposantePhysique = composantePhysique;
            ComposanteGraphique = composanteGraphique;
            VieMax = 100;
        }

        public MAvatar(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, int vie)
            : base(game)
        {
            ComposantePhysique = composantePhysique;
            ComposanteGraphique = composanteGraphique;
            VieMax = vie;
        }
        public MAvatar(Game game, DescriptionAvatar description, Vector3 position )
            : base(game)
        {
            VieMax = description.VieMax;

            ComposantePhysique = new ObjetPhysique(game, description.DescriptionComposantePhysique, position);
            ComposanteGraphique = new MObjetDeBaseAniméEtÉclairé(game, description.DescriptionComposanteGraphique, position, 1 / 60f);
        }

        public override void Initialize()
        {
            TypeEnt = TypeEntité.Avatar;
            ComposanteGraphique.Initialize();
            ComposantePhysique.Initialize();
            ListeArme = new List<Fusil>(NOMBRE_FUSIL_MAX);
            IntervalMAJ = 1 / 60f;
            TempsDepuisMAJ = 0;
            IndexArme = 0;
            Vie = VieMax;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerModèle = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            MMoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!EstMort)
            {
                if (TempsDepuisMAJ >= IntervalMAJ)
                {
                    BougerAvatar();
                    BougerArme();
                    ComposanteGraphique.SetPosition(ComposantePhysique.Position);
                    TempsDepuisMAJ = 0;
                }
                if (AUnFusil)
                {
                    ArmeSélectionnée.Update(gameTime); //
                }
            }
            base.Update(gameTime);
        }

        public ObjetPhysique GetObjetPhysique()
        {
            return ComposantePhysique;
        }

        protected virtual void BougerAvatar()
        {
            if (Position.X > DISTANCE_MAX || Position.X < -DISTANCE_MAX ||
                Position.Y > DISTANCE_MAX || Position.Y < -DISTANCE_MAX ||
                Position.Z > DISTANCE_MAX || Position.Z < -DISTANCE_MAX)
            {
                Mourir();
            }
        }

        protected virtual void BougerArme()
        {
            if (AUnFusil)
            {
                ArmeSélectionnée.SetPosition(this);
                ArmeSélectionnée.SetRotation(this);
            }
        }

        protected virtual void GérerCollisions()
        {
            for (int i = 0; i < ComposantePhysique.ListeCollision.Count; ++i)
            {

            }
        }

        public void AjouterArme(Fusil fusil)
        {
            if (!ListeArme.Exists(x => x.NomArme == fusil.NomArme))
            {
                ListeArme.Add(fusil);
                IndexArme = ListeArme.FindIndex(x => x.NomArme == fusil.NomArme);
            }
            //MoteurPhysique.EnleverObjet(fusil);
            //ManagerModèle.EnleverModèle(fusil);
        }

        protected void RetirerArme(Fusil fusil)
        {
            ListeArme.Remove(fusil);
        }

        protected void ChangerArme(bool monter)
        {
            IndexArme += monter ? VARIATION_INDEX : -VARIATION_INDEX;
            if (ListeArme.Exists(x => x != ArmeSélectionnée && x.AucuneMunition))
            {
                RetirerArme(ListeArme.Find(x => x != ArmeSélectionnée && x.AucuneMunition));
            }
        }

        protected void AjouterMunition(Munition munition)
        {
            if (ListeArme != null && ListeArme.Exists(x => x.NomArme == munition.NomFusil))
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
                    ListeArme.Find(x => x.NomArme == munition.NomFusil).RécupérerMunitions();
                }
            }
        }

        public void Attaquer()
        {
            if (AUnFusil)
            {
                ArmeSélectionnée.Attaquer();
            }
        }

        public void Recharger()
        {
            if (AUnFusil)
            {
                ArmeSélectionnée.Recharger();
            }
        }

        protected void AjouterVie(Soin soin)
        {
            if (!EstMort)
            {
                Vie += soin.NombreSoin;
            }
            //MoteurPhysique.EnleverObjet(soin);
            //ManagerModèle.EnleverModèle(soin);
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
            //ComposanteGraphique = null;
            //ComposantePhysique = null;
            Vie = MORT;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!EstMort)
            {
                ComposanteGraphique.Draw(gameTime);
                ComposantePhysique.Draw(gameTime);

                DrawArme(gameTime);

                base.Draw(gameTime);
            }
        }

        void DrawArme(GameTime gameTime)
        {
            if (AUnFusil)
            {
                ArmeSélectionnée.Draw(gameTime);
            }
        }

        public Vector3 Position
        {
            get { return ComposantePhysique.Position; }
        }

        public void SetPosition(Vector3 nouvellePosition)
        {
            ComposantePhysique.SetPosition(nouvellePosition);
            ComposanteGraphique.SetPosition(nouvellePosition);
        }

        public void SetRotation(Vector3 rotation)
        {
            ComposanteGraphique.SetRotation(rotation);
            ComposantePhysique.SetRotation(rotation);
        }

        protected virtual void ModifierDirection(Vector2 direction)
        {
            SetRotation(Vector3.UnitY * CustomMathHelper.AngleDeVecteur2D(direction));
        }

        public Vector3 Vitesse
        {
            get { return ComposantePhysique.Vitesse; }
            set
            {
                ComposantePhysique.SetVitesse(value);
            }
        }

        public int Vie
        {
            get
            {
                return _vie;
            }
            protected set
            {
                _vie = SetVie(value);
            }
        }

        int SetVie(int vie)
        {
            return vie >= MORT ? vie <= VieMax ? vie : VieMax : MORT;
        }

        public Fusil ArmeSélectionnée
        {
            get
            {
                return ListeArme[IndexArme];
            }
            protected set
            {
                ListeArme[IndexArme] = value;
            }
        }

        protected int IndexArme
        {
            get { return _indexArme; }
            set
            {
                _indexArme = SetIndexArme(value);
            }
        }

        int SetIndexArme(int index)
        {
            return index < INDEX_PREMIER_FUSIL ? ListeArme.Count - 1 : index > ListeArme.Count - 1 ? INDEX_PREMIER_FUSIL : index;
        }

        public void SetCaméra(Caméra cam)
        {
            ComposanteGraphique.SetCaméra(cam);
            SetCaméraAutreComposante(cam);
        }

        protected virtual void SetCaméraAutreComposante(Caméra cam)
        {
            if (AUnFusil)
            {
                ArmeSélectionnée.SetCaméra(cam);
            }
        }

        public Collider GetCollider()
        {
            return new SphereCollision(this.Position, 1.25f);
        }
    }
}