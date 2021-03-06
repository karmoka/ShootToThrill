﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public abstract class Billboard : PrimitiveDeBaseAnimée, IModele3d
   {
      Caméra CaméraActuelle { get; set; }

      const int NB_TRIANGLES = 2;
      protected Vector3[,] PtsSommets { get; private set; }
      Vector3 Origine { get; set; }
      Vector2 Delta { get; set; }
      protected BasicEffect EffetDeBase { get; private set; }
      protected Vector3 Position { get; set; }


      public Billboard(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, 
                   Vector2 étendue, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Delta = new Vector2(étendue.X, étendue.Y);
         Origine = new Vector3(-Delta.X / 2, -Delta.Y / 2, 0); //pour centrer la primitive au point (0,0,0)
      }

      public override void Initialize()
      {
         NbSommets = NB_TRIANGLES + 2;
         PtsSommets = new Vector3[2, 2];
         CréerTableauPoints();
         CréerTableauSommets();
         base.Initialize();

         CaméraActuelle = CaméraJeu;
      }

      private void CréerTableauPoints()
      {
         PtsSommets[0, 0] = new Vector3(Origine.X, Origine.Y, Origine.Z);
         PtsSommets[1, 0] = new Vector3(Origine.X + Delta.X, Origine.Y, Origine.Z);
         PtsSommets[0, 1] = new Vector3(Origine.X, Origine.Y + Delta.Y, Origine.Z);
         PtsSommets[1, 1] = new Vector3(Origine.X + Delta.X, Origine.Y + Delta.Y, Origine.Z);
      }

      protected abstract void CréerTableauSommets();

      protected override void LoadContent()
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         InitialiserParamètresEffetDeBase();
         base.LoadContent();
      }

      protected abstract void InitialiserParamètresEffetDeBase();

      public override void Draw(GameTime gameTime)
      {
         RasterizerState old = GraphicsDevice.RasterizerState;
         RasterizerState nouveau = new RasterizerState();
         nouveau.CullMode = CullMode.None;
         GraphicsDevice.RasterizerState = nouveau;

         EffetDeBase.World = GetMonde() * Matrix.CreateBillboard(Vector3.Zero, CaméraActuelle.Position, Vector3.Up, null);
         EffetDeBase.View = CaméraActuelle.Vue;
         EffetDeBase.Projection = CaméraActuelle.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            DessinerTriangleStrip();
         }

         GraphicsDevice.RasterizerState = old;


        base.Draw(gameTime);
      }

      protected abstract void DessinerTriangleStrip();

       public void SetCaméra(Caméra nouvelleCaméra)
      {
          CaméraActuelle = nouvelleCaméra;
      }
       public void SetPosition(Vector3 position)
       {
           //this.Position = position;
       }
       public void SetRotation(Vector3 rotation)
       {

       }
   }
}

