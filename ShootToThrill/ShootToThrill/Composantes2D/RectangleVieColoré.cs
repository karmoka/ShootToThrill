using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    public class RectangleVieColoré : PrimitiveDeBaseAnimée, IModele3d
   {
      new Caméra CaméraJeu { get; set; }

      public void SetCaméra(Caméra cam)
      {
          CaméraJeu = cam;
      }

      const int NB_SOMMETS_TOTAL = 14,
                NB_SOMMETS_MIN = 4,
                NB_TRINAGLES_INTÉRIEURS = 2,
                NB_TRIANGLES_EXTÉRIEURS = 12;

      Color[] Couleur { get; set; }
      VertexPositionColor[] Sommets1 { get; set; }
      VertexPositionColor[] Sommets2 { get; set; }
      VertexPositionColor[] Sommets3 { get; set; }
      Vector3 DimensionExtérieure { get; set; }
      Vector3 DimensionInterieure { get; set; }
      Vector3 Origine { get; set; }
      BasicEffect EffetDeBase { get; set; }
      Vector3[] PositionsSommets1 { get; set; }
      Vector3[] PositionsSommets2
      { 
          get
          {
              return new Vector3[NB_SOMMETS_MIN] { PositionsSommets1[11], PositionsSommets1[9], PositionsSommets1[5], PositionsSommets1[7] };
          }
      }
      Vector3[] PositionsSommets3
      {
          get
          {
              return new Vector3[NB_SOMMETS_MIN] { PositionsSommets1[1], PositionsSommets1[11], PositionsSommets1[3], PositionsSommets1[5] };
          }
      }
      Vector3 Position { get; set; }
      float Distance { get; set; }
      float Échelle { get; set; }
      Vector3 Rotation { get; set; }

      public RectangleVieColoré(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color[] tableauCouleur, Vector3 dimensionExtérieure, Vector3 dimensionIntérieure, float intervalleMAJ)
         : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
          Couleur = tableauCouleur;
          DimensionExtérieure = dimensionExtérieure;
          DimensionInterieure = dimensionIntérieure;
          Position = positionInitiale;
          Échelle = homothétieInitiale;
          Rotation = rotationInitiale;
      }

      public override void Initialize()
      {
          Origine = new Vector3(-DimensionExtérieure.X / 2, DimensionExtérieure.Y / 2, DimensionExtérieure.Z / 2);
          Sommets1 = new VertexPositionColor[NB_SOMMETS_TOTAL];
          Sommets2 = new VertexPositionColor[NB_SOMMETS_MIN];
          Sommets3 = new VertexPositionColor[NB_SOMMETS_MIN];
          PositionsSommets1 = new Vector3[NB_SOMMETS_TOTAL];
          Distance = DimensionInterieure.X;
          CalculerMonde();
          base.Initialize();
      }

      protected override void LoadContent()
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         EffetDeBase.VertexColorEnabled = true;
         base.LoadContent();
      }

      public void CalculerMonde()
      {
          Monde = Matrix.Identity;
          Monde *= Matrix.CreateScale(Échelle);
          Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
          Monde *= Matrix.CreateTranslation(Position);
      }

      protected override void InitialiserSommets()
      {
          //    0   1   2   3   4   5   6   7   8   9   10  11  0   1
          //
          //    0   -   -   -   10   -   -   -   8
          //    |   \   .   \   |   \   .   /   |
          //    |   .   1   -   11   -   9  .   |
          //    |   /   |   .   .   .   |   /   |
          //    |   .   3   -   5   -   7   .   |
          //    |   /   .   \   |   \   .   \   |
          //    2   -   -   -   4   -   -    -  6
          //    

          PositionsSommets1[0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
          PositionsSommets1[1] = new Vector3(Origine.X + (DimensionExtérieure.X - DimensionInterieure.X) / 2, Origine.Y - (DimensionExtérieure.Y - DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[2] = new Vector3(Origine.X, Origine.Y - DimensionExtérieure.Y, Origine.Z);
          PositionsSommets1[3] = new Vector3(Origine.X + (DimensionExtérieure.X - DimensionInterieure.X) / 2, Origine.Y - (DimensionExtérieure.Y + DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[4] = new Vector3(Origine.X + DimensionExtérieure.X / 2, Origine.Y - DimensionExtérieure.Y, Origine.Z);
          PositionsSommets1[5] = new Vector3(Origine.X + Distance, Origine.Y - (DimensionExtérieure.Y + DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[6] = new Vector3(Origine.X + DimensionExtérieure.X, Origine.Y - DimensionExtérieure.Y, Origine.Z);
          PositionsSommets1[7] = new Vector3(Origine.X + (DimensionExtérieure.X + DimensionInterieure.X) / 2, Origine.Y - (DimensionExtérieure.Y + DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[8] = new Vector3(Origine.X + DimensionExtérieure.X, Origine.Y, Origine.Z);
          PositionsSommets1[9] = new Vector3(Origine.X + (DimensionExtérieure.X + DimensionInterieure.X) / 2, Origine.Y - (DimensionExtérieure.Y - DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[10] = new Vector3(Origine.X + DimensionExtérieure.X / 2, Origine.Y, Origine.Z);
          PositionsSommets1[11] = new Vector3(Origine.X + Distance, Origine.Y - (DimensionExtérieure.Y - DimensionInterieure.Y) / 2, Origine.Z);
          PositionsSommets1[12] = PositionsSommets1[0];
          PositionsSommets1[13] = PositionsSommets1[1];

          for (int i = 0; i < NB_SOMMETS_TOTAL; ++i)
          {
              Sommets1[i] = new VertexPositionColor(PositionsSommets1[i], Couleur[0]);
          }
          for (int i = 0; i < NB_SOMMETS_MIN; ++i)
          {
              Sommets2[i] = new VertexPositionColor(PositionsSommets2[i], Couleur[1]);
              Sommets3[i] = new VertexPositionColor(PositionsSommets3[i], Couleur[0]);
          }
      }

      public void InitialiserSommets(Vector3 position, float distance)
      {
          SetPosition(position);
          Distance = distance;
          InitialiserSommets();
      }

      public void SetRotation(Vector3 rotation)
      {

      }

      public void SetPosition(Vector3 position)
      {
          Position = position;
      }

      public override void Draw(GameTime gameTime)
      {
          RasterizerState oldRasterizerState = GraphicsDevice.RasterizerState;
          RasterizerState newRasterizerstate = new RasterizerState();
          newRasterizerstate.CullMode = CullMode.None;
          //newRasterizerstate.FillMode = FillMode.WireFrame;
          GraphicsDevice.RasterizerState = newRasterizerstate;
          EffetDeBase.World = GetMonde();
          EffetDeBase.View = CaméraJeu.Vue;
          EffetDeBase.Projection = CaméraJeu.Projection;
          foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
          {
              passeEffet.Apply();
              GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets1, 0, NB_TRIANGLES_EXTÉRIEURS);
              GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets2, 0, NB_TRINAGLES_INTÉRIEURS);
              GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets3, 0, NB_TRINAGLES_INTÉRIEURS);
          }
          base.Draw(gameTime);
          GraphicsDevice.RasterizerState = oldRasterizerState;
      }
      
      public override Matrix GetMonde()
      {
          CalculerMonde();
          return Monde;
      }
   }
}
