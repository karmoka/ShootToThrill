// Auteur :       Raphael Croteau
// Fichier :      SphereCollision.cs
// Description :  Représente un volume de collision sphérique dans l'espace.

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
   public class SphereCollision : Collider
   {
      public float Rayon { get; private set; }

      public SphereCollision(Vector3 centre, float rayon)
      {
         Center = centre;
         Rayon = rayon;
         Type = Type_Collider.Sphere;
      }

      public override bool Intersects(Collider autre)
      {
         bool intersection = false;

         if(autre.Type == Type_Collider.Sphere)
         {
            intersection = CollisionSphereSphere(this, autre as SphereCollision);
         }
         if (autre.Type == Type_Collider.Cube)
         {
            intersection = CollisionSphereCube(this, autre as MCubeCollision);
         }

         return intersection;
      }

      public override Vector3 Normale(Vector3 positionAutreObjet)
      {
         return Vector3.Normalize(this.Center - positionAutreObjet);
      }
      public override float DistanceBord(Vector3 position)
      {
         return Rayon;
      }


   }
}
