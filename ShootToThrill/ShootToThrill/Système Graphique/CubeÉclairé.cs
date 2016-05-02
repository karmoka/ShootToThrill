using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class CubeÉclairée : CubeTexturé
   {
      public const float PUISSANCE_SPÉCULAIRE = 8f;
      protected string NomTextureBumpMap { get; set; }
      Texture2D TextureBumpMap { get; set; } 
      protected string NomEffetAffichage { get; set; }
      protected Effect EffetAffichage { get; set; }
      Lumière LumièreJeu { get; set; }
      protected RessourcesManager<Effect> GestionnaireDeShaders { get; private set; }
      MatériauÉclairé MatériauAffichage { get; set; }
      Vector3 CouleurLumièreAmbiante { get; set; }
      Vector4 CouleurLumièreDiffuse { get; set; }
      Vector3 CouleurLumièreSpéculaire { get; set; }
      Vector3 CouleurLumièreEmissive { get; set; }
      float CarréDistanceLumière { get; set; }
      InfoModèle InfoSphère { get; set; }
      BoundingSphere SphèreDeCollision { get; set; }

      public CubeÉclairée(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 dimension, float intervalleMAJ, string nomImage,
                            string nomEffetAffichage, Lumière lumièreJeu)
         : base(game,homothétieInitiale,rotationInitiale,positionInitiale,dimension,intervalleMAJ, nomImage)
      {
         NomTextureBumpMap = null;
         NomEffetAffichage = nomEffetAffichage.ToUpper();
         LumièreJeu = lumièreJeu;
         SphèreDeCollision = new BoundingSphere(positionInitiale, 2f);
      }

      //public CubeÉclairée(Atelier jeu, Vector3 origine, float rayon, Vector2 composition, string nomTexture, 
      //                     string nomTextureBumpMap, string nomEffetAffichage, Lumière lumièreJeu, float intervalleMAJ)
      //   : base(jeu, origine, rayon, composition, nomTexture, intervalleMAJ)
      //{
      //   NomTextureBumpMap = nomTextureBumpMap;
      //   NomEffetAffichage = nomEffetAffichage.ToUpper();
      //   LumièreJeu = lumièreJeu;
      //   SphèreDeCollision = new BoundingSphere(origine, rayon);
      //}

      public override void Initialize()
      {
         base.Initialize();
         CouleurLumièreAmbiante = new Vector3(0.4f, 0.4f, 0.4f);
         CouleurLumièreDiffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
         CouleurLumièreEmissive = new Vector3(0.1f, 0.1f, 0.1f);
         CouleurLumièreSpéculaire = new Vector3(0.6f, 0.6f, 0.6f);
         InfoSphère = new InfoModèle(EffetAffichage, Image, true, CouleurLumièreAmbiante, CouleurLumièreDiffuse, CouleurLumièreEmissive, CouleurLumièreSpéculaire, PUISSANCE_SPÉCULAIRE);
         SphèreDeCollision = SphèreDeCollision.Transform(GetMonde());
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         GestionnaireDeShaders = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
         EffetAffichage = GestionnaireDeShaders.Find(NomEffetAffichage);
         TextureBumpMap = NomTextureBumpMap != null ? gestionnaireDeTextures.Find(NomTextureBumpMap) : null;
         MatériauAffichage = new MatériauÉclairé(CaméraJeu, LumièreJeu, TextureBumpMap, CouleurLumièreAmbiante, CouleurLumièreDiffuse,
                                                 CouleurLumièreEmissive, CouleurLumièreSpéculaire, LumièreJeu.Intensité);
      }

      public override void Draw(GameTime gameTime)
      {
         if (CaméraJeu.Frustum.Intersects(SphèreDeCollision))
         {
            MatériauAffichage.UpdateMatériau(Position, GetMonde());
            ParamètresShaders.InitialiserParamètresShader(NomEffetAffichage, EffetAffichage, InfoSphère, MatériauAffichage);
            foreach (EffectPass passeEffet in EffetAffichage.CurrentTechnique.Passes)
            {
               passeEffet.Apply();

               GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleStrip, Sommets1, 0, NB_SOMMETS - 2);
               GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleStrip, Sommets2, 0, NB_SOMMETS - 2);
            }
         }
      }
	}
}