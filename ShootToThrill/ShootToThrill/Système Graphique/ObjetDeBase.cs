using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class MObjetDeBase : Microsoft.Xna.Framework.DrawableGameComponent, IModele3d
   {
      Effect MyEffect { get; set; }

      string NomMod�le { get; set; }
      RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
      protected Cam�ra Cam�raJeu { get; set; }
      protected float �chelle { get; set; }
      protected Vector3 Rotation { get; set; }
      protected Vector3 Position { get; set; }

      protected Model Mod�le { get; private set; }
      protected Matrix[] TransformationsMod�le { get; private set; }
      protected Matrix Monde { get; set; }

      public MObjetDeBase(Game jeu, String nomMod�le, float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu)
      {
         NomMod�le =  nomMod�le;
         Position = positionInitiale;
         �chelle = �chelleInitiale;
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
         Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
         GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
         Mod�le = GestionnaireDeMod�les.Find(NomMod�le);
         TransformationsMod�le = new Matrix[Mod�le.Bones.Count];
         Mod�le.CopyAbsoluteBoneTransformsTo(TransformationsMod�le);

         MyEffect = Game.Content.Load<Effect>("Effets/Lumi�re");
      }

      public override void Draw(GameTime gameTime)
      {
         foreach (ModelMesh maille in Mod�le.Meshes)
         {
            Matrix mondeLocal = TransformationsMod�le[maille.ParentBone.Index] * GetMonde();
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(maille.ParentBone.Transform * GetMonde()));

            foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
            {
               portionDeMaillage.Effect = MyEffect;
               MyEffect.Parameters["WorldMatrix"].SetValue(mondeLocal * maille.ParentBone.Transform);
               MyEffect.Parameters["ViewMatrix"].SetValue(Cam�raJeu.Vue);
               MyEffect.Parameters["ProjectionMatrix"].SetValue(Cam�raJeu.Projection);
               MyEffect.Parameters["WorldInverseTransposeMatrix"].SetValue(worldInverseTransposeMatrix);
               //BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
               //effet.EnableDefaultLighting();
               //effet.Projection = Cam�raJeu.Projection;
               //effet.View = Cam�raJeu.Vue;
               //effet.World = mondeLocal;
            }
            maille.Draw();
         }
      }

      public virtual Matrix GetMonde()
      {
         return Monde;
      }

      public void SetCam�ra(Cam�ra cam)
      {
         Cam�raJeu = cam;
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
