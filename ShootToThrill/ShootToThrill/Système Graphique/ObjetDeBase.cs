using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class MObjetDeBase : Microsoft.Xna.Framework.DrawableGameComponent, IModele3d
   {
      Effect MyEffect { get; set; }

      string NomModèle { get; set; }
      RessourcesManager<Model> GestionnaireDeModèles { get; set; }
      protected Caméra CaméraJeu { get; set; }
      protected float Échelle { get; set; }
      protected Vector3 Rotation { get; set; }
      protected Vector3 Position { get; set; }

      protected Model Modèle { get; private set; }
      protected Matrix[] TransformationsModèle { get; private set; }
      protected Matrix Monde { get; set; }

      public MObjetDeBase(Game jeu, String nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu)
      {
         NomModèle =  nomModèle;
         Position = positionInitiale;
         Échelle = échelleInitiale;
         Rotation = rotationInitiale;
      }

      public override void Initialize()
      {
         CalculerMonde();
         base.Initialize();
      }

      private void CalculerMonde()
      {
         Monde = Matrix.Identity;
         Monde *= Matrix.CreateScale(1f);
         Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
         Monde *= Matrix.CreateTranslation(Position);
      }

      protected override void LoadContent()
      {
         CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
         GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
         Modèle = GestionnaireDeModèles.Find(NomModèle);
         TransformationsModèle = new Matrix[Modèle.Bones.Count];
         Modèle.CopyAbsoluteBoneTransformsTo(TransformationsModèle);

         MyEffect = Game.Content.Load<Effect>("Effets/Lumière");
      }

      public override void Draw(GameTime gameTime)
      {
         foreach (ModelMesh maille in Modèle.Meshes)
         {
            Matrix mondeLocal = TransformationsModèle[maille.ParentBone.Index] * GetMonde();
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(maille.ParentBone.Transform * GetMonde()));

            foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
            {
               portionDeMaillage.Effect = MyEffect;
               MyEffect.Parameters["WorldMatrix"].SetValue(mondeLocal * maille.ParentBone.Transform);
               MyEffect.Parameters["ViewMatrix"].SetValue(CaméraJeu.Vue);
               MyEffect.Parameters["ProjectionMatrix"].SetValue(CaméraJeu.Projection);
               MyEffect.Parameters["WorldInverseTransposeMatrix"].SetValue(worldInverseTransposeMatrix);
               //BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
               //effet.EnableDefaultLighting();
               //effet.Projection = CaméraJeu.Projection;
               //effet.View = CaméraJeu.Vue;
               //effet.World = mondeLocal;
            }
            maille.Draw();
         }
      }

      public virtual Matrix GetMonde()
      {
         return Monde;
      }

      public void SetCaméra(Caméra cam)
      {
         CaméraJeu = cam;
      }
      public void ChangerCouleur(Color couleur)
      {

      }
      public void SetPosition(Vector3 position)
      {
         CalculerMonde();
         this.Position = position;
      }
   }
}
