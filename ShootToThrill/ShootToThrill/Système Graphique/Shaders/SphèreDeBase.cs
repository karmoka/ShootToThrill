using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class SphèreDeBase : Sphère
   {
      BasicEffect EffetDeBase { get; set; }
      BlendState GestionAlpha { get; set; }

      public SphèreDeBase(Game jeu, Vector3 positionInitiale, float rayon, Vector2 charpente, string nomTexture,  float intervalleMAJ)
         : base(jeu, positionInitiale,rayon, charpente, nomTexture, intervalleMAJ)
      { }

      protected override void LoadContent()
      {
         base.LoadContent();
         EffetDeBase = new BasicEffect(GraphicsDevice);
         EffetDeBase.TextureEnabled = true;
         EffetDeBase.Texture = TextureSphère;
         GestionAlpha = BlendState.AlphaBlend;
      }

      public override void Draw(GameTime gameTime)
      {
         BlendState oldBlendState = GraphicsDevice.BlendState;
         GraphicsDevice.BlendState = GestionAlpha;
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Sommets, 0, NbTriangles);
         }
         base.Draw(gameTime);
         GraphicsDevice.BlendState = oldBlendState;
      }
   }
}
