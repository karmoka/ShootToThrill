using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class ParamètresShaders : GameComponent
   {
      GestionnairesLumières GestionnaireDeLumières { get; set; }

      public ParamètresShaders(Game game)
         : base(game)
      {

      }

      public void AssignerGestionnaireDeLumière(GestionnairesLumières g)
      {
         GestionnaireDeLumières = g;
      }

      public override void Initialize()
      {

         base.Initialize();
      }

      public void InitialiserParamètresShader(string nomEffet, Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         switch (nomEffet)
         {
            case "BASE":
               InitialiserParamètresShaderBase(effetLocal, infoModèle, MatériauAffichage);
               break;
            case "BLINN-PHONG":
               InitialiserParamètresShaderBlinnPhong(effetLocal, infoModèle, MatériauAffichage);
               break;
            case "BLINN-PHONG-BUMPMAP":
               InitialiserParamètresShaderBlinnPhongBumpMap(effetLocal, infoModèle, MatériauAffichage);
               break;
            case "PHONG":
               InitialiserParamètresShaderPhong(effetLocal, infoModèle, MatériauAffichage);
               break;
            case "PHONG-JEAN":
               InitialiserParamètresShaderPhongJean(effetLocal, infoModèle, MatériauAffichage);
               break;
            case "SPOTLIGHT":
               InitialiserParamètresShaderSpotLight(effetLocal, infoModèle, MatériauAffichage);
               break;
            default:
               throw new ArgumentException("Paramètres de l'effet d'affichage inconnus!");
         }
      }

      void InitialiserParamètresShaderBase(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      void InitialiserParamètresShaderBlinnPhong(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["MondeTransposeeInverse"].SetValue(MatériauAffichage.MondeTransposéeInverse);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["DirectionLumiere"].SetValue(MatériauAffichage.Position - MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["CouleurLumiere"].SetValue(MatériauAffichage.LumièreJeu.Couleur);
         effetLocal.Parameters["CouleurLumiereAmbiante"].SetValue(infoModèle.CouleurAmbiante);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["CouleurLumiereEmissive"].SetValue(infoModèle.CouleurEmissive);
         effetLocal.Parameters["CouleurLumiereSpeculaire"].SetValue(infoModèle.CouleurSpéculaire);
         effetLocal.Parameters["PuissanceSpeculaire"].SetValue(infoModèle.PuissanceSpéculaire);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      void InitialiserParamètresShaderBlinnPhongBumpMap(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["MondeTransposeeInverse"].SetValue(MatériauAffichage.MondeTransposéeInverse);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["DirectionLumiere"].SetValue(MatériauAffichage.Position - MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["CouleurLumiere"].SetValue(MatériauAffichage.LumièreJeu.Couleur);
         effetLocal.Parameters["CouleurLumiereAmbiante"].SetValue(infoModèle.CouleurAmbiante);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["CouleurLumiereEmissive"].SetValue(infoModèle.CouleurEmissive);
         effetLocal.Parameters["CouleurLumiereSpeculaire"].SetValue(infoModèle.CouleurSpéculaire);
         effetLocal.Parameters["PuissanceSpeculaire"].SetValue(infoModèle.PuissanceSpéculaire);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
         effetLocal.Parameters["BumpMap"].SetValue(MatériauAffichage.BumpMap);
      }

      void InitialiserParamètresShaderPhong(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["PositionLumiere"].SetValue(MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["DirectionLumiere"].SetValue(MatériauAffichage.Position - MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["CarreDistanceLumiere"].SetValue(MatériauAffichage.CarréDistanceLumière);
         effetLocal.Parameters["CouleurLumiereAmbiante"].SetValue(infoModèle.CouleurAmbiante);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["CouleurLumiereSpeculaire"].SetValue(infoModèle.CouleurSpéculaire);
         effetLocal.Parameters["PuissanceSpeculaire"].SetValue(infoModèle.PuissanceSpéculaire);
         effetLocal.Parameters["IntensiteLumiereDiffuse"].SetValue(infoModèle.IntensitéLumièreDiffuse);
         effetLocal.Parameters["IntensiteLumiereSpeculaire"].SetValue(infoModèle.IntensitéLumièreSpéculaire);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      void InitialiserParamètresShaderPhongJean(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["PositionLumiere"].SetValue(MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["CouleurLumiereAmbiante"].SetValue(infoModèle.CouleurAmbiante);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["PuissanceSpeculaire"].SetValue(infoModèle.PuissanceSpéculaire);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      void InitialiserParamètresShaderSpotLight(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         if (GestionnaireDeLumières == null)
         {
            Vector3[] Positions = { new Vector3(0, 10, 0), new Vector3(0, -10, 0), new Vector3(0, 0, 10), new Vector3(0, 0, -10) };
            Vector3[] Couleurs = { new Vector3(0, 0, 255), new Vector3(0, 255, 0), new Vector3(255, 0, 0), new Vector3(255, 255, 255) };
            float[] Rayons = { 100, 100, 100, 100 };
            effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
            effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
            effetLocal.Parameters["MondeTransposeeInverse"].SetValue(MatériauAffichage.MondeTransposéeInverse);
            effetLocal.Parameters["PositionObjet"].SetValue(MatériauAffichage.Position);

            effetLocal.Parameters["NombreLumieres"].SetValue(4);
            effetLocal.Parameters["PositionLumieres"].SetValue(Positions);
            effetLocal.Parameters["CouleursLumieres"].SetValue(Couleurs);
            effetLocal.Parameters["RayonsLumieres"].SetValue(Rayons);

            effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
            effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
            effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
         }
         else
         {
            effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
            effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
            effetLocal.Parameters["MondeTransposeeInverse"].SetValue(MatériauAffichage.MondeTransposéeInverse);
            effetLocal.Parameters["PositionObjet"].SetValue(MatériauAffichage.Position);

            effetLocal.Parameters["NombreLumieres"].SetValue(GestionnaireDeLumières.NombreLumières());
            effetLocal.Parameters["PositionLumieres"].SetValue(GestionnaireDeLumières.Positions());
            effetLocal.Parameters["CouleursLumieres"].SetValue(GestionnaireDeLumières.Couleurs());
            effetLocal.Parameters["RayonsLumieres"].SetValue(GestionnaireDeLumières.Rayons());

            effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
            effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
            effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
         }



      }

      void InitialiserParamètresShaderTeinteDeGris(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["PositionLumiere"].SetValue(MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["RayonLumiere"].SetValue(MatériauAffichage.LumièreJeu.Rayon);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      void InitialiserParamètresShaderModèle(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["Monde"].SetValue(MatériauAffichage.Monde);
         effetLocal.Parameters["VueProjection"].SetValue(MatériauAffichage.VueProjection);
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["MondeTransposeeInverse"].SetValue(MatériauAffichage.MondeTransposéeInverse);
         effetLocal.Parameters["PositionCamera"].SetValue(MatériauAffichage.CaméraJeu.Position);
         effetLocal.Parameters["PositionLumiere"].SetValue(MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["DirectionLumiere"].SetValue(MatériauAffichage.Position - MatériauAffichage.LumièreJeu.Position);
         effetLocal.Parameters["CouleurLumiere"].SetValue(MatériauAffichage.LumièreJeu.Couleur);
         effetLocal.Parameters["CarreDistanceLumiere"].SetValue(MatériauAffichage.CarréDistanceLumière);
         effetLocal.Parameters["RayonLumiere"].SetValue(MatériauAffichage.LumièreJeu.Rayon);
         effetLocal.Parameters["CouleurLumiereAmbiante"].SetValue(infoModèle.CouleurAmbiante);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["CouleurLumiereEmissive"].SetValue(infoModèle.CouleurEmissive);
         effetLocal.Parameters["CouleurLumiereSpeculaire"].SetValue(infoModèle.CouleurSpéculaire);
         effetLocal.Parameters["PuissanceSpeculaire"].SetValue(infoModèle.PuissanceSpéculaire);
         effetLocal.Parameters["IntensiteLumiereDiffuse"].SetValue(infoModèle.IntensitéLumièreDiffuse);
         effetLocal.Parameters["IntensiteLumiereSpeculaire"].SetValue(infoModèle.IntensitéLumièreSpéculaire);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }
   }
}
