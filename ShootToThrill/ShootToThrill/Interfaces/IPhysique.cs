using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public interface IPhysique
   {
      Collider GetCollider();
      ObjetPhysique GetObjetPhysique();
   }
}
