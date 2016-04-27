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
    public class PrimitivePente : PrimitiveDeBaseAnimée, IModele3d
    {
        public Caméra CaméraActuelle { get; set; }

        const int NB_SOMMETS = 8;

        public void SetCaméra(Caméra cam)
        {
            CaméraActuelle = cam;
        }
        public void SetPosition(Vector3 position)
        {

        }
        public void SetRotation(Vector3 rotation)
        {
            //RotationInitiale = rotation;
        }

        Color Couleur { get; set; }
        VertexPositionColor[] SommetsPremièreStrip { get; set; }
        VertexPositionColor[] SommetsDeuxièmeStrip { get; set; }
        public Vector3 Dimension { get; set; }
        public Vector3 Position { get; set; }
        Vector3 OrigineObjet { get; set; }
        public Vector3 Charpente { get; set; }

        public Vector3 OriginePosition { get; set; }
        BasicEffect EffetDeBase { get; set; }
        Vector3[] PositionsSommets { get; set; }


        public PrimitivePente(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur, Vector3 dimension, float intervalleMAJ)
            : base(game, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Couleur = couleur;
            Dimension = dimension;
            OrigineObjet = new Vector3(-Dimension.X / 2, -Dimension.Y / 2, -Dimension.Z / 2);
            OriginePosition = positionInitiale - Dimension / 2;
            Charpente = new Vector3(1, 1, 1);
            Position = positionInitiale;
        }

        public override void Initialize()
        {
            SommetsPremièreStrip = new VertexPositionColor[NB_SOMMETS];
            SommetsDeuxièmeStrip = new VertexPositionColor[NB_SOMMETS];
            PositionsSommets = new Vector3[NB_SOMMETS];

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
            PositionsSommets[0] = new Vector3(OrigineObjet.X, OrigineObjet.Y, OrigineObjet.Z);
            PositionsSommets[1] = new Vector3(OrigineObjet.X, OrigineObjet.Y, OrigineObjet.Z + Dimension.Z);
            PositionsSommets[2] = new Vector3(OrigineObjet.X, OrigineObjet.Y + Dimension.Y, OrigineObjet.Z);
            PositionsSommets[3] = new Vector3(OrigineObjet.X, OrigineObjet.Y + Dimension.Y, OrigineObjet.Z + Dimension.Z / 4);
            PositionsSommets[4] = new Vector3(OrigineObjet.X + Dimension.X / 4, OrigineObjet.Y + Dimension.Y, OrigineObjet.Z);
            PositionsSommets[5] = new Vector3(OrigineObjet.X + Dimension.X / 4, OrigineObjet.Y + Dimension.Y, OrigineObjet.Z + Dimension.Z / 4);
            PositionsSommets[6] = new Vector3(OrigineObjet.X + Dimension.X, OrigineObjet.Y, OrigineObjet.Z);
            PositionsSommets[7] = new Vector3(OrigineObjet.X + Dimension.X, OrigineObjet.Y, OrigineObjet.Z + Dimension.Z);

            Color couleur = Couleur;

            for (int i = 0; i < NB_SOMMETS; ++i)
            {
                couleur.B += (byte)(couleur.B / 5);
                couleur.G += (byte)(couleur.G / 5);
                couleur.R += (byte)(couleur.R / 5);
                SommetsPremièreStrip[i] = new VertexPositionColor(PositionsSommets[i], couleur);
            }

            int[] tableauImpair = new int[] { 0, 1, 3, 5 };
            int[] tableauPair = new int[] { 2, 4, 6, 7 };

            for (int i = 0; i < tableauImpair.Length; ++i)
            {
                couleur.B += (byte)(couleur.B / 5);
                couleur.G += (byte)(couleur.G / 5);
                couleur.R += (byte)(couleur.R / 5);
                SommetsDeuxièmeStrip[tableauImpair[i]] = new VertexPositionColor(PositionsSommets[tableauImpair[tableauImpair.Length - i - 1]], couleur);
            }

            for (int i = 0; i < tableauPair.Length; ++i)
            {
                couleur.B += (byte)(couleur.B / 5);
                couleur.G += (byte)(couleur.G / 5);
                couleur.R += (byte)(couleur.R / 5);
                SommetsDeuxièmeStrip[tableauPair[i]] = new VertexPositionColor(PositionsSommets[tableauPair[tableauPair.Length - i - 1]], couleur);
            }
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

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsPremièreStrip, 0, NB_SOMMETS - 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsDeuxièmeStrip, 0, NB_SOMMETS - 2);
            }
            base.Draw(gameTime);
        }
    }
}
