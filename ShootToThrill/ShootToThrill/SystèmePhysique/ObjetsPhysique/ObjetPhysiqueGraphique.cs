// Auteur :       Raphael Croteau
// Fichier :      ModelPhysique.cs
// Description :  !!!Désuet!!! un modèle qui répond à F = ma
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
   public class ObjetPhysiqueGraphique : ObjetPhysique, IModele3d, IPositionable
   {
      RessourcesManager<Model> GestionnaireModele { get; set; }
      string NomModèle { get; set; }
      Vector3 DimensionCube { get; set; }

      public IModele3d Modèle3D { get; set; }
      public Collider Collision { get; set; }
      public float Rayon { get; private set; }

      public ObjetPhysiqueGraphique(Game game, Vector3 position, Vector3 vitesse, float masse, string nomModèle)
         : base(game, position, vitesse, masse)
      {
         NomModèle = nomModèle;
      }
      public ObjetPhysiqueGraphique(Game game, Vector3 position, Vector3 vitesse, float masse, Vector3 dimensionCube)
         : base(game, position, vitesse, masse)
      {

      }
      public ObjetPhysiqueGraphique(Game game, DescriptionModelPhysique description, Vector3 position)
          : base(game, description, position)
      {
          Rayon = description.Rayon;
          NomModèle = description.NomModèle;
      }

      public override void Initialize()
      {
         GestionnaireModele = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;

         if(NomModèle != "")
         {
            MObjetDeBase o = new MObjetDeBase(Game, NomModèle, 1f, Vector3.Zero, this.Position);
            Modèle3D = o as IModele3d;
            Collision = new CubeCollision(this.Position, DimensionModele(GestionnaireModele.Find(NomModèle), Matrix.Identity), Vector3.Zero);
         }
         else
         {
            Modèle3D = new CubeColoré(Game, 1, Vector3.Zero, this.Position, Color.White, DimensionCube, OptionJeu.IntervalMAJStandard);
            Collision = new CubeCollision(this.Position, this.DimensionCube, Vector3.Zero);
         }

         Modèle3D.Initialize();

         base.Initialize();
      }

      protected override void LoadContent()
      {

          base.LoadContent();
      }

      public override Collider GetCollider()
      {
         Collision.Center = this.Position;
         return Collision;
      }
      public override void Update(GameTime gameTime)
      {
         (Modèle3D as IPositionable).SetPosition(this.Position);
         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         Modèle3D.Draw(gameTime);

         base.Draw(gameTime);
      }

      public void SetCaméra(AtelierXNA.Caméra cam)
      {
         Modèle3D.SetCaméra(cam);
      }

      //Trouver ici http://gamedev.stackexchange.com/questions/2438/how-do-i-create-bounding-boxes-with-xna-4-0
      protected Vector3 DimensionModele(Model model, Matrix worldTransform)
      {
         // Initialize minimum and maximum corners of the bounding box to max and min values
         Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
         Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

         // For each mesh of the model
         foreach (ModelMesh mesh in model.Meshes)
         {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
               // Vertex buffer parameters
               int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
               int vertexBufferSize = meshPart.NumVertices * vertexStride;

               // Get vertex data as float
               float[] vertexData = new float[vertexBufferSize / sizeof(float)];
               meshPart.VertexBuffer.GetData<float>(vertexData);

               // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
               for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
               {
                  Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                  min = Vector3.Min(min, transformedPosition);
                  max = Vector3.Max(max, transformedPosition);
               }
            }
         }

         // Create and return bounding box
         return max-min;
      }
   }
}
