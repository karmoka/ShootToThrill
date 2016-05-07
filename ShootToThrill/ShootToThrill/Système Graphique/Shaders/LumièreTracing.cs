using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
   public class LumièreTracing : Lumière
   {
      IPositionable ObjetàTracer { get; set; }

      public void Update()
      {
         Position = ObjetàTracer.Position + Vector3.Up;
      }

      public LumièreTracing(Game jeu, Vector3 position, Vector3 couleur, float rayon, IPositionable objetÀTracer)
         :base(jeu,position,couleur,rayon,0,Vector3.Zero,Vector4.Zero)
      {
         Position = position;
         Couleur = couleur;
         Rayon = rayon;
         ObjetàTracer = objetÀTracer;
      }
   }
}
