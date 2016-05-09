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
using AnimationAux;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
    class MAvatar : EntitéGraphiqueEtPhysique, IPositionable, IModele3d, IPhysique
    {
        int _vie,
            _indexArme;
        const int MORT = 0,
                  INDEX_PREMIER_FUSIL = 0,
                  NOMBRE_FUSIL_MAX = 5,
                  AUCUNE_MUNITION = 0,
                  VARIATION_INDEX = 1,
                  DISTANCE_MAX = 200;
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
        public float VitesseAvatarMaximum { get; set; }

        protected List<Fusil> ListeArme { get; set; }




        public MAvatar(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique)
            : base(game, composanteGraphique, composantePhysique)
        {
            VieMax = 100;
        }

        public MAvatar(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, int vie)
           : base(game, composanteGraphique, composantePhysique)
        {
            VieMax = vie;
        }
        public MAvatar(Game game, DescriptionAvatar description, Vector3 position )
            : base(game, description, position)
        {
            VieMax = description.VieMax;
        }

        public override void Initialize()
        {
            TypeEnt = TypeEntité.Avatar;
            ComposanteGraphique.Initialize();
            ComposantePhysique.Initialize();
            ListeArme = new List<Fusil>(NOMBRE_FUSIL_MAX);

            IntervalMAJ = OptionsJeu.IntervalMAJStandard;
            TempsDepuisMAJ = 0;

            IndexArme = 0;
            Vie = VieMax;

            base.Initialize();

            ComposantePhysique.ÉtatsPhysiques.Add(new ÉtatPhysique(Game, CustomMathHelper.E(-4), null));
        }

        public override void Update(GameTime gameTime)
        {
           TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;

           if (!EstMort && TempsDepuisMAJ >= IntervalMAJ)
           {
              BougerAvatar();
              BougerArme();
              GérerCollisions();

              TempsDepuisMAJ = 0;
           }
           if (!EstMort && AUnFusil)
           {
              ArmeSélectionnée.Update(gameTime); 
           }

           base.Update(gameTime);
        }

        public ObjetPhysique GetObjetPhysique()
        {
            return ComposantePhysique;
        }

        protected virtual void BougerAvatar()
        {
            if (Position.Length() >= DISTANCE_MAX)
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
            //for (int i = 0; i < ComposantePhysique.ListeCollision.Count; ++i)
            //{

            //}
        }

        public void AjouterArme(Fusil fusil)
        {
            if (!ListeArme.Exists(x => x.NomArme == fusil.NomArme) && fusil != null)
            {
                fusil.IdPropriétaire = this.UniqueId;
                ListeArme.Add(fusil);
                IndexArme = ListeArme.FindIndex(x => x.NomArme == fusil.NomArme);
            }
            //MoteurPhysique.EnleverObjet(fusil);
            //ManagerModèle.EnleverModèle(fusil);
        }

        protected void RetirerArme(Fusil fusil)
        {
            fusil.IdPropriétaire = Entité.ID_AUCUN_PROPRIÉTAIRE;
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
            Vie = MORT;
            ChangerÉtatGraphique();
            ChangerÉtatPhysique();
        }

        public override void Draw(GameTime gameTime)
        {
           if (AUnFusil)
           {
              ArmeSélectionnée.Draw(gameTime);
           }

           base.Draw(gameTime);
        }

        protected virtual void TournerSurY(Vector2 direction)
        {
            SetRotation(CustomMathHelper.DéterminerRotationModeleBlender(direction));
        }

        public Vector3 Vitesse
        {
            get { return ComposantePhysique.Vitesse; }
            protected set
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
                _vie = VérifierValeurVie(value);
            }
        }

        int VérifierValeurVie(int vie)
        {
            return vie >= MORT ? vie <= VieMax ? vie : VieMax : MORT;
        }

        public Fusil ArmeSélectionnée
        {
            get
            {
                return ListeArme[IndexArme];
            }
            private set
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
           //Si l'index dépasse la liste, il est retourné à zéro.
            return index < INDEX_PREMIER_FUSIL ? ListeArme.Count - 1 : index > ListeArme.Count - 1 ? INDEX_PREMIER_FUSIL : index;
        }

        public override void SetCaméra(Caméra cam)
        {
            if (AUnFusil)
            {
               ArmeSélectionnée.SetCaméra(cam);
            }

            base.SetCaméra(cam);
        }

        public override Collider GetCollider()
        {
           return new SphereCollision(this.Position, 1f);
        }
    }
}