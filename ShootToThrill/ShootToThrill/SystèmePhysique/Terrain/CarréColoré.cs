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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CarréColoré : PrimitiveDeBaseAnimée, IModele3d
   {
      new Caméra CaméraJeu { get; set; }

      public void SetCaméra(Caméra cam)
      {
          CaméraJeu = cam;
      }
       public void SetPosition(Vector3 position)
      {

      }

      const int NB_SOMMETS = 4;
      const int NB_TRIANGLES = 2;
      const int CHANGEMENT_BLEU = 100;
      const int CHANGEMENT_ROUGE = 50;
      const int CHANGEMENT_VERT = 150;

      public Color Couleur { get; private set; }
      VertexPositionColor[] Sommets { get; set; }
      Vector3 Dimension { get; set; }
      Vector3 Origine { get; set; }
      BasicEffect EffetDeBase { get; set; }
      Vector3[] PositionsSommets { get; set; }
      public Vector3 Position { get; private set; }
      float Échelle { get; set; }
      Vector3 Rotation { get; set; }

      public CarréColoré(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur, Vector3 dimension, float intervalleMAJ)
         : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Couleur = couleur;
         Dimension = dimension;
         Position = positionInitiale;
         Échelle = homothétieInitiale;
         Rotation = rotationInitiale;
      }

      public override void Initialize()
      {
         Origine = new Vector3(-Dimension.X / 2, 0, Dimension.Z / 2);
         Sommets = new VertexPositionColor[NB_SOMMETS];
         PositionsSommets = new Vector3[NB_SOMMETS];
         CalculerMonde();
         base.Initialize();
      }

      protected override void LoadContent()
      {
         //EffetDeBase = new BasicEffect(GraphicsDevice);
         //EffetDeBase.VertexColorEnabled = true;
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
         PositionsSommets[0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
         PositionsSommets[1] = new Vector3(Origine.X, Origine.Y, Origine.Z - Dimension.Z);
         PositionsSommets[2] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z);
         PositionsSommets[3] = new Vector3(Origine.X + Dimension.X, Origine.Y, Origine.Z - Dimension.Z);

         for (int i = 0; i < NB_SOMMETS; ++i)
         {
          Sommets[i] = new VertexPositionColor(PositionsSommets[i], Couleur);
         }
      }

      public void InitialiserSommets(Color couleur)
      {
         Couleur = couleur;
         InitialiserSommets();
      }

      public override void Draw(GameTime gameTime)
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NB_TRIANGLES);
         }
         base.Draw(gameTime);
      }
      
      public override Matrix GetMonde()
      {
          return Monde;
      }
   }
}
