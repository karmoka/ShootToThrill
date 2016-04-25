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
using ProjetPrincipal;

namespace AtelierXNA
{
   class Program
   {
      static void Main(string[] args)
      {
         DescriptionOptions description = new DescriptionOptions()
         {
            IntervalMAJStandard = 1 / 60f,
            WindowWidth = 1000,
            WindowHeight = 700,

            CameraHeightOffset = 5,
            CameraDistanceOffset = 5,

            ViePersonnageMax = 100,
            SensibilitéFriction = 0.01f,
            RatioDistance3dMetre = 3f,

            Gravité = new Vector3(0, -9.8f, 0),
            NomFontMenu = "ArialDebug"
         };


         XmlWriterSettings settings = new XmlWriterSettings();
         settings.Indent = true;

         using (XmlWriter writer = XmlWriter.Create("Options.xml", settings))
         {
             IntermediateSerializer.Serialize<DescriptionOptions>(writer, description, null);
         }
      }
   }
}
