using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AtelierXNA
{
   public class MObjetDeBaseAniméEtÉclairé : MObjetDeBaseAnimé
   {
      public const float PUISSANCE_SPÉCULAIRE = 8f;
      string NomTextureModèle { get; set; }
      string NomTextureBumpMap { get; set; }
      protected string NomEffetAffichage { get; set; }
      Effect EffetAffichage { get; set; }
      Texture2D TextureModèle { get; set; }
      Texture2D TextureBumpMap { get; set; }
      Lumière LumièreJeu { get; set; }
      protected MatériauÉclairé MatériauAffichage { get; set; }
      Vector3 CouleurLumièreAmbiante { get; set; }
      Vector4 CouleurLumièreDiffuse { get; set; }
      Vector3 CouleurLumièreSpéculaire { get; set; }
      Vector3 CouleurLumièreEmissive { get; set; }
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      protected BoundingSphere SphèreDeCollision { get; set; }

      public MObjetDeBaseAniméEtÉclairé(Game jeu, String nomModèle, String nomTextureModèle,
                                       float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                                       string nomEffetAffichage, Lumière lumièreJeu, float intervalleMAJ)
         : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         LumièreJeu = lumièreJeu;
         NomTextureModèle = nomTextureModèle;
         NomTextureBumpMap = null;
         NomEffetAffichage = nomEffetAffichage.ToUpper();
      }
      public MObjetDeBaseAniméEtÉclairé(Game jeu, String nomModèle, String nomTextureModèle, String nomTextureBumpMap,
                                       float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                                       string nomEffetAffichage, Lumière lumièreJeu, float intervalleMAJ)
         : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         LumièreJeu = lumièreJeu;
         NomTextureModèle = nomTextureModèle;
         NomTextureBumpMap = nomTextureBumpMap;
         NomEffetAffichage = nomEffetAffichage.ToUpper();
      }

      public override void Initialize()
      {
         CouleurLumièreAmbiante = new Vector3(0.5f, 0.5f, 0.5f);
         CouleurLumièreDiffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
         CouleurLumièreEmissive = new Vector3(0.1f, 0.1f, 0.1f);
         CouleurLumièreSpéculaire = new Vector3(0.6f, 0.6f, 0.6f);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         GestionnaireDeShaders = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
         TextureModèle = GestionnaireDeTextures.Find(NomTextureModèle);
         TextureBumpMap = NomTextureBumpMap!=null?GestionnaireDeTextures.Find(NomTextureBumpMap):null;
         EffetAffichage = (GestionnaireDeShaders.Find(NomEffetAffichage)).Clone();
         MatériauAffichage = new MatériauÉclairé(CaméraJeu, LumièreJeu, TextureBumpMap, CouleurLumièreAmbiante, CouleurLumièreDiffuse,
                                                 CouleurLumièreEmissive, CouleurLumièreSpéculaire, LumièreJeu.Intensité);
         AnalyserModèle();
         CréerSphèreDeCollision();
      }

      protected void CréerSphèreDeCollision()
      {
         BoundingSphere sphèreTotaleTemporaire = new BoundingSphere();
         for (int i = 0; i < Modèle.Meshes.Count; ++i)
         {
            BoundingSphere sphèreCollisionDuMaillage = Modèle.Meshes[i].BoundingSphere;
            sphèreTotaleTemporaire = BoundingSphere.CreateMerged(sphèreTotaleTemporaire, sphèreCollisionDuMaillage); // Ou
            //BoundingSphere.CreateMerged(ref sphèreTotaleTemporaire, ref sphèreCollisionDuMaillage, out sphèreTotaleTemporaire);
         }
         SphèreDeCollision = sphèreTotaleTemporaire.Transform(Monde);
      }

      private void AnalyserModèle()
      {
         Texture2D textureLocale = TextureModèle;

         foreach (ModelMesh maillage in Modèle.Meshes)
         {
            foreach (ModelMeshPart facetteMaillage in maillage.MeshParts)
            {
               InfoModèle infoModèle;
               Effect effetLocal = EffetAffichage.Clone();
               if (facetteMaillage.Effect is BasicEffect)
               {
                  BasicEffect effetDeBase = (BasicEffect)facetteMaillage.Effect;
                  if (effetDeBase.Texture != null)
                  {
                     textureLocale = effetDeBase.Texture;
                  }
                  //infoModèle = new InfoModèle(effetDeBase.Texture, effetDeBase.TextureEnabled, effetDeBase.AmbientLightColor, new Vector4(effetDeBase.DiffuseColor, 1f),
                  //                                       effetDeBase.EmissiveColor, effetDeBase.SpecularColor, effetDeBase.SpecularPower);
                  infoModèle = new InfoModèle(effetLocal, effetDeBase.Texture, effetDeBase.TextureEnabled, CouleurLumièreAmbiante, new Vector4(effetDeBase.DiffuseColor, 1f),
                                              CouleurLumièreEmissive, effetDeBase.SpecularColor, effetDeBase.SpecularPower);
                  //infoModèle = new InfoModèle(effetLocal, effetDeBase.Texture, effetDeBase.TextureEnabled, CouleurLumièreAmbiante, CouleurLumièreDiffuse,
                  //                            CouleurLumièreEmissive, CouleurLumièreSpéculaire, PUISSANCE_SPÉCULAIRE);
               }
               else
               {
                  infoModèle = new InfoModèle(effetLocal, textureLocale, false, CouleurLumièreAmbiante, CouleurLumièreDiffuse, CouleurLumièreEmissive, CouleurLumièreSpéculaire, PUISSANCE_SPÉCULAIRE);
               }
               facetteMaillage.Tag = infoModèle;
           }
         }
      }

      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
         LumièreJeu.Position = CaméraJeu.Position;
         MatériauAffichage.CaméraJeu = this.CaméraJeu;
      }

      public override void Draw(GameTime gameTime)
      {
         if (CaméraJeu.Frustum.Intersects(SphèreDeCollision))
         {
            Matrix[] Transformations = new Matrix[Modèle.Bones.Count];
            Modèle.CopyAbsoluteBoneTransformsTo(Transformations);

            MatériauAffichage.UpdateMatériau(Position, Monde);
            foreach (ModelMesh maillage in Modèle.Meshes)
            {
               foreach (ModelMeshPart facetteMaillage in maillage.MeshParts)
               {
                  InfoModèle infoModèle = (InfoModèle)facetteMaillage.Tag;
                  Effect effetLocal = infoModèle.EffetAffichage;
                  ParamètresShaders.InitialiserParamètresShader(NomEffetAffichage, effetLocal, infoModèle, MatériauAffichage);
                  facetteMaillage.Effect = effetLocal;
               }
               maillage.Draw();
            }
         }
      }
   }
}
