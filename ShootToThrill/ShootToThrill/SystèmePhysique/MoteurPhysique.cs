using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace AtelierXNA
{
    /// <summary>
    /// Gère la physique de toute les entités incluants les collisions et la gestion de collision
    /// </summary>
   class MoteurPhysique : GameComponent, IPausable
   {
      public List<ObjetPhysique> ListePhysique { get; private set; }
      List<InformationIntersection> InformationSurCollision { get; set; }

      float IntervalMAJ { get; set; }
      float TempsDepuisMAJ { get; set; }

      bool EnPause { get; set; }

      public MoteurPhysique(Game game, float intervalMAJ)
         : base(game)
      {
         IntervalMAJ = intervalMAJ;
         ListePhysique = new List<ObjetPhysique>();
      }

      public override void Initialize()
      {
         EnPause = false;

         InformationSurCollision = new List<InformationIntersection>();
         TempsDepuisMAJ = 0;
         foreach (ObjetPhysique i in ListePhysique)
         {
            i.Initialize();
         }

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

      public void AjouterObjet(ObjetPhysique objet)
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

       public void EnleverObjet(ObjetPhysique objet)
      {
          ListePhysique.Remove(objet);
      }

      public override void Update(GameTime gameTime)
      {
         TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;

         if (!EnPause && TempsDepuisMAJ >= IntervalMAJ)
         {
            Simuler();
            DétecterCollision();
            GérerCollision();

            TempsDepuisMAJ = 0;
         }

         base.Update(gameTime);
      }

      private void Simuler()
      {
         for (int i = 0; i < GrosseurListe; ++i)
         {
            ListePhysique[i].Intégrer(IntervalMAJ);
         }
      }

      private void DétecterCollision()
      {
         for (int i = 0; i < GrosseurListe; ++i)
         {
            for (int j = i + 1; j < GrosseurListe; ++j)
            {
               bool intersection = ListePhysique[i].GetCollider().Intersects(ListePhysique[j].GetCollider());

               if (intersection)
               {
                  InformationSurCollision.Add(new InformationIntersection(ListePhysique[i], ListePhysique[j]));
               }
            }
         }
      }

      private void GérerCollision()
      {
         foreach(InformationIntersection infoColli in InformationSurCollision)
         {
            ObjetPhysique A = infoColli.ObjetA;
            ObjetPhysique B = infoColli.ObjetB;

            A.EnCollision(B, infoColli);
            B.EnCollision(A, infoColli);

            if (A.EstTangible && B.EstTangible)
            {
               //Vector3 norm = B.GetCollider().Normale(A.Position);
               ////La norme est corrigé pour gérer la collision des deux bords de l'objet
               //if (Vector3.Dot((B.Position - A.Position), norm) > 0)
               //   norm = -norm;
               //A.SetVitesse(CustomMathHelper.Réfléchir(A.Vitesse, norm) * 0.95f);
               //B.SetVitesse(CustomMathHelper.Réfléchir(A.Vitesse, norm) * 0.95f);

               //CorrigerPosition(infoColli.ObjetA, infoColli.ObjetB, infoColli, norm);
            }
         }

         InformationSurCollision.Clear();
      }

      void CorrigerPosition(ObjetPhysique A, ObjetPhysique B, InformationIntersection infoColli, Vector3 normale)
      {
      //    A.SetPosition(A.Position + normale * 0.01f * A.MasseInverse);
      //    B.SetPosition(B.Position - normale * 0.01f * B.MasseInverse);
      }

      public int GrosseurListe
      {
         get { return ListePhysique.Count; }
      }

      public Vector3 GetPositionJoueurPlusProche(Vector3 positionEnnemi)
      {
          Vector3 positionAvatarPlusProche = Vector3.Zero;
          List<float> distance = new List<float>();
          foreach (Joueur j in ListePhysique.Where(x => x is Joueur))
          {
              distance.Add(Vector3.Distance(j.Position, positionEnnemi));
          }
          foreach (Joueur j in ListePhysique.Where(x => x is Joueur))
          {
              if (Vector3.Distance(j.Position, positionEnnemi) == distance.OrderBy(d => d).ElementAt(0))// && e is Avatar_)
              {
                  positionAvatarPlusProche = j.Position;
              }
          }
          return positionAvatarPlusProche;
      }
   }
}
