using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    /// <summary>
    /// Donne un id unique à toutes les composantes. Garde en mémoire une références de chacune d'entre elles
    /// </summary>
   class EntitySystem : GameComponent
   {
      int NombreCourant { get; set; }
      List<Entité> ListeEntité { get; set; }

      public EntitySystem(Game game)
         :base(game)
      {
         ListeEntité = new List<Entité>();
         NombreCourant = 0;
      }

      public int ObtenirId(Entité e)
      {
         ListeEntité.Add(e);
         return NombreCourant++;
      }

      public Entité ObtenirEntité(int id)
      {
         return ListeEntité.Find(x => x.UniqueId == id);
      }

   }
}
