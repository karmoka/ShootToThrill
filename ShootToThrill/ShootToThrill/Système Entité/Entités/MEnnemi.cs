using System;
using System.Collections;
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
    class MEnnemi : MAvatar
    {
        const float INTERVAL_ATTAQUE = 1f;
        int _domage;
        Vector3 PositionAvatarPlusProche { get; set; }
        float IntervalRechercheAvatar { get; set; }
        float TempsDepuisRechercheAvatar { get; set; }
        float TempsDepuisAttaque { get; set; }
        Pathfinding Pathfinding { get; set; }
        int Index { get; set; }
        Vector3[] Path { get; set; }
        int ObjetDrop { get; set; }
        public int Domage
        {
            get
            {
                return EstEnTrainAttaquer ? _domage : 0;
            }
            private set
            {
                _domage = value;
            }
        }
        bool EstEnTrainAttaquer { get; set; }
        EnnemiScreenManager EnnemiScreenManager { get; set; }

        public MEnnemi(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique, int objetDrop, int vie, int domage)
            : base(game, composanteGraphique, composantePhysique, vie)
        {
            ObjetDrop = objetDrop;
            Domage = domage;
        }
        public MEnnemi(Game game, DescriptionEnnemi description, Vector3 position, int objetDrop)
           : base(game, description,position)
        {
           ObjetDrop = objetDrop;
           Domage = description.Domage;
        }

        public override void Initialize()
        {
            IntervalRechercheAvatar = 0.05f;
            VitesseJoueur = 0.1f;
            TempsDepuisRechercheAvatar = 0;
            TempsDepuisAttaque = 0;
            EstEnTrainAttaquer = false;
            base.Initialize();
            EnnemiScreenManager = new EnnemiScreenManager(Game, this);
            EnnemiScreenManager.Initialize();
        }
        protected override void LoadContent()
        {
            Pathfinding = Game.Services.GetService(typeof(Pathfinding)) as Pathfinding;
            base.LoadContent();
            PositionAvatarPlusProche = MoteurPhysique.GetPositionJoueurPlusProche(this.Position);//MMoteurPhysique.GetPositionJoueurPlusProche(Position);
            RequêtePathManager.PathRequête(Position, PositionAvatarPlusProche, OnPathFound);
        }

        public override void Update(GameTime gameTime)
        {
            float tempsDepuisMAJ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsDepuisRechercheAvatar += tempsDepuisMAJ;
            if (TempsDepuisRechercheAvatar >= IntervalRechercheAvatar)
            {
               PositionAvatarPlusProche = MoteurPhysique.GetPositionJoueurPlusProche(this.Position); //MMoteurPhysique.GetPositionJoueurPlusProche(Position);
                UpdaterComportement(tempsDepuisMAJ);
                EnnemiScreenManager.Update(gameTime);
                TempsDepuisRechercheAvatar = 0;
            }
            base.Update(gameTime);
        }

        void UpdaterComportement(float tempsDepuisMAJ)
        {
            EstEnTrainAttaquer = PositionAvatarPlusProche.Length() <= 1;
            if (!EstEnTrainAttaquer)
            {
                DéplacerEnnemi();
            }
            else
            {
                Attaquer(tempsDepuisMAJ);
            }
        }

        void DéplacerEnnemi()
        {
            RequêtePathManager.PathRequête(Position, PositionAvatarPlusProche, OnPathFound);
            TournerSurY(new Vector2(PositionAvatarPlusProche.X - Position.X, PositionAvatarPlusProche.Z - Position.Z));
        }

        void Attaquer(float tempsDepuisMAJ)
        {
            TempsDepuisAttaque += tempsDepuisMAJ;
            if (TempsDepuisAttaque >= INTERVAL_ATTAQUE)
            {
                MoveTowards(PositionAvatarPlusProche, Position);
                TempsDepuisAttaque = 0;
            }
        }

        protected override void BougerAvatar()
        {
            if (Path != null)
            {
                if (Path.Length > 0 && !EstEnTrainAttaquer)
                {
                    FollowPath();
                }
            }
            base.BougerAvatar();
        }

        protected override void TournerSurY(Vector2 direction)
        {
            base.TournerSurY(direction);
        }

        protected override void Mourir()
        {
           RandomItemDrop itemDrop = new RandomItemDrop(Game);
           Item item = itemDrop.GetRandomItem(ObjetDrop, Position);
           if (item != null)
           {
              item.Initialize();
              ManagerModele.AjouterModele(item);
              MoteurPhysique.AjouterObjet(item as IPhysique);
           }

           base.Mourir();
           this.Dispose();
        }

        protected override void GérerCollisions()
        {
            foreach (IPhysique i in ComposantePhysique.ListeCollision)
            {
                if (i is MJoueur)
                {
                    EstEnTrainAttaquer = false;
                }
            }
        }

        #region Pathfinding
        public void OnPathFound(Vector3[] nouveauPath, bool pathSuccessfull)
        {
            if (pathSuccessfull)
            {
                if (nouveauPath != null)
                {
                    if (nouveauPath.Length > 0 && !EstEnTrainAttaquer)
                    {
                        if (Path == nouveauPath)
                        {
                            FollowPath();
                        }
                        else
                        {
                            Path = nouveauPath;
                            FollowNouveauPath();
                        }
                    }
                }
            }
        }

        void FollowNouveauPath()
        {
            Vector3 wayPointsActuels = Path[0];
            Index = 0;
            while (true)
            {
                Vector3 positionMonde = Pathfinding.GetPositionActuelleMonde(Position).PositionMonde;
                if (positionMonde == wayPointsActuels)
                {
                    ++Index;
                    if (Index >= Path.Length)
                    {
                        Index = 0;
                        MoveTowards(PositionAvatarPlusProche, Position);
                        break;
                    }
                    wayPointsActuels = Path[Index];
                }
                MoveTowards(wayPointsActuels, positionMonde);
                return;
            }
        }

        void FollowPath()
        {
            Vector3 wayPointsActuels = Path[Index];
            while (true)
            {
                Vector3 positionMonde = Pathfinding.GetPositionActuelleMonde(Position).PositionMonde;
                if (positionMonde == wayPointsActuels)
                {
                    ++Index;
                    if (Index >= Path.Length)
                    {
                        Index = 0;
                        MoveTowards(PositionAvatarPlusProche, Position);
                        break;
                    }
                    wayPointsActuels = Path[Index];
                }
                MoveTowards(wayPointsActuels, positionMonde);
                return;
            }
        }

        void MoveTowards(Vector3 wayPointsActuels, Vector3 positionMonde)
        {
            float x = wayPointsActuels.X - positionMonde.X;
            float z = wayPointsActuels.Z - positionMonde.Z;
            Vitesse += x < 0 ? new Vector3(-VitesseJoueur, 0, 0) : x > 0 ? new Vector3(VitesseJoueur, 0, 0) : Vector3.Zero;
            Vitesse += z < 0 ? new Vector3(0, 0, -VitesseJoueur) : z > 0 ? new Vector3(0, 0, VitesseJoueur) : Vector3.Zero;
        }
        #endregion

        public override void SetCaméra(Caméra cam)
        {
           EnnemiScreenManager.SetCaméra(cam);

           base.SetCaméra(cam);
        }

        public override void Draw(GameTime gameTime)
        {
            EnnemiScreenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}