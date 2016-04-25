using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
   public class CaméraTracing : Caméra  
   {
       Options OptionJeu { get; set; }
      IPositionable ObjetTracé { get; set; }
      public Viewport CameraViewPort { get; set; }

      public CaméraTracing(Game jeu, Vector3 cible, Vector3 orientation,IPositionable objetATracer, Viewport viewPort)
         : base(jeu)
      {
          OptionJeu = Game.Services.GetService(typeof(Options)) as Options;

         CameraViewPort = viewPort;
         ObjetTracé = objetATracer;

         Vector3 positionCaméra = cible + new Vector3(OptionJeu.CameraDistanceOffset, OptionJeu.CameraHeightOffset, OptionJeu.CameraDistanceOffset);
         CréerPointDeVue(positionCaméra, cible, orientation); // Création de la matrice de vue
         CréerVolumeDeVisualisation(Caméra.OUVERTURE_OBJECTIF, CameraViewPort.AspectRatio, Caméra.DISTANCE_PLAN_RAPPROCHÉ, Caméra.DISTANCE_PLAN_ÉLOIGNÉ); // Création de la matrice de projection (volume de visualisation)
      }

      public override void Update(GameTime gameTime)
      {
         SuivreCible(ObjetTracé.Position);

         base.Update(gameTime);
      }

      public void SuivreCible(Vector3 cible)
      {
          Vector3 positionCaméra = cible + new Vector3(OptionJeu.CameraDistanceOffset , OptionJeu.CameraHeightOffset, OptionJeu.CameraDistanceOffset);

         CréerPointDeVue(positionCaméra, cible, OrientationVerticale);
      }

      public Vector3 ProjeterDansEspace(Vector2 v, float distance)
      {
         return CameraViewPort.Unproject(new Vector3(v.X, v.Y, distance), this.Projection, this.Vue, Matrix.Identity);
      }

      public bool EnVue(BoundingSphere b)
      {
         return new BoundingFrustum(Vue * Projection).Intersects(b);
      }
   }
}
