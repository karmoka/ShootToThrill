﻿using System;
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
    public class CubeTexturé : PrimitiveDeBaseAnimée2, IModele3d
    {
        //Caméra CaméraActuelle { get; set; }

        public void SetCaméra(Caméra cam)
        {
            CaméraActuelle = cam;
        }

        protected const int NB_SOMMETS = 8;

        protected VertexPositionNormalTexture[] Sommets1 { get; set; }
        protected VertexPositionNormalTexture[] Sommets2 { get; set; }
        Vector3[] Normales { get; set; }
        public Vector3 Dimension { get; private set; }
        Vector3 Charpente { get; set; }
        public Vector3 Position { get; private set; }
        Vector3 Origine { get; set; }
        protected Texture2D Image { get; set; }
        string NomImage { get; set; }

        public Vector3 Ori { get; private set; }
        BasicEffect EffetDeBase { get; set; }
        Vector3[] PositionsSommets { get; set; }
        Vector2[] Découpage { get; set; }
        float Échelle { get; set; }
        Vector3 Rotation { get; set; }

        protected RessourcesManager<Texture2D> gestionnaireDeTextures;
        BlendState GestionAlpha { get; set; }

        public CubeTexturé(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 dimension, float intervalleMAJ, string nomImage)
            : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Échelle = HomothétieInitiale;
            Rotation = rotationInitiale;
            Dimension = dimension;
            Origine = Dimension / -2;
            Ori = positionInitiale - Dimension / 2;
            Position = positionInitiale;
            NomImage = nomImage;
        }

        public override void Initialize()
        {
            Sommets1 = new VertexPositionNormalTexture[NB_SOMMETS * 2 + 2];
            Sommets2 = new VertexPositionNormalTexture[NB_SOMMETS * 2 + 2];
            Normales = new Vector3[6];
            PositionsSommets = new Vector3[NB_SOMMETS];
            Découpage = new Vector2[NB_SOMMETS];
            CalculerMonde();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EffetDeBase = Game.Services.GetService(typeof(BasicEffect)) as BasicEffect;//new BasicEffect(GraphicsDevice);
            gestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = gestionnaireDeTextures.Find(NomImage);

            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = Image;
            GestionAlpha = BlendState.AlphaBlend;
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

            Découpage[0] = new Vector2(0, 0);
            Découpage[1] = new Vector2(0, 1);
            Découpage[2] = new Vector2(1 / 3.0f, 0);
            Découpage[3] = new Vector2(1 / 3.0f, 1);
            Découpage[4] = new Vector2(2 / 3.0f, 0);
            Découpage[5] = new Vector2(2 / 3.0f, 1);
            Découpage[6] = new Vector2(1, 0);
            Découpage[7] = new Vector2(1, 1);


            Vector3 centre = PositionsSommets[5] / 2;

            for (int i = 0; i < 6; ++i)
            {
                Normales[i] = PositionsSommets[i] - centre;
                Normales[i].Normalize();
            }

            Normales[0] = Vector3.Left;
            Normales[1] = Vector3.Up;
            Normales[2] = Vector3.Right;
            Normales[3] = Vector3.Backward;
            Normales[4] = Vector3.Down;
            Normales[5] = Vector3.Forward;

            Sommets1[0] = new VertexPositionNormalTexture(PositionsSommets[0], Normales[0], Découpage[1]);
            Sommets1[1] = new VertexPositionNormalTexture(PositionsSommets[3], Normales[0], Découpage[2]);
            Sommets1[2] = new VertexPositionNormalTexture(PositionsSommets[1], Normales[0], Découpage[0]);

            Sommets1[3] = new VertexPositionNormalTexture(PositionsSommets[0], Normales[0], Découpage[1]);
            Sommets1[4] = new VertexPositionNormalTexture(PositionsSommets[2], Normales[0], Découpage[3]);
            Sommets1[5] = new VertexPositionNormalTexture(PositionsSommets[3], Normales[0], Découpage[2]);

            Sommets1[6] = new VertexPositionNormalTexture(PositionsSommets[2], Normales[1], Découpage[3]);
            Sommets1[7] = new VertexPositionNormalTexture(PositionsSommets[5], Normales[1], Découpage[4]);
            Sommets1[8] = new VertexPositionNormalTexture(PositionsSommets[3], Normales[1], Découpage[2]);

            Sommets1[9] = new VertexPositionNormalTexture(PositionsSommets[2], Normales[1], Découpage[3]);
            Sommets1[10] = new VertexPositionNormalTexture(PositionsSommets[4], Normales[1], Découpage[5]);
            Sommets1[11] = new VertexPositionNormalTexture(PositionsSommets[5], Normales[1], Découpage[4]);

            Sommets1[12] = new VertexPositionNormalTexture(PositionsSommets[4], Normales[2], Découpage[5]);
            Sommets1[13] = new VertexPositionNormalTexture(PositionsSommets[7], Normales[2], Découpage[6]);
            Sommets1[14] = new VertexPositionNormalTexture(PositionsSommets[5], Normales[2], Découpage[4]);

            Sommets1[15] = new VertexPositionNormalTexture(PositionsSommets[4], Normales[2], Découpage[5]);
            Sommets1[16] = new VertexPositionNormalTexture(PositionsSommets[6], Normales[2], Découpage[7]);
            Sommets1[17] = new VertexPositionNormalTexture(PositionsSommets[7], Normales[2], Découpage[6]);


            Sommets2[0] = new VertexPositionNormalTexture(PositionsSommets[5], Normales[3], Découpage[0]);
            Sommets2[1] = new VertexPositionNormalTexture(PositionsSommets[1], Normales[3], Découpage[3]);
            Sommets2[2] = new VertexPositionNormalTexture(PositionsSommets[3], Normales[3], Découpage[1]);

            Sommets2[3] = new VertexPositionNormalTexture(PositionsSommets[5], Normales[3], Découpage[0]);
            Sommets2[4] = new VertexPositionNormalTexture(PositionsSommets[7], Normales[3], Découpage[2]);
            Sommets2[5] = new VertexPositionNormalTexture(PositionsSommets[1], Normales[3], Découpage[3]);

            Sommets2[6] = new VertexPositionNormalTexture(PositionsSommets[7], Normales[4], Découpage[2]);
            Sommets2[7] = new VertexPositionNormalTexture(PositionsSommets[0], Normales[4], Découpage[5]);
            Sommets2[8] = new VertexPositionNormalTexture(PositionsSommets[1], Normales[4], Découpage[3]);

            Sommets2[9] = new VertexPositionNormalTexture(PositionsSommets[7], Normales[4], Découpage[2]);
            Sommets2[10] = new VertexPositionNormalTexture(PositionsSommets[6], Normales[4], Découpage[4]);
            Sommets2[11] = new VertexPositionNormalTexture(PositionsSommets[0], Normales[4], Découpage[5]);

            Sommets2[12] = new VertexPositionNormalTexture(PositionsSommets[6], Normales[5], Découpage[4]);
            Sommets2[13] = new VertexPositionNormalTexture(PositionsSommets[2], Normales[5], Découpage[7]);
            Sommets2[14] = new VertexPositionNormalTexture(PositionsSommets[0], Normales[5], Découpage[5]);

            Sommets2[15] = new VertexPositionNormalTexture(PositionsSommets[6], Normales[5], Découpage[4]);
            Sommets2[16] = new VertexPositionNormalTexture(PositionsSommets[4], Normales[5], Découpage[6]);
            Sommets2[17] = new VertexPositionNormalTexture(PositionsSommets[2], Normales[5], Découpage[7]);
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

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetOri(Vector3 ori)
        {
            Ori = ori;
        }

        public void SetRotation(Vector3 nouvelleRotation)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraActuelle.Vue;
            EffetDeBase.Projection = CaméraActuelle.Projection;

            BlendState oldBlendState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = GestionAlpha;

            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();

                //GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Sommets1, 0, NB_SOMMETS - 2);
                //GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, Sommets2, 0, NB_SOMMETS - 2);
            }
            base.Draw(gameTime);
            GraphicsDevice.BlendState = oldBlendState;
        }

        public override Matrix GetMonde()
        {
            return Monde;
        }
    }
}