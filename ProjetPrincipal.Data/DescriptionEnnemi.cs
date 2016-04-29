using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetPrincipal.Data
{
   public class DescriptionEnnemi : DescriptionModelPhysique
   {
       public float MasseInverse { get; set; }
       public float Rayon { get; set; }
       public string NomEnnemi { get; set; }
       public int VieMax { get; set; }
       public int Domage { get; set; }
   }
}
