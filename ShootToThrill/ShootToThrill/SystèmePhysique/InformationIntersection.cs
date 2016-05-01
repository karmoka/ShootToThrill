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
      public ObjetPhysique ObjetA { get; set; }
      public ObjetPhysique ObjetB { get; set; }

      public Collider colliderA { get; set; }
      public Collider colliderB { get; set; }

      public Vector3 DistanceIntersection { get; private set; }
      public float DistanceMinimal { get; private set; }
      public float DistancePénétration { get; private set; }

      public InformationIntersection(ObjetPhysique objet1, ObjetPhysique objet2)
      {
         ObjetA = objet1;
         ObjetB = objet2;

         colliderA = ObjetA.GetCollider();
         colliderB = ObjetB.GetCollider();

         DistanceIntersection = objet1.GetCollider().Center - objet2.GetCollider().Center;

         DistanceMinimal = colliderA.DistanceBord(colliderB.Center) + colliderB.DistanceBord(colliderA.Center);

         Vector3 normale = colliderB.Normale(colliderA.Center);
         float distance = Vector3.Dot(DistanceIntersection, normale);
         DistancePénétration = CustomMathHelper.ValeurAbsolue(distance - DistanceMinimal);
      }

   }
}
