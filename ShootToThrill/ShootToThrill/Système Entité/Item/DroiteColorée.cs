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
    public class DroiteColorée : PrimitiveDeBaseAnimée2
    {
        const int NB_CROISEMENT = 5;
        public Caméra CaméraActuelle { get; set; }

        public void ChangerCaméra(Caméra cam)
        {
            CaméraActuelle = cam;
        }

        Vector3[] PositionsPoints { get; set; }

        Vector3 Point { get; set; }
        Vector3 Direction { get; set; }
        float Longueur { get; set; }
        Color Couleur { get; set; }
        VertexPositionColor[] Points { get; set; }

        BasicEffect EffetDeBase { get; set; }
        private VertexBuffer vertexBuffer { get; set; }

        bool PointsDéterminés { get; set; }

        public DroiteColorée(Game game, VertexPositionColor[] points, Vector3 point)
            : base(game, 1f, Vector3.Zero, point, 0.05f)
        {
            Points = points;
            PointsDéterminés = true;
        }

        public DroiteColorée(Game game, Vector3 point, Vector3 direction, Color couleur, float longueur)
            : base(game, 1f, Vector3.Zero, point, 0.05f)
        {
            Point = point;
            Direction = direction;
            Longueur = longueur;
            Couleur = couleur;
            PointsDéterminés = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.VertexColorEnabled = true;

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), Points.Count(), BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(Points);

            base.LoadContent();
        }

        protected override void InitialiserSommets()
        {
            if (!PointsDéterminés)
            {
                PositionsPoints[0] = Vector3.Zero;
                PositionsPoints[1] = Direction * Longueur;

                Points[0] = new VertexPositionColor(PositionsPoints[0], Color.White);
                Points[1] = new VertexPositionColor(PositionsPoints[1], Color.Yellow);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            CalculerMatriceMonde();
            EffetDeBase.Projection = CaméraActuelle.Projection;
            EffetDeBase.View = CaméraActuelle.Vue;
            EffetDeBase.World = Monde;
            EffetDeBase.VertexColorEnabled = true;
            EffetDeBase.LightingEnabled = false;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in EffetDeBase.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Points, 0, Points.Count() - 1);
            }

            base.Draw(gameTime);
        }
    }
}