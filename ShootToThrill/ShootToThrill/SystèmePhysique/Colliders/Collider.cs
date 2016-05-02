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
   public enum Type_Collider { Sphere, Cube, Droite}

    /// <summary>
    /// Classe abstraire définissante un volume de collision
    /// </summary>
   public abstract class Collider
   {
      public abstract bool Intersects(Collider autre);

      public Vector3 Center { get; set; }

      public Type_Collider Type { get; set; }

      public abstract Vector3 Normale(Vector3 positionAutreObjet);
      public virtual void Initialize()
      {

      }

      public abstract float DistanceBord(Vector3 position);
      public float DistanceImpact { get; set; }

      protected static bool CollisionSphereSphere(SphereCollision sphere1, SphereCollision sphere2)
      {
         float Distance = (sphere1.Center - sphere2.Center).Length();
         return (Distance <= sphere1.Rayon || Distance <= sphere2.Rayon);
      }

      protected static bool CollisionSphereCube(SphereCollision sphere, MCubeCollision cube)
      {
         bool enCollision = false;
         Vector3 normale = cube.Normale(sphere.Center);
         Vector3 distance = sphere.Center - cube.Center;

         if (CustomMathHelper.ValeurAbsolue(Vector3.Dot(distance, normale)) <= sphere.Rayon)
            enCollision = true;

         if (distance.Length() > (cube.DemiDimention.X + cube.DemiDimention.Y + cube.DemiDimention.Z))
            enCollision = false;

         return enCollision;
      }

      protected static bool CollisionCubeCube(MCubeCollision cube1, MCubeCollision cube2)
      {
         return cube1.EnCollisionAvecCube(cube2);
      }
   }
}
