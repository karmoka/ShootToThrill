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
       
       CubeCollision CubeCollision { get; set; }
       public CubeColoré CubeColoré { get; private set; }
       public CubeÉclairée CubeTexturé { get; private set; }
       public bool EstTransparent { get; set; }

      public CubeAdditionnable(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur, Vector3 dimension, float intervalleMAJ)
          : base(game, positionInitiale, Vector3.Zero, 0)
      {
          //Position = positionInitiale;
          CubeCollision = new CubeCollision(positionInitiale, dimension, Vector3.Zero);
          CubeColoré = new CubeColoré(game, homothétieInitiale, Vector3.Zero, positionInitiale, couleur, dimension, intervalleMAJ);
          CubeTexturé = new CubeÉclairée(game, homothétieInitiale, Vector3.Zero, positionInitiale, dimension, intervalleMAJ, "strip3", "Spotlight",new Lumière(Game));
      }

      public override void Initialize()
      {
          EstImmuable = true;
          CubeColoré.Initialize();
          CubeTexturé.Initialize();
          EstTransparent = false;
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

              cube1.CubeTexturé.SetDimension(new Vector3(cube1.CubeTexturé.Dimension.X + cube2.CubeTexturé.Dimension.X, cube1.CubeTexturé.Dimension.Y, cube1.CubeTexturé.Dimension.Z));
              cube1.CubeTexturé.SetOri(cube1.CubeTexturé.Ori.X < cube2.CubeTexturé.Ori.X ? cube1.CubeTexturé.Ori : cube2.CubeTexturé.Ori);
          }

          if (direction.X == 0 && direction.Z == 0)
          {
              cube1.CubeColoré.SetDimension(new Vector3(cube1.CubeColoré.Dimension.X, cube1.CubeColoré.Dimension.Y + cube2.CubeColoré.Dimension.Y, cube1.CubeColoré.Dimension.Z));
              cube1.CubeColoré.SetOri(cube1.CubeColoré.Ori.Y < cube2.CubeColoré.Ori.Y ? cube1.CubeColoré.Ori : cube2.CubeColoré.Ori);

              cube1.CubeTexturé.SetDimension(new Vector3(cube1.CubeTexturé.Dimension.X, cube1.CubeTexturé.Dimension.Y + cube2.CubeTexturé.Dimension.Y, cube1.CubeTexturé.Dimension.Z));
              cube1.CubeTexturé.SetOri(cube1.CubeTexturé.Ori.Y < cube2.CubeTexturé.Ori.Y ? cube1.CubeTexturé.Ori : cube2.CubeTexturé.Ori);
          }

          if (direction.Z != 0 && direction.X == 0)
          {
              cube1.CubeColoré.SetDimension(new Vector3(cube1.CubeColoré.Dimension.X, cube1.CubeColoré.Dimension.Y, cube1.CubeColoré.Dimension.Z + cube2.CubeColoré.Dimension.Z));
              cube1.CubeColoré.SetOri(cube1.CubeColoré.Ori.Z < cube2.CubeColoré.Ori.Z ? cube1.CubeColoré.Ori : cube2.CubeColoré.Ori);

              cube1.CubeTexturé.SetDimension(new Vector3(cube1.CubeTexturé.Dimension.X, cube1.CubeTexturé.Dimension.Y, cube1.CubeTexturé.Dimension.Z + cube2.CubeTexturé.Dimension.Z));
              cube1.CubeTexturé.SetOri(cube1.CubeTexturé.Ori.Z < cube2.CubeTexturé.Ori.Z ? cube1.CubeTexturé.Ori : cube2.CubeTexturé.Ori);
          }

          cube1.SetPosition(cube1.CubeColoré.Ori + cube1.CubeColoré.Dimension / 2);
          cube1.CubeCollision = new CubeCollision(cube1.CubeColoré.Position, cube1.CubeColoré.Dimension, Vector3.Zero);

          return cube1;
      }

      public void SetCaméra(Caméra cam)
      {
          CubeColoré.SetCaméra(cam);
          CubeTexturé.SetCaméra(cam);
      }

      public override void Draw(GameTime gameTime)
      {
          //CubeColoré.SetPosition(this.Position);
          //CubeColoré.Draw(gameTime);

          if (!EstTransparent)
          {
              CubeTexturé.SetPosition(this.Position);
              CubeTexturé.Draw(gameTime);
          }
      }
      
      public ObjetPhysique GetObjetPhysique()
      {
         return this as ObjetPhysique;
      }
   }
}
