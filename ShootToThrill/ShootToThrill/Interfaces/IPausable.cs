using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    /// <summary>
    /// Décrit une entité qui peut être mise en pause
    /// </summary>
   public interface IPausable
   {
      void Pause();
      void Résumer();
   }
}
