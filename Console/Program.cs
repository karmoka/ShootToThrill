using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
   class Program
   {
      static void Main(string[] args)
      {
          DescriptionObjetDeBaseAniméÉclairé ComposanteGraphique = new DescriptionObjetDeBaseAniméÉclairé()
          {
             NomModèle = "Boss1",
              Échelle = 0.75f,
              NomEffetAffichage = "Spotlight",
              NomTextureModèle = "tile_12",
              Rotation = new Vector3(-MathHelper.PiOver2, 0, 0)
          };
          DescriptionObjetPhysique ComposantePhysique = new DescriptionObjetPhysique()
          {
              EstImmuable = false,
              MasseInverse = 0.1f,
              Position = Vector3.Zero,
              Vitesse = Vector3.Zero
          };

          DescriptionEnnemi description = new DescriptionEnnemi()
          {
              DescriptionComposanteGraphique = ComposanteGraphique,
              DescriptionComposantePhysique  = ComposantePhysique,
              VieMax = 1000,
              Domage = 15,
              NomEnnemi = "Boss1"
          };


         XmlWriterSettings settings = new XmlWriterSettings();
         settings.Indent = true;

         using (XmlWriter writer = XmlWriter.Create("boss11.xml", settings))
         {
             IntermediateSerializer.Serialize<DescriptionAvatar>(writer, description, null);
         }
      }
   }
}
