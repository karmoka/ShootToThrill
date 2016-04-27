using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
   public class CubeColoré : PrimitiveDeBaseAnimée, IModele3d
   {
      Caméra CaméraActuelle { get; set; }

      public void SetCaméra(Caméra cam)
      {
         CaméraActuelle = cam;
      }
      public void SetPosition(Vector3 position)
      {
         Position = position;
      }

      const int NB_SOMMETS = 8;

      Color Couleur { get; set; }
      VertexPositionColor[] Sommets1 { get; set; }
      VertexPositionColor[] Sommets2 { get; set; }
      public Vector3 Dimension { get; private set; }
      public Vector3 Position { get; private set; }
      Vector3 Origine { get; set; }

      public Vector3 Ori { get; private set; }
      BasicEffect EffetDeBase { get; set; }
      Vector3[] PositionsSommets { get; set; }
      float Échelle { get; set; }
      Vector3 Rotation { get; set; }

      public CubeColoré(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur, Vector3 dimension, float intervalleMAJ)
         : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
          Échelle = HomothétieInitiale;
          Rotation = rotationInitiale;
          Couleur = couleur;
          Dimension = dimension;
          Origine = Dimension / -2;
          Ori = positionInitiale - Dimension / 2;
          Position = positionInitiale;
      }

      public override void Initialize()
      {
         Sommets1 = new VertexPositionColor[NB_SOMMETS];
         Sommets2 = new VertexPositionColor[NB_SOMMETS];
         PositionsSommets = new Vector3[NB_SOMMETS];
         CalculerMonde();
         base.Initialize();
      }

      protected override void LoadContent()
      {
          EffetDeBase = new BasicEffect(GraphicsDevice);
          EffetDeBase.VertexColorEnabled = true;
         base.LoadContent();
      }

      protected override void InitialiserSommets()
      {
          PositionsSommets[0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
          PositionsSommets[1] = new Vector3(Origine.X, Origine.Y, Origine.Z + Dimension.Z);
          PositionsSommets[2] = new Vector3(Origine.X, Origine.Y + Dimension.Y, Origine.Z);
          PositionsSommets[3] = new Vector3(Origine.X, Origine.Y + Dimension.Y, Origine.Z + Dimension.Z);
          PositionsSommets[4] = new Vector3(Origine.X + Dimension.X, Origine.Y + Dimension.Y, Origine.Z);
          PositionsSommets[5] = new Vector3(Origine.X + Dimension.X, Origine.Y + Dimension.Y, Origine.Z + Dimension.Z);
          PositionsSommets[6] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z);
          PositionsSommets[7] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z + Dimension.Z);

         Color couleur = Couleur;

         Sommets1[0] = new VertexPositionColor(PositionsSommets[0], Color.Gray);
         Sommets1[1] = new VertexPositionColor(PositionsSommets[1], Color.Gray);
         Sommets1[2] = new VertexPositionColor(PositionsSommets[2], Color.White);
         Sommets1[3] = new VertexPositionColor(PositionsSommets[3], Color.White);
         Sommets1[4] = new VertexPositionColor(PositionsSommets[4], Color.White);
         Sommets1[5] = new VertexPositionColor(PositionsSommets[5], Color.White);
         Sommets1[6] = new VertexPositionColor(PositionsSommets[6], Color.Gray);
         Sommets1[7] = new VertexPositionColor(PositionsSommets[7], Color.Gray);

         Sommets2[0] = new VertexPositionColor(PositionsSommets[3], Color.White);
         Sommets2[1] = new VertexPositionColor(PositionsSommets[5], Color.White);
         Sommets2[2] = new VertexPositionColor(PositionsSommets[1], Color.Gray);
         Sommets2[3] = new VertexPositionColor(PositionsSommets[7], Color.Gray);
         Sommets2[4] = new VertexPositionColor(PositionsSommets[0], Color.Gray);
         Sommets2[5] = new VertexPositionColor(PositionsSommets[6], Color.Gray);
         Sommets2[6] = new VertexPositionColor(PositionsSommets[2], Color.White);
         Sommets2[7] = new VertexPositionColor(PositionsSommets[4], Color.White);
      }

      public void InitialiserSommets(Color couleur)
      {
          PositionsSommets[0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
          PositionsSommets[1] = new Vector3(Origine.X, Origine.Y, Origine.Z + Dimension.Z);
          PositionsSommets[2] = new Vector3(Origine.X, Origine.Y + Dimension.Y, Origine.Z);
          PositionsSommets[3] = new Vector3(Origine.X, Origine.Y + Dimension.Y, Origine.Z + Dimension.Z);
          PositionsSommets[4] = new Vector3(Origine.X + Dimension.X, Origine.Y + Dimension.Y, Origine.Z);
          PositionsSommets[5] = new Vector3(Origine.X + Dimension.X, Origine.Y + Dimension.Y, Origine.Z + Dimension.Z);
          PositionsSommets[6] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z);
          PositionsSommets[7] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z + Dimension.Z);

         for (int i = 0; i < NB_SOMMETS; ++i)
         {
             couleur.B += (byte)(couleur.B / 5);
             couleur.G += (byte)(couleur.G / 5);
             couleur.R += (byte)(couleur.R / 5);
            Sommets1[i] = new VertexPositionColor(PositionsSommets[i], couleur);
         }

         int[] tableauImpair = new int[] { 0, 1, 3, 5 };
         int[] tableauPair = new int[] { 2, 4, 6, 7 };

         for (int i = 0; i < tableauImpair.Length; ++i)
         {
             couleur.B += (byte)(couleur.B / 5);
             couleur.G += (byte)(couleur.G / 5);
             couleur.R += (byte)(couleur.R / 5);
             Sommets2[tableauImpair[i]] = new VertexPositionColor(PositionsSommets[tableauImpair[tableauImpair.Length - i - 1]], couleur);
         }

         for (int i = 0; i < tableauPair.Length; ++i)
         {
             couleur.B += (byte)(couleur.B / 5);
             couleur.G += (byte)(couleur.G / 5);
             couleur.R += (byte)(couleur.R / 5);
             Sommets2[tableauPair[i]] = new VertexPositionColor(PositionsSommets[tableauPair[tableauPair.Length - i - 1]], couleur);
         }
      }

      public void CalculerMonde()
      {
          Monde = Matrix.Identity;
          Monde *= Matrix.CreateScale(Échelle);
          Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
          Monde *= Matrix.CreateTranslation(Position);
      }

      public void SetDimension(Vector3 dimension)
      {
          Dimension = dimension;
      }

      public void SetOri(Vector3 ori)
      {
          Ori = ori;
      }

      public override void Draw(GameTime gameTime)
      {
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraActuelle.Vue;
         EffetDeBase.Projection = CaméraActuelle.Projection;

         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();

            RasterizerState JeuRasterizerState = new RasterizerState();
            JeuRasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = JeuRasterizerState;

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets1, 0, NB_SOMMETS - 2);
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets2, 0, NB_SOMMETS - 2);
         }
         base.Draw(gameTime);
      }

      public override Matrix GetMonde()
      {
          return Monde;
      }
   }
}