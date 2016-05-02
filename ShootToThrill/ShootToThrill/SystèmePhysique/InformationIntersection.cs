// Auteur :       Raphael Croteau
// Fichier :      ModelPhysique.cs
// Description :  Objet d'information sur une collision qui peut être utilisé plus tard pour les gérer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class InformationIntersection
   {
      public IPhysique ObjetA { get; set; }
      public IPhysique ObjetB { get; set; }

      public Vector3 PositionA { get; set; }
      public Vector3 PositionB { get; set; }
      public Vector3 Normale { get; private set; }
      public Vector3 DistanceIntersection { get; private set; }

      public InformationIntersection(IPhysique objet1, IPhysique objet2)
      {
         ObjetA = objet1;
         ObjetB = objet2;

         PositionA = ObjetA.GetCollider().Center;
         PositionB = ObjetB.GetCollider().Center;

         DistanceIntersection = objet1.GetCollider().Center - objet2.GetCollider().Center;

         Vector3 norm = objet2.GetCollider().Normale(PositionA);
         //La norme est corrigé pour gérer la collision des deux bords de l'objet
         if (Vector3.Dot((PositionB - PositionA), norm) >= 0)
            norm = -norm;
         Normale = norm;

         float distance = Vector3.Dot(DistanceIntersection, Normale);
      }

   }
}
