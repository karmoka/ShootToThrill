using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;


namespace AtelierXNA
{
    /// <summary>
    /// Décrit une entité dont la position peut etre connu
    /// </summary>
   public interface IPositionable
   {
      Vector3 Position { get; }
      void SetPosition(Vector3 nouvellePosition);
   }
}
