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
    public class Pente : ObjetPhysique, IModele3d
    {
        CubeCollision cubeCollision { get; set; }
        public PrimitivePente pente { get; set; } 

        public Pente(Game game, Vector3 position, Vector3 dimension)
            : base(game, position, Vector3.Zero, 0f)
        {

            //cubeCollision = new MCubeCollision(position, dimension, rotation);
            pente = new PrimitivePente(game, 1, Vector3.Zero, position, Color.White, dimension, 1 / 60f);
        }
        public override void Initialize()
        {
            EstImmuable = true;
            pente.Initialize();
            base.Initialize();
        }

        public override Collider GetCollider()
        {
            cubeCollision.Center = this.Position;
            return cubeCollision;
        }
        public void SetCaméra(Caméra cam)
        {
            pente.CaméraActuelle = cam;
        }
        public override void Draw(GameTime gameTime)
        {
            pente.Position = this.Position;
            pente.Draw(gameTime);
        }
    }
}
