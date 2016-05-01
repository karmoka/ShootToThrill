﻿// Auteur :       Raphael Croteau
// Fichier :      ModelPhysique.cs
// Description :  !!!Désuet!!! un modèle qui répond à F = ma
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
   public class ModelPhysique : ObjetPhysique, IModele3d, IPositionable
   {
       float RotationY { get; set; }

      string NomModèle { get; set; }
      public MObjetDeBase Modèle { get; set; }
      public float Rayon { get; private set; }

      public ModelPhysique(Game game, Vector3 position, Vector3 vitesse, float masse, float rayon, string nomModèle)
         : base(game, position, vitesse, masse)
      {
         Rayon = rayon;
         NomModèle = nomModèle;
      }
      public ModelPhysique(Game game, DescriptionModelPhysique description)
          : base(game, description)
      {
          Rayon = description.Rayon;
          NomModèle = description.NomModèle;
      }

      public ModelPhysique(Game game, Vector3 position, float rayon, string nomModèle)
          : base(game, position)
      {
          Rayon = rayon;
          NomModèle = nomModèle;
      }

      public override void Initialize()
      {
         RotationY = 0;
         Modèle = new MObjetDeBase(Game, NomModèle, 0.1f, Vector3.Zero, this.Position);
         Modèle.Initialize();
         base.Initialize();
      }

      protected override void LoadContent()
      {
          base.LoadContent();
      }

       public void RotationSurY(float rotation)
      {
          RotationY = rotation;
          //Modèle.SetRotationY(rotation);

      }

      public override Collider GetCollider()
      {
         return new SphereCollision(Position, Rayon);
      }
      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
          Modèle.SetPosition(Position);
         Modèle.Draw(gameTime);
         base.Draw(gameTime);
      }

      public void SetCaméra(AtelierXNA.Caméra cam)
      {
         Modèle.SetCaméra(cam);
      }
      public void ChangerCouleur(Color couleur)
      {
          Modèle.ChangerCouleur(couleur);
      }
      public override void SetRotation(Vector3 rotation)
      {
          Modèle.SetRotation(rotation);
          base.SetRotation(rotation);
      }
      public override void SetPosition(Vector3 position)
      {
          Modèle.SetPosition(position);

          base.SetPosition(position);
      }
   }
}
