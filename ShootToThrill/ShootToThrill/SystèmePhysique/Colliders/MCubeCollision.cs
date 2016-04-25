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
    /// <summary>
    /// Cube de collision ''tournable''. Utilise le théorème des axes séparateurs
    /// </summary>
   public class MCubeCollision : Collider
   {
      public Vector3 DemiDimention { get; set; }
      Vector3 rotation_;

      public Vector3 AxeX { get; protected set; }
      public Vector3 AxeY { get; protected set; }
      public Vector3 AxeZ { get; protected set; }

      public Vector3 Rotation
      {
         get { return rotation_; }
         set 
         {
            rotation_ = value;
            AxeX = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
            AxeY = Vector3.Transform(Vector3.UnitY, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
            AxeZ = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
         }
      }


      public MCubeCollision(Vector3 centre, Vector3 dimension, Vector3 rotation)
         : base()
      {
         Rotation = rotation;

         AxeX = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
         AxeY = Vector3.Transform(Vector3.UnitY, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
         AxeZ = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));

         Center = centre;
         DemiDimention = dimension / 2;

         Type = Type_Collider.Cube;
      }

      public void TournerCube(Vector3 rotation)
      {
         Rotation += rotation;
         AxeX = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
         AxeY = Vector3.Transform(Vector3.UnitY, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));
         AxeZ = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z));

         AxeX = Vector3.Normalize(AxeX);
         AxeY = Vector3.Normalize(AxeY);
         AxeZ = Vector3.Normalize(AxeZ);
      }

      public override bool Intersects(Collider autre)
      {
         bool intersects = false;

         if(autre is MCubeCollision)
         {
            intersects = EnCollisionAvecCube(autre as MCubeCollision);
         }
         if (autre is SphereCollision)
         {
            intersects = EnCollisionAvecSphere(autre as SphereCollision);
         }

         return intersects;
      }

      public bool EnCollisionAvecCube(MCubeCollision cube)
      {
         bool enCollision = true;
         Vector3 Distance = this.Center - cube.Center;

         //case 1
         Vector3 AxeComparé = this.AxeX;
         if (ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > this.DemiDimention.X + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.X * cube.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Y * cube.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Z * cube.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = this.AxeY;
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > this.DemiDimention.Y + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.X * cube.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Y * cube.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Z * cube.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = this.AxeZ;
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > this.DemiDimention.Z + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.X * cube.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Y * cube.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(cube.DemiDimention.Z * cube.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = cube.AxeX;
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > cube.DemiDimention.X + ValeurAbsolue(Vector3.Dot(this.DemiDimention.X * this.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Y * this.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Z * this.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = cube.AxeY;
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > cube.DemiDimention.Y + ValeurAbsolue(Vector3.Dot(this.DemiDimention.X * this.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Y * this.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Z * this.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = cube.AxeZ;
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > cube.DemiDimention.Z + ValeurAbsolue(Vector3.Dot(this.DemiDimention.X * this.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Y * this.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(this.DemiDimention.Z * this.AxeZ, AxeComparé)))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeX, cube.AxeX);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeX, cube.AxeY);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeX, cube.AxeZ);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeY, cube.AxeX);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeY, cube.AxeY);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeY, cube.AxeZ);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeZ, cube.AxeX);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeZ, cube.AxeY);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         AxeComparé = Vector3.Cross(this.AxeZ, cube.AxeZ);
         if (enCollision && ValeurAbsolue(Vector3.Dot(Distance, AxeComparé)) > LongueurCubeProjeté(this, cube, AxeComparé))
            enCollision = false;

         return enCollision;
      }

      static float LongueurCubeProjeté(MCubeCollision A, MCubeCollision B, Vector3 AxeComparé)
      {
         return ValeurAbsolue(Vector3.Dot(A.DemiDimention.X * A.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(A.DemiDimention.Y * A.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(A.DemiDimention.Z * A.AxeZ, AxeComparé)) +
                ValeurAbsolue(Vector3.Dot(B.DemiDimention.X * B.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(B.DemiDimention.Y * B.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(B.DemiDimention.Z * B.AxeZ, AxeComparé));
      }

      public bool EnCollisionAvecSphere(SphereCollision sphere)
      {
         bool enCollision = false;
         Vector3 distance = this.Center - sphere.Center;
         if (this.DemiDimention.X <= sphere.Rayon)
            enCollision = true;
         if (this.DemiDimention.Y <= sphere.Rayon)
            enCollision = true;
         if (this.DemiDimention.Z <= sphere.Rayon)
            enCollision = true;

         return enCollision;
      }

      public override Vector3 Normale(Vector3 positionAutreObjet)
      {
         Vector3 diff = this.Center - positionAutreObjet;
         diff = ValeurAbsolueVecteur(diff);
         diff -= DemiDimention;
         diff = MinimumZero(diff);

         float x = Vector3.Dot(diff, AxeX);
         float y = Vector3.Dot(diff, AxeY);
         float z = Vector3.Dot(diff, AxeZ);

         Vector3 axe = x > y ? (x > z ? AxeX : AxeZ) : (y > z ? AxeY : AxeZ);

         return axe;
      }

      public override float DistanceBord(Vector3 position)
      {
         return DemiDimention.Y;
      }

      static float ValeurAbsolue(float f)
      {
         if (f < 0)
            f *= -1;
         return f;
      }
      static Vector3 ValeurAbsolueVecteur(Vector3 v)
      {
         return new Vector3(ValeurAbsolue(v.X),ValeurAbsolue(v.Y),ValeurAbsolue(v.Z));
      }
      Vector3 MinimumZero(Vector3 t)
      {
         float x= t.X;
         float y = t.Y;
         float z=t.Z;
         if (t.X < 0)
            x = 0;
         if (t.Y < 0)
            y = 0;
         if (t.Z < 0)
            z = 0;
         return new Vector3(x, y, z);
      }
   }
}
