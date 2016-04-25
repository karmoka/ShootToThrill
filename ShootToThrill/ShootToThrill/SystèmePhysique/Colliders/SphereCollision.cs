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
            float Distance = (this.Center - autre.Center).Length();
            if (Distance <= this.Rayon || Distance <= (autre as SphereCollision).Rayon)
               intersection = true;
         }
         if (autre.Type == Type_Collider.Cube)
         {
            intersection = EnCollisionAvecCube(autre as MCubeCollision);
         }

         return intersection;
      }
      public bool EnCollisionAvecCube(MCubeCollision cube)
      {
         bool enCollision = false;
         Vector3 normale = cube.Normale(this.Center);
         Vector3 distance = this.Center - cube.Center;

         if (CustomMathHelper.ValeurAbsolue(Vector3.Dot(distance, normale))<= this.Rayon)
            enCollision = true;

         if (distance.Length() > (cube.DemiDimention.X+cube.DemiDimention.Y+cube.DemiDimention.Z))
            enCollision = false ;


         return enCollision;
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
