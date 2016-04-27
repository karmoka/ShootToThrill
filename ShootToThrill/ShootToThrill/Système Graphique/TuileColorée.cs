using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   class BillboardColoréTracing : Billboard
   {
       IPositionable Tracing { get; set; }
      const int NB_TRIANGLES = 2;
      VertexPositionColor[] Sommets { get; set; }
      Color Couleur { get; set; }
      Matrix matriceSuplémentaire;

      public BillboardColoréTracing(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, 
                          Vector2 étendue, Color couleur, float intervalleMAJ, IPositionable tracing)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, intervalleMAJ)
      {
         Couleur = couleur;
         Tracing = tracing;
      }

      protected override void CréerTableauSommets()
      {
         Sommets = new VertexPositionColor[NbSommets];
      }

      protected override void InitialiserParamètresEffetDeBase()
      {
         EffetDeBase.VertexColorEnabled = true;
      }

      protected override void InitialiserSommets() // Est appelée par base.Initialize()
      {
         int NoSommet = -1;
         for (int j = 0; j < 1; ++j)
         {
            for (int i = 0; i < 2; ++i)
            {
               Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j], Couleur);
               Sommets[++NoSommet] = new VertexPositionColor(PtsSommets[i, j + 1], Couleur);
            }
         }
      }

      public override void Update(GameTime gameTime)
      {
          base.Position = Tracing.Position;
          base.Update(gameTime);
      }

      protected override void DessinerTriangleStrip()
      {
         GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
      }
   }
}

