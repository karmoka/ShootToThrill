using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public abstract class PrimitiveDeBaseAnim�e2 : PrimitiveDeBase2
   {
      public float Homoth�tie { get; set; }
      public Vector3 Position { get; set; }
      public float IntervalleMAJ { get; set; }
      protected InputManager GestionInput { get; private set; }
      float Temps�coul�DepuisMAJ { get; set; }
      float Incr�mentAngleRotation { get; set; }
      bool Lacet { get; set; }
      bool Tangage { get; set; }
      bool Roulis { get; set; }
      protected bool Monde�Recalculer { get; set; }

      float angleLacet;
      protected float AngleLacet
      {
         get
         {
            if (Lacet)
            {
               angleLacet += Incr�mentAngleRotation;
               MathHelper.WrapAngle(angleLacet);
            }
            return angleLacet;
         }
         set { angleLacet = value; }
      }

      float angleTangage;
      protected float AngleTangage
      {
         get
         {
            if (Tangage)
            {
               angleTangage += Incr�mentAngleRotation;
               MathHelper.WrapAngle(angleTangage);
            }
            return angleTangage;
         }
         set { angleTangage = value; }
      }

      float angleRoulis;
      protected float AngleRoulis
      {
         get
         {
            if (Roulis)
            {
               angleRoulis += Incr�mentAngleRotation;
               MathHelper.WrapAngle(angleRoulis);
            }
            return angleRoulis;
         }
         set { angleRoulis = value; }
      }


      protected PrimitiveDeBaseAnim�e2(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
         : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale)
      {
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         Homoth�tie = Homoth�tieInitiale;
         InitialiserRotations();
         Position = PositionInitiale;
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         Incr�mentAngleRotation = MathHelper.Pi * IntervalleMAJ / 2;
         Temps�coul�DepuisMAJ = 0;
         base.Initialize();
      }

      protected override void CalculerMatriceMonde()
      {
         Monde = Matrix.Identity *
                 Matrix.CreateScale(Homoth�tie) *
                 Matrix.CreateFromYawPitchRoll(AngleLacet, AngleTangage, AngleRoulis) *
                 Matrix.CreateTranslation(Position);
      }

      public override void Update(GameTime gameTime)
      {
         G�rerClavier();
         float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
         Temps�coul�DepuisMAJ += Temps�coul�;
         if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
         {
            EffectuerMise�Jour();
            Temps�coul�DepuisMAJ -= IntervalleMAJ;
         }
         base.Update(gameTime);
      }

      protected virtual void EffectuerMise�Jour()
      {
         if (Monde�Recalculer)
         {
            CalculerMatriceMonde();
            Monde�Recalculer = false;
         }
      }

      private void InitialiserRotations()
      {
         AngleLacet = RotationInitiale.Y;
         AngleTangage = RotationInitiale.X;
         AngleRoulis = RotationInitiale.Z;
      }

      protected virtual void G�rerClavier()
      {
         if (GestionInput.EstEnfonc�e(Keys.LeftControl) || GestionInput.EstEnfonc�e(Keys.RightControl))
         {
            if (GestionInput.EstNouvelleTouche(Keys.Space))
            {
               InitialiserRotations();
               Monde�Recalculer = true;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D1) || GestionInput.EstNouvelleTouche(Keys.NumPad1))
            {
               Lacet = !Lacet;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D2) || GestionInput.EstNouvelleTouche(Keys.NumPad2))
            {
               Tangage = !Tangage;
            }
            if (GestionInput.EstNouvelleTouche(Keys.D3) || GestionInput.EstNouvelleTouche(Keys.NumPad3))
            {
               Roulis = !Roulis;
            }
         }
         Monde�Recalculer = Monde�Recalculer || Lacet || Tangage || Roulis;
      }
   }
}