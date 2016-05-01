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
        public float Longueur { get; private set; }
        Vector3 VecteurUnitaire { get; set; }
        Vector3 Point { get; set; }
        Ray Droite { get; set; }
        List<ObjetPhysique> DansTrajectoire { get; set; }
        Vector3 PointIntersection { get; set; }
        Game Jeu { get; set; }
        Fusil Arme { get; set; }
        public DroiteCollision(Game game, Vector3 vecteur, Vector3 point, Fusil arme)
        {
            VecteurUnitaire = vecteur;
            VecteurUnitaire.Normalize();
            Point = point;
            Jeu = game;
            Arme = arme;
            Longueur = Arme.Portée;

            DansTrajectoire = new List<ObjetPhysique>();
            PointIntersection = new Vector3();

            Droite = new Ray(Point, VecteurUnitaire);
        }

        public void CoupDeFeu()
        {
            int nbObjetEnCollision;
            MoteurPhysique moteurPhysique = Jeu.Services.GetService(typeof(MoteurPhysique)) as MoteurPhysique;
            foreach (ObjetPhysique objet in moteurPhysique.ListePhysique)
            {
                if ((objet is Ennemi || objet is CubeAdditionnable) && this.Intersects(objet.GetCollider()) && (PointIntersection - Point).Length() < Longueur)
                {
                    DansTrajectoire.Add(objet);
                }
            }

            nbObjetEnCollision = DansTrajectoire.Count();
            List<float> distances = new List<float>();

            foreach (ObjetPhysique objet in DansTrajectoire)
            {
                Collider collider = objet.GetCollider();
                if (this.Intersects(collider))
                {
                    distances.Add(collider.DistanceImpact);
                }
            }

            distances.OrderBy(x => x);
            if (DansTrajectoire.Count > 0)
            {
                Longueur = distances[0];
                ObjetPhysique objetPhysique = null;
                foreach (ObjetPhysique objet in DansTrajectoire)
                {
                    if ((objet is Ennemi || objet is CubeAdditionnable) && this.Intersects(objet.GetCollider()) && (PointIntersection - Point).Length() != 0 && (PointIntersection - Point).Length() < Longueur)
                    {
                        objetPhysique = objet;
                    }
                }

                //PointIntersection = Point + VecteurUnitaire * Longueur;

                if (objetPhysique is Ennemi)
                {
                    (objetPhysique as Ennemi).RetirerVie(Arme.Dommage);
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
            else if (autre.Type == Type_Collider.Cube)
            {
                Vector3 min = (autre as CubeCollision).Center - (autre as CubeCollision).DemiDimention;
                Vector3 max = (autre as CubeCollision).Center + (autre as CubeCollision).DemiDimention;
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