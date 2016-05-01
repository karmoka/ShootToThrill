using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class ParamètresShaders
   {
      public static void InitialiserParamètresShader(string nomEffet, Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderBase(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         effetLocal.Parameters["MondeVueProjection"].SetValue(MatériauAffichage.MondeVueProjection);
         effetLocal.Parameters["CouleurLumiereDiffuse"].SetValue(infoModèle.CouleurDiffuse);
         effetLocal.Parameters["Texture"].SetValue(infoModèle.Texture);
         effetLocal.Parameters["TextureActive"].SetValue(infoModèle.TextureActive);
      }

      static void InitialiserParamètresShaderBlinnPhong(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderBlinnPhongBumpMap(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderPhong(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderPhongJean(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderSpotLight(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
      {
         Vector3[] Directions = { new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(-5, -5, 0) };
         Vector3[] Positions = { new Vector3(0, 10, 0), new Vector3(0, -10, 0), new Vector3(0, 0, 10), new Vector3(0, 0, -10) };
         Vector4[] Couleurs = { new Vector4(0, 0, 255, 1), new Vector4(0, 255, 0, 1), new Vector4(255, 0, 0, 1), new Vector4(255, 255, 255, 0.5f) };
         float[] Rayons = { 20f, 15f, 15f, 5f };

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

      static void InitialiserParamètresShaderTeinteDeGris(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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

      static void InitialiserParamètresShaderModèle(Effect effetLocal, InfoModèle infoModèle, MatériauÉclairé MatériauAffichage)
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
