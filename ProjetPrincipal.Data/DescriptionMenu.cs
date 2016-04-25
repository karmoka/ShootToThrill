using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetPrincipal.Data
{
   public class DescriptionMenu
   {
      public Vector2 Position { get; set; }
      public Vector2 DimensionBouton { get; set; }

      public string NomFonts { get; set; }
      public string NomImageBouton { get; set; }
      public string[] TexteBouton { get; set; }
      public EventHandler[] FonctionBouton { get; set; }
   }
}
