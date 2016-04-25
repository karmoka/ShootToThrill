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
    class Ennemi : Avatar
    {
        Vector3 PositionAvatarPlusProche { get; set; }
        float IntervalRechercheAvatar { get; set; }
        float TempsDepuisRechercheAvatar { get; set; }
        Pathfinding Pathfinding { get; set; }
        int Index { get; set; }
        Vector3[] Path { get; set; }
        int ObjetDrop { get; set; }
        public int Domage { get; private set; }
        public Ennemi(Game game, Vector3 position, Vector3 vitesse, float masseInverse, float rayon, string nomModèle, int vie, int domage, int objetDrop)
            : base(game, position, vitesse, masseInverse, rayon, nomModèle, vie)
        {
            IntervalRechercheAvatar = 0.05f;
            ObjetDrop = objetDrop;
            Domage = domage;
        }

        public Ennemi(Game game, DescriptionJoueur description)
            : base(game, description)
        {

        }

        public override void Initialize()
        {
            VitesseJoueur = 0.1f;
            TempsDepuisRechercheAvatar = 0;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            PositionAvatarPlusProche = MoteurPhysique.GetPositionJoueurPlusProche(Position);
            Pathfinding = Game.Services.GetService(typeof(Pathfinding)) as Pathfinding;
            RequêtePathManager.PathRequête(Position, PositionAvatarPlusProche, OnPathFound);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            TempsDepuisRechercheAvatar += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsDepuisRechercheAvatar > IntervalRechercheAvatar)
            {
                PositionAvatarPlusProche = MoteurPhysique.GetPositionJoueurPlusProche(Position);
                RequêtePathManager.PathRequête(Position, PositionAvatarPlusProche, OnPathFound);
                TempsDepuisRechercheAvatar = 0;
            }
            base.Update(gameTime);
        }

        protected override void BougerAvatar()
        {
            FollowPath();
        }

        protected override void Mourir()
        {
            RandomItemDrop itemDrop = new RandomItemDrop(Game);
            Item item = itemDrop.GetRandomItem(ObjetDrop, Position);
            if (item != null)
            {
                item.Initialize();
                ManagerModèle.AjouterModele(item);
                MoteurPhysique.AjouterObjet(item);
            }
            base.Mourir();
        }

        public override void EnCollision(ObjetPhysique autre, InformationIntersection infoColli)
        {
            if (autre is Fusil)
            {
                AjouterArme(autre as Fusil);
            }
        }

        #region Pathfinding
        public void OnPathFound(Vector3[] nouveauPath, bool pathSuccessfull)
        {
            if (pathSuccessfull)
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

        void FollowNouveauPath()
        {
            if (Path != null)
            {
                if (Path.Length > 0)
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

        public void MoveTowards(Vector3 wayPointsActuels, Vector3 positionMonde)
        {
            float x = wayPointsActuels.X - positionMonde.X;
            float z = wayPointsActuels.Z - positionMonde.Z;
            Vitesse += x < 0 ? new Vector3(-VitesseJoueur, 0, 0) : x > 0 ? new Vector3(VitesseJoueur, 0, 0) : Vector3.Zero;
            Vitesse += z < 0 ? new Vector3(0, 0, -VitesseJoueur) : z > 0 ? new Vector3(0, 0, VitesseJoueur) : Vector3.Zero;
        }
        #endregion
    }
}