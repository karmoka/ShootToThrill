using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetPrincipal.Data
{
   public class DescriptionAvatar
   {
      public int VieMax { get; set; }

      public DescriptionObjetDeBaseAniméÉclairé DescriptionComposanteGraphique { get; set; }
      public DescriptionObjetPhysique DescriptionComposantePhysique { get; set; }
   }
}
