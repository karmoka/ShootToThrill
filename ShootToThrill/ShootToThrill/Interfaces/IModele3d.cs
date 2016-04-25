using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AtelierXNA
{
    /// <summary>
    /// Décrit une entité qui peut être affiché en 3D selon différentes caméras
    /// </summary>
   public interface IModele3d
   {
      void SetPosition(Vector3 position);
      void SetCaméra(Caméra caméra);
      void Draw(GameTime gameTime);
      void Update(GameTime gameTime);
      void Initialize();
   }
}
