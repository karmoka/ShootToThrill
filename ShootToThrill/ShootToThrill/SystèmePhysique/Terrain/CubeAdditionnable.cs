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
   public class CubeAdditionnable : ObjetPhysique, IModele3d, IPhysique
   {
       
       MCubeCollision CubeCollision { get; set; }
       public CubeColoré CubeColoré { get; private set; }

      public CubeAdditionnable(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur, Vector3 dimension, float intervalleMAJ)
          : base(game, positionInitiale, Vector3.Zero, 0)
      {
          //Position = positionInitiale;
          CubeCollision = new MCubeCollision(positionInitiale, dimension, Vector3.Zero);
          CubeColoré = new CubeColoré(game, homothétieInitiale, Vector3.Zero, positionInitiale, couleur, dimension, intervalleMAJ);
      }

      public override void Initialize()
      {
          EstImmuable = true;
          CubeColoré.Initialize();
          base.Initialize();
      }

      public override Collider GetCollider()
      {
          CubeCollision.Center = this.Position;
          return CubeCollision;
      }

      public static CubeAdditionnable operator +(CubeAdditionnable cube1, CubeAdditionnable cube2)
      {
          Vector3 direction = cube1.Position - cube2.Position;

          if (direction.X != 0 && direction.Z == 0)
          {
              cube1.CubeColoré.SetDimension(new Vector3(cube1.CubeColoré.Dimension.X + cube2.CubeColoré.Dimension.X, cube1.CubeColoré.Dimension.Y, cube1.CubeColoré.Dimension.Z));
              cube1.CubeColoré.SetOri(cube1.CubeColoré.Ori.X < cube2.CubeColoré.Ori.X ? cube1.CubeColoré.Ori : cube2.CubeColoré.Ori);
              //cube1.CubeColoré.Ori += (cube2.CubeColoré.Ori - cube1.CubeColoré.Ori) / 2;
          }

          if (direction.X == 0 && direction.Z == 0)
          {
              cube1.CubeColoré.SetDimension(new Vector3(cube1.CubeColoré.Dimension.X, cube1.CubeColoré.Dimension.Y + cube2.CubeColoré.Dimension.Y, cube1.CubeColoré.Dimension.Z));
              cube1.CubeColoré.SetOri(cube1.CubeColoré.Ori.Y < cube2.CubeColoré.Ori.Y ? cube1.CubeColoré.Ori : cube2.CubeColoré.Ori);
              //cube1.CubeColoré.Ori += (cube2.CubeColoré.Ori - cube1.CubeColoré.Ori) / 2;
          }

          if (direction.Z != 0 && direction.X == 0)
          {
              cube1.CubeColoré.SetDimension(new Vector3(cube1.CubeColoré.Dimension.X, cube1.CubeColoré.Dimension.Y, cube1.CubeColoré.Dimension.Z + cube2.CubeColoré.Dimension.Z));
              cube1.CubeColoré.SetOri(cube1.CubeColoré.Ori.Z < cube2.CubeColoré.Ori.Z ? cube1.CubeColoré.Ori : cube2.CubeColoré.Ori);
              //cube1.CubeColoré.Ori += (cube2.CubeColoré.Ori - cube1.CubeColoré.Ori) / 2;
          }

          cube1.SetPosition(cube1.CubeColoré.Ori + cube1.CubeColoré.Dimension / 2);
          cube1.CubeCollision = new MCubeCollision(cube1.CubeColoré.Position, cube1.CubeColoré.Dimension, Vector3.Zero);

          return cube1;
      }

      public void SetCaméra(Caméra cam)
      {
          CubeColoré.SetCaméra(cam);
      }

      public override void Draw(GameTime gameTime)
      {
          CubeColoré.SetPosition(this.Position);
          CubeColoré.Draw(gameTime);
      }
      
      public ObjetPhysique GetObjetPhysique()
      {
         return this as ObjetPhysique;
      }
   }
}
