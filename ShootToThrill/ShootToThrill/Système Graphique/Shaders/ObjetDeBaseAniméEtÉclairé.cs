using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AtelierXNA
{
   public class MObjetDeBaseAnim�Et�clair� : MObjetDeBaseAnim�
   {
      public const float PUISSANCE_SP�CULAIRE = 8f;
      string NomTextureMod�le { get; set; }
      string NomTextureBumpMap { get; set; }
      protected string NomEffetAffichage { get; set; }
      Effect EffetAffichage { get; set; }
      Texture2D TextureMod�le { get; set; }
      Texture2D TextureBumpMap { get; set; }
      Lumi�re Lumi�reJeu { get; set; }
      protected Mat�riau�clair� Mat�riauAffichage { get; set; }
      Vector3 CouleurLumi�reAmbiante { get; set; }
      Vector4 CouleurLumi�reDiffuse { get; set; }
      Vector3 CouleurLumi�reSp�culaire { get; set; }
      Vector3 CouleurLumi�reEmissive { get; set; }
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      protected BoundingSphere Sph�reDeCollision { get; set; }

      public MObjetDeBaseAnim�Et�clair�(Game jeu, String nomMod�le, String nomTextureMod�le,
                                       float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                                       string nomEffetAffichage, Lumi�re lumi�reJeu, float intervalleMAJ)
         : base(jeu, nomMod�le, �chelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Lumi�reJeu = lumi�reJeu;
         NomTextureMod�le = nomTextureMod�le;
         NomTextureBumpMap = null;
         NomEffetAffichage = nomEffetAffichage.ToUpper();
      }
      public MObjetDeBaseAnim�Et�clair�(Game jeu, String nomMod�le, String nomTextureMod�le, String nomTextureBumpMap,
                                       float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                                       string nomEffetAffichage, Lumi�re lumi�reJeu, float intervalleMAJ)
         : base(jeu, nomMod�le, �chelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Lumi�reJeu = lumi�reJeu;
         NomTextureMod�le = nomTextureMod�le;
         NomTextureBumpMap = nomTextureBumpMap;
         NomEffetAffichage = nomEffetAffichage.ToUpper();
      }

      public override void Initialize()
      {
         CouleurLumi�reAmbiante = new Vector3(0.5f, 0.5f, 0.5f);
         CouleurLumi�reDiffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
         CouleurLumi�reEmissive = new Vector3(0.1f, 0.1f, 0.1f);
         CouleurLumi�reSp�culaire = new Vector3(0.6f, 0.6f, 0.6f);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         GestionnaireDeShaders = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
         TextureMod�le = GestionnaireDeTextures.Find(NomTextureMod�le);
         TextureBumpMap = NomTextureBumpMap!=null?GestionnaireDeTextures.Find(NomTextureBumpMap):null;
         EffetAffichage = (GestionnaireDeShaders.Find(NomEffetAffichage)).Clone();
         Mat�riauAffichage = new Mat�riau�clair�(Cam�raJeu, Lumi�reJeu, TextureBumpMap, CouleurLumi�reAmbiante, CouleurLumi�reDiffuse,
                                                 CouleurLumi�reEmissive, CouleurLumi�reSp�culaire, Lumi�reJeu.Intensit�);
         AnalyserMod�le();
         Cr�erSph�reDeCollision();
      }

      protected void Cr�erSph�reDeCollision()
      {
         BoundingSphere sph�reTotaleTemporaire = new BoundingSphere();
         for (int i = 0; i < Mod�le.Meshes.Count; ++i)
         {
            BoundingSphere sph�reCollisionDuMaillage = Mod�le.Meshes[i].BoundingSphere;
            sph�reTotaleTemporaire = BoundingSphere.CreateMerged(sph�reTotaleTemporaire, sph�reCollisionDuMaillage); // Ou
            //BoundingSphere.CreateMerged(ref sph�reTotaleTemporaire, ref sph�reCollisionDuMaillage, out sph�reTotaleTemporaire);
         }
         Sph�reDeCollision = sph�reTotaleTemporaire.Transform(Monde);
      }

      private void AnalyserMod�le()
      {
         Texture2D textureLocale = TextureMod�le;

         foreach (ModelMesh maillage in Mod�le.Meshes)
         {
            foreach (ModelMeshPart facetteMaillage in maillage.MeshParts)
            {
               InfoMod�le infoMod�le;
               Effect effetLocal = EffetAffichage.Clone();
               if (facetteMaillage.Effect is BasicEffect)
               {
                  BasicEffect effetDeBase = (BasicEffect)facetteMaillage.Effect;
                  if (effetDeBase.Texture != null)
                  {
                     textureLocale = effetDeBase.Texture;
                  }
                  //infoMod�le = new InfoMod�le(effetDeBase.Texture, effetDeBase.TextureEnabled, effetDeBase.AmbientLightColor, new Vector4(effetDeBase.DiffuseColor, 1f),
                  //                                       effetDeBase.EmissiveColor, effetDeBase.SpecularColor, effetDeBase.SpecularPower);
                  infoMod�le = new InfoMod�le(effetLocal, effetDeBase.Texture, effetDeBase.TextureEnabled, CouleurLumi�reAmbiante, new Vector4(effetDeBase.DiffuseColor, 1f),
                                              CouleurLumi�reEmissive, effetDeBase.SpecularColor, effetDeBase.SpecularPower);
                  //infoMod�le = new InfoMod�le(effetLocal, effetDeBase.Texture, effetDeBase.TextureEnabled, CouleurLumi�reAmbiante, CouleurLumi�reDiffuse,
                  //                            CouleurLumi�reEmissive, CouleurLumi�reSp�culaire, PUISSANCE_SP�CULAIRE);
               }
               else
               {
                  infoMod�le = new InfoMod�le(effetLocal, textureLocale, false, CouleurLumi�reAmbiante, CouleurLumi�reDiffuse, CouleurLumi�reEmissive, CouleurLumi�reSp�culaire, PUISSANCE_SP�CULAIRE);
               }
               facetteMaillage.Tag = infoMod�le;
           }
         }
      }

      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
         Lumi�reJeu.Position = Cam�raJeu.Position;
         Mat�riauAffichage.Cam�raJeu = this.Cam�raJeu;
      }

      public override void Draw(GameTime gameTime)
      {
         if (Cam�raJeu.Frustum.Intersects(Sph�reDeCollision))
         {
            Matrix[] Transformations = new Matrix[Mod�le.Bones.Count];
            Mod�le.CopyAbsoluteBoneTransformsTo(Transformations);

            Mat�riauAffichage.UpdateMat�riau(Position, Monde);
            foreach (ModelMesh maillage in Mod�le.Meshes)
            {
               foreach (ModelMeshPart facetteMaillage in maillage.MeshParts)
               {
                  InfoMod�le infoMod�le = (InfoMod�le)facetteMaillage.Tag;
                  Effect effetLocal = infoMod�le.EffetAffichage;
                  Param�tresShaders.InitialiserParam�tresShader(NomEffetAffichage, effetLocal, infoMod�le, Mat�riauAffichage);
                  facetteMaillage.Effect = effetLocal;
               }
               maillage.Draw();
            }
         }
      }
   }
}
