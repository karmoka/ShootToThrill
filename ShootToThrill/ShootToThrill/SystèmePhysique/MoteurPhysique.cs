// Auteur :       Raphael Croteau
// Fichier :      MoteurPhysique.cs
// Description :  Gère une liste d'objetphysique et s'occuper des collisions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    /// <summary>
    /// Gère la physique de toute les entités incluants les collisions et la gestion de collision
    /// </summary>
   class MoteurPhysique : GameComponent, IPausable
   {
      public List<IPhysique> ListePhysique { get; private set; }

      float IntervalMAJ { get; set; }
      float TempsDepuisMAJ { get; set; }

      bool EnPause { get; set; }

      public MoteurPhysique(Game game, float intervalMAJ)
         : base(game)
      {
         IntervalMAJ = intervalMAJ;
         ListePhysique = new List<IPhysique>();
      }

      public override void Initialize()
      {
         EnPause = false;

         TempsDepuisMAJ = 0;

         base.Initialize();
      }

      public void Pause()
      {
         EnPause = true;
      }
      public void Résumer()
      {
         EnPause = false;
      }

      public void AjouterObjet(IPhysique objet)
      {
         if (objet is Ennemi && ListePhysique.Exists(x => x is Joueur))
         {
             ListePhysique.Insert(ListePhysique.FindLastIndex(x => x is Joueur) + 1, objet);
         }
         else
         {
             ListePhysique.Add(objet);
         }
      }

      public void EnleverObjet(IPhysique objet)
      {
          ListePhysique.Remove(objet);
      }

      public override void Update(GameTime gameTime)
      {
         TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;

         if (!EnPause && TempsDepuisMAJ >= IntervalMAJ)
         {
            Simuler();
            DétecterGérerCollision();
            //GérerCollision();

            TempsDepuisMAJ = 0;
         }

         base.Update(gameTime);
      }

      private void Simuler()
      {
         for (int i = 0; i < GrosseurListe; ++i)
         {
            ListePhysique[i].GetObjetPhysique().Intégrer(IntervalMAJ);
         }
      }

      private void DétecterGérerCollision()
      {
         for (int i = 0; i < GrosseurListe; ++i)
         {
            for (int j = i + 1; j < GrosseurListe; ++j)
            {
               bool intersection = ListePhysique[i].GetCollider().Intersects(ListePhysique[j].GetCollider());

               if (intersection)
               {
                  InformationIntersection infoColli = new InformationIntersection(ListePhysique[i].GetObjetPhysique(), ListePhysique[j].GetObjetPhysique());
                  ListePhysique[i].GetObjetPhysique().EnCollision(ListePhysique[j].GetObjetPhysique(), infoColli);
                  ListePhysique[j].GetObjetPhysique().EnCollision(ListePhysique[i].GetObjetPhysique(), infoColli);
               }
            }
         }
      }

      public int GrosseurListe
      {
         get { return ListePhysique.Count; }
      }
   }
}
