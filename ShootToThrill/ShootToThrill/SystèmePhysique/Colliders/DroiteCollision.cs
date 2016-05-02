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


namespace AtelierXNA
{
    public class DroiteCollision : Collider
    {
        public float Longueur { get; set; }

        Vector3 VecteurUnitaire { get; set; }
        Vector3 Point { get; set; }
        Ray Droite { get; set; }

        List<ObjetPhysique> DansTrajectoire { get; set; }
        Vector3 PointIntersection { get; set; }

        Game Jeu { get; set; }

        int Dommage { get; set; }


        public DroiteCollision(Game game, Vector3 vecteur, Vector3 point, float portée, int dommage)
        {
            VecteurUnitaire = vecteur;
            VecteurUnitaire.Normalize();
            Point = point;
            Jeu = game;
            Longueur = portée;
            Dommage = dommage;

            DansTrajectoire = new List<ObjetPhysique>();
            PointIntersection = new Vector3();

            Droite = new Ray(Point, VecteurUnitaire);
        }

        public void CoupDeFeu()
        {
            int nbObjetEnCollision;
            List<float> distances = new List<float>();

            foreach (ObjetPhysique objet in (Jeu.Services.GetService(typeof(MoteurPhysique)) as MoteurPhysique).ListePhysique)
            {
                if (this.Intersects(objet.GetCollider()) && !(objet is Joueur) && (PointIntersection - Point).Length() < Longueur)
                {
                    DansTrajectoire.Add(objet);
                    distances.Add((PointIntersection - Point).Length());
                }
            }

            nbObjetEnCollision = DansTrajectoire.Count();

            distances.OrderBy(x => x);

            if (DansTrajectoire.Count > 0)
            {
                Longueur = distances[0];
                ObjetPhysique objetPhysique = null;

                foreach(ObjetPhysique objet in DansTrajectoire)
                {
                    if(this.Intersects(objet.GetCollider()) && (PointIntersection - Point).Length() <= Longueur)
                    {
                        objetPhysique = objet;
                    }
                }

                PointIntersection = Point + VecteurUnitaire * Longueur;


                if (objetPhysique is Ennemi)
                {
                    (objetPhysique as Ennemi).RetirerVie(Dommage);
                }
            }
        }

        public override bool Intersects(Collider autre)
        {
            bool intersection = false;

            if (autre.Type == Type_Collider.Sphere)
            {
                BoundingSphere sphère = new BoundingSphere(autre.Center, (autre as SphereCollision).Rayon);
                intersection = Droite.Intersects(sphère) != null;

                if (intersection)
                {
                    autre.DistanceImpact = Droite.Intersects(sphère).Value;
                    PointIntersection = Point + VecteurUnitaire * autre.DistanceImpact;
                }
            }

            if (autre.Type == Type_Collider.Cube)
            {
                Vector3 min = (autre as MCubeCollision).Center - (autre as MCubeCollision).DemiDimention;
                Vector3 max = (autre as MCubeCollision).Center + (autre as MCubeCollision).DemiDimention;
                BoundingBox boîte = new BoundingBox(min, max);
                intersection = Droite.Intersects(boîte) != null;

                if (intersection)
                {
                    autre.DistanceImpact = Droite.Intersects(boîte).Value;
                    PointIntersection = Point + VecteurUnitaire * autre.DistanceImpact;
                }
            }

            return intersection;
        }

        public override Vector3 Normale(Vector3 positionAutreObjet)
        {
            return Vector3.Normalize(this.Center - positionAutreObjet);
        }
        public override float DistanceBord(Vector3 position)
        {
            return Longueur;
        }
    }
}