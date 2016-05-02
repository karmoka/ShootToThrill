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

        public DroiteCollision DroiteCollision { get; private set; }


        BasicEffect EffetDeBase { get; set; }
        private VertexBuffer vertexBuffer { get; set; }

        float TempsÉcouléDepuisMAJ { get; set; }
        bool EstDessinée { get; set; }

        string TypeArme { get; set; }

        public DroiteColorée(Game game, Vector3 point, Vector3 direction, int dommage, float portée, string nomArme)
            : base(game, 1f, Vector3.Zero, point, 0.05f)
        {
            Direction = direction;
            Point = point;
            Longueur = portée;
            TypeArme = nomArme;
            DroiteCollision = new DroiteCollision(game, Direction, Point, Longueur, dommage);
        }

        public override void Initialize()
        {
            PositionsPoints = new Vector3[2];
            Points = new VertexPositionColor[2];

            if (TypeArme == "Teslagun")
            {
                PositionsPoints = new Vector3[NB_CROISEMENT + 1];
                Points = new VertexPositionColor[NB_CROISEMENT + 1];
            }

            Couleur = Color.Gray;
            TempsÉcouléDepuisMAJ = 0;
            EstDessinée = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.VertexColorEnabled = true;

            int nbVertex = 2;
            if (TypeArme == "Teslagun")
            {
                nbVertex = NB_CROISEMENT + 1;
            }

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), nbVertex, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(Points);

            base.LoadContent();
        }

        protected override void InitialiserSommets()
        {
            Longueur = DroiteCollision.Longueur;

            PositionsPoints[0] = Vector3.Zero;
            PositionsPoints[1] = Direction * Longueur;

            Points[0] = new VertexPositionColor(PositionsPoints[0], Color.White);
            Points[1] = new VertexPositionColor(PositionsPoints[1], Color.Yellow);

            Random générateurAléatoire = new Random();

            if (TypeArme == "Teslagun")
            {
                Points[0] = new VertexPositionColor(PositionsPoints[0], Color.LightBlue);
                for (int i = 1; i <= NB_CROISEMENT; ++i)
                {
                    PositionsPoints[i] = PointAléatoire(générateurAléatoire, 10, Point + Direction * i * Longueur / (NB_CROISEMENT)) - Point;
                    Points[i] = new VertexPositionColor(PositionsPoints[i], Color.LightBlue);
                }
            }
        }

        Vector3 PointAléatoire(Random générateurAléatoire, int rayon, Vector3 point)
        {
            Vector3 vecteurAléatoire = new Vector3(générateurAléatoire.Next(0,20), générateurAléatoire.Next(0,20),générateurAléatoire.Next(0,20));
            float rayonAléatoire = générateurAléatoire.Next(0, rayon) / 10.0f;

            Vector3 directionRayon = Vector3.Cross(Direction, vecteurAléatoire);
            directionRayon.Normalize();
            directionRayon *= rayonAléatoire;

            return directionRayon + point;
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
                int nbPrimitives = 1;
                if (TypeArme == "Teslagun")
                {
                    nbPrimitives = NB_CROISEMENT;
                }
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Points, 0, nbPrimitives);
            }

            EstDessinée = true;

            base.Draw(gameTime);
        }
    }
}