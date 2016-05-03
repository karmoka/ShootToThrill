using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ProjetPrincipal.Data;


namespace AtelierXNA
{
   public class MObjetDeBaseAniméEtÉclairé : MObjetDeBaseAnimé
   {
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      protected ParamètresShaders GestionnaireParamètresShaders { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }

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

      public BoundingSphere SphèreDeCollision { get; private set; }

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
      //game, description, position, 1/60f
      public MObjetDeBaseAniméEtÉclairé(Game jeu, DescriptionObjetDeBaseAniméÉclairé description, Vector3 positionInitiale, float intervalleMAJ)
          : base(jeu, description.NomModèle, description.Échelle, description.Rotation, positionInitiale, intervalleMAJ)
      {
          LumièreJeu = new Lumière(jeu,positionInitiale,Color.White.ToVector3(),1,1,Vector3.One,Vector4.One);
          NomTextureModèle = description.NomTextureModèle;
          NomTextureBumpMap = null;
          NomEffetAffichage = description.NomEffetAffichage.ToUpper();
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
         GestionnaireParamètresShaders = Game.Services.GetService(typeof(ParamètresShaders)) as ParamètresShaders;
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         GestionnaireDeShaders = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;

         TextureModèle = GestionnaireDeTextures.Find(NomTextureModèle);
         TextureBumpMap = NomTextureBumpMap!=null?GestionnaireDeTextures.Find(NomTextureBumpMap):null;
         EffetAffichage = (GestionnaireDeShaders.Find(NomEffetAffichage)).Clone();
         MatériauAffichage = new MatériauÉclairé(CaméraActuelle, LumièreJeu, TextureBumpMap, CouleurLumièreAmbiante, CouleurLumièreDiffuse,
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

                  infoModèle = new InfoModèle(effetLocal, effetDeBase.Texture, effetDeBase.TextureEnabled, CouleurLumièreAmbiante, new Vector4(effetDeBase.DiffuseColor, 1f),
                                              CouleurLumièreEmissive, effetDeBase.SpecularColor, effetDeBase.SpecularPower);
               }
               else
               {
                  infoModèle = new InfoModèle(effetLocal, textureLocale, false, CouleurLumièreAmbiante, CouleurLumièreDiffuse, CouleurLumièreEmissive, CouleurLumièreSpéculaire, PUISSANCE_SPÉCULAIRE);
               }
               facetteMaillage.Tag = infoModèle;
           }
         }
      }

      public override void Draw(GameTime gameTime)
      {
         //if (CaméraActuelle.Frustum.Intersects(SphèreDeCollision))
         {
            MatériauAffichage.CaméraJeu = this.CaméraActuelle;

            Matrix[] Transformations = new Matrix[Modèle.Bones.Count];
            Modèle.CopyAbsoluteBoneTransformsTo(Transformations);

            MatériauAffichage.UpdateMatériau(Position, Monde);
            foreach (ModelMesh maillage in Modèle.Meshes)
            {
               foreach (ModelMeshPart facetteMaillage in maillage.MeshParts)
               {
                  InfoModèle infoModèle = (InfoModèle)facetteMaillage.Tag;
                  Effect effetLocal = infoModèle.EffetAffichage;
                  GestionnaireParamètresShaders.InitialiserParamètresShader(NomEffetAffichage, effetLocal, infoModèle, MatériauAffichage);
                  facetteMaillage.Effect = effetLocal;
               }
               maillage.Draw();
            }
         }
      }
   }
}
