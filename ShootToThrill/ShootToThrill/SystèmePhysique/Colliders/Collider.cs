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

      public abstract float DistanceBord(Vector3 position);
      public float DistanceImpact { get; set; }

   }
}
