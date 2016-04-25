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

        public DroiteCollision DroiteCollision { get; private set; }

        Fusil Arme { get; set; }

        BasicEffect EffetDeBase { get; set; }
        private VertexBuffer vertexBuffer { get; set; }

        float TempsÉcouléDepuisMAJ { get; set; }
        bool EstDessinée { get; set; }

        public DroiteColorée(Game game, Fusil arme)
            : base(game, 1f, Vector3.Zero, arme.Position, 0.05f)
        {
            Direction = new Vector3(arme.Direction.X, 0, -arme.Direction.Y);
            Arme = arme;
            Point = arme.Position;
            Longueur = arme.Portée;
            DroiteCollision = new DroiteCollision(game, Direction, Point, arme);
        }

        public override void Initialize()
        {
            PositionsPoints = new Vector3[2];
            Points = new VertexPositionColor[2];
            Couleur = Color.Gray;
            TempsÉcouléDepuisMAJ = 0;
            EstDessinée = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.VertexColorEnabled = true;

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(Points);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (this.vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            EffetDeBase.Dispose();
        }

        protected override void InitialiserSommets()
        {
            Longueur = DroiteCollision.Longueur;

            PositionsPoints[0] = Vector3.Zero;
            PositionsPoints[1] = Direction * Longueur;

            Points[0] = new VertexPositionColor(PositionsPoints[0], Couleur);
            Points[1] = new VertexPositionColor(PositionsPoints[1], Couleur);
        }


        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                EffectuerMAJ();
                TempsÉcouléDepuisMAJ = 0;
            }

            base.Update(gameTime);
        }

        public void EffectuerMAJ()
        {
            if (EstDessinée)
            {
                Effacer();
            }
        }

        public void Effacer()
        {
            Arme.EffacerDroite();
            EstDessinée = false;
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
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Points, 0, 1);
            }

            EstDessinée = true;

            base.Draw(gameTime);
        }
    }
}