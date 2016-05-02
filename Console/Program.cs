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
              NomModèle = "Avatar0",
              Échelle = 0.75f,
              NomEffetAffichage = "Spotlight",
              NomTextureModèle = "Image1",
              Rotation = new Vector3(MathHelper.PiOver2, 0, 0)
          };
          DescriptionObjetPhysique ComposantePhysique = new DescriptionObjetPhysique()
          {
              EstImmuable = false,
              MasseInverse = 1 / 2f,
              Position = Vector3.Zero,
              Vitesse = Vector3.Zero
          };

          DescriptionAvatar description = new DescriptionAvatar()
          {
              DescriptionComposanteGraphique = ComposanteGraphique,
              DescriptionComposantePhysique  = ComposantePhysique
          };


         XmlWriterSettings settings = new XmlWriterSettings();
         settings.Indent = true;

         using (XmlWriter writer = XmlWriter.Create("Avatar0.xml", settings))
         {
             IntermediateSerializer.Serialize<DescriptionAvatar>(writer, description, null);
         }
      }
   }
}
