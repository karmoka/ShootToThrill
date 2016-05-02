// Auteur :       Raphael Croteau
// Fichier :      ModelPhysique.cs
// Description :  !!!Désuet!!! Classe liant la physique a un cube coloré
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class MCubePhysique : ObjetPhysique, IModele3d, IPositionable
   {

      MCubeCollision cubeCollision { get; set; }
      CubeColoré cubeGraphique { get; set; }

      public MCubePhysique(Game game, Vector3 position, Vector3 dimension, Vector3 vitesse, float masse)
         : base(game,position,vitesse,masse)
      {
         cubeCollision = new MCubeCollision(position,dimension,new Vector3(0f,0f,0f));
         cubeGraphique = new CubeColoré(game, 1, new Vector3(0f, 0f, 0f), position, Color.White, dimension, 1 / 60f);
      }

      public override void Initialize()
      {
         EstImmuable = true;
         cubeGraphique.Initialize();
         base.Initialize();
      }

      public override Collider GetCollider()
      {
         cubeCollision.Center = this.Position;
         return cubeCollision;
      }

      public void SetCaméra(Caméra cam)
      {
         cubeGraphique.SetCaméra(cam);
      }

      public override void Draw(GameTime gameTime)
      {
         cubeGraphique.SetPosition(this.Position);
         cubeGraphique.Draw(gameTime);
      }
   }
}
