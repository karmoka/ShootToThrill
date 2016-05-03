using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public abstract class PrimitiveDeBase2 : Microsoft.Xna.Framework.DrawableGameComponent
   {
      protected float Homoth�tieInitiale { get; private set; }
      protected Vector3 RotationInitiale { get; private set; }
      protected Vector3 PositionInitiale { get; private set; }
      protected int NbSommets { get; set; }
      protected int NbTriangles { get; set; }
      protected Matrix Monde { get; set; }
      protected Cam�ra Cam�raActuelle { get; set; }

      protected PrimitiveDeBase2(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu)
      {
         Homoth�tieInitiale = homoth�tieInitiale;
         RotationInitiale = rotationInitiale;
         PositionInitiale = positionInitiale;
      }

      public override void Initialize()
      {
         InitialiserSommets();
         CalculerMatriceMonde();
         base.Initialize();
      }

      protected override void LoadContent()
      {
         Cam�raActuelle = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
         base.LoadContent();
      }

      protected virtual void CalculerMatriceMonde()
      {
         Monde = Matrix.Identity *
                 Matrix.CreateScale(Homoth�tieInitiale) *
                 Matrix.CreateFromYawPitchRoll(RotationInitiale.Y, RotationInitiale.X, RotationInitiale.Z) *
                 Matrix.CreateTranslation(PositionInitiale);
      }

      protected abstract void InitialiserSommets();

      public virtual Matrix GetMonde()
      {
         return Monde;
      }
   }
}