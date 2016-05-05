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
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public static class CustomMathHelper
   {
      public static Vector3 ProjectionVectorielle(Vector3 u, Vector3 v)
      {
         return Vector3.Dot(u, v) / (v.Length() * v.Length()) * v;
      }

      public static float ValeurAbsolue(float f)
      {
         if (f < 0)
            f *= -1;
         return f;
      }

      public static Vector3 Réfléchir(Vector3 vector, Vector3 normal)
      {
         return vector - 2 * Vector3.Dot(vector, normal) * normal;
      }

       public static float AngleEntreDeuxVecteurs(Vector3 vecteur1, Vector3 vecteur2)
      {
           return (float)(Math.Acos((Vector3.Dot(vecteur1, vecteur2)/(vecteur1.Length() * vecteur2.Length()))));
      }

       public static float AngleDeVecteur2D(Vector2 vecteur)
       {
           return (float)(-Math.Atan2(vecteur.X, vecteur.Y));// + Math.PI);
       }

      public static Vector3 PlancherZero(Vector3 vecteur)
       {
          return new Vector3(PlancherZero(vecteur.X), PlancherZero(vecteur.Y), PlancherZero(vecteur.Z));
       }

      public static float PlancherZero(float f)
      {
         return f > 0 ? f : 0;
      }

      public static Vector3 ValeurAbsolue(Vector3 vecteur)
      {
         return new Vector3(ValeurAbsolue(vecteur.X), ValeurAbsolue(vecteur.Y), ValeurAbsolue(vecteur.Z));
      }

      public static Vector3 DéterminerRotationModeleBlender(Vector2 vecteurRotation)
      {
         return new Vector3(CustomMathHelper.AngleDeVecteur2D(vecteurRotation) + MathHelper.PiOver2, -MathHelper.PiOver2, 0);
      }
   }
}
