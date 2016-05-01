// Auteur :       Raphael Croteau
// Fichier :      CubeCollision.cs
// Date :         le 16 mars 2015
// Description :  Cette classe permet de Lire un fichier texte et d'en manipuler 
//                les données.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;



namespace AtelierXNA
{
    /// <summary>
    /// Cube de collision ''tournable''. Utilise le théorème des axes séparateurs
    /// </summary>
   public class CubeCollision : Collider
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


      public CubeCollision(Vector3 centre, Vector3 dimension, Vector3 rotation)
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

         if (autre is CubeCollision)
         {
            intersects = CollisionCubeCube(this, autre as CubeCollision);
         }
         if (autre is SphereCollision)
         {
            intersects = CollisionSphereCube(autre as SphereCollision, this);
         }

         return intersects;
      }

      public bool EnCollisionAvecCube(CubeCollision cube)
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

      static float LongueurCubeProjeté(CubeCollision A, CubeCollision B, Vector3 AxeComparé)
      {
         return ValeurAbsolue(Vector3.Dot(A.DemiDimention.X * A.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(A.DemiDimention.Y * A.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(A.DemiDimention.Z * A.AxeZ, AxeComparé)) +
                ValeurAbsolue(Vector3.Dot(B.DemiDimention.X * B.AxeX, AxeComparé)) + ValeurAbsolue(Vector3.Dot(B.DemiDimention.Y * B.AxeY, AxeComparé)) + ValeurAbsolue(Vector3.Dot(B.DemiDimention.Z * B.AxeZ, AxeComparé));
      }

      public override Vector3 Normale(Vector3 positionAutreObjet)
      {
         Vector3 diff = this.Center - positionAutreObjet;
         diff = ValeurAbsolueVecteur(diff);
         diff -= DemiDimention;
         diff = CustomMathHelper.PlancherZero(diff);

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

      /// <summary>
      /// Fonction pour alléger la lecture du code. Réfère simplement a customMathHelper pour que les fonctions soient uniforment dans le code
      /// </summary>
      /// <param name="f"></param>
      /// <returns></returns>
      static float ValeurAbsolue(float f)
      {
         return CustomMathHelper.ValeurAbsolue(f);
      }
      static Vector3 ValeurAbsolueVecteur(Vector3 v)
      {
         return CustomMathHelper.ValeurAbsolue(v);
      }
   }
}
