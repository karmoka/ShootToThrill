﻿// Auteur :       Raphael Croteau
// Fichier :      ObjetPhysique.cs
// Description :  Représente un objet physique répondant à F= ma. Une masse inverse de 0 indique une masse infini, donc aucun déplacement.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ProjetPrincipal.Data;

namespace AtelierXNA
{
   public class ObjetPhysique : DrawableGameComponent
   {
      Vector3 ATTÉNUATION_VERTICAL = new Vector3(1, 0.5f, 1);
      public List<ÉtatPhysique> ÉtatsPhysiques { get; set; }

      const float RAYON_DÉFAUT = 1.25f;
      private Collider VolumeDeCollision { get; set; }

      public List<IPhysique> ListeCollision { get; private set; }
      protected Options OptionJeu { get; private set; }

      public float Charge { get; set; }
      protected float MasseInverse { get; private set; }

      public bool EstTangible { get; set; }
      public bool EstImmuable { get; set; }

      public Vector3 Position { get; private set; }
      private Vector3 Rotation { get; set; }
      public Vector3 Vitesse { get; protected set; }
      private Vector3 Accélération { get; set; }

      private Vector3 ForceRésultante { get; set; }
      private List<Vector3> Forces { get; set; }
      private Vector3 ForceGravitationnelle { get { return OptionJeu.Gravité; } }

      public ObjetPhysique(Game game, Vector3 position, Vector3 vitesse, float masseInverse, Collider volumeCollision)
         : base(game)
      {
         Position = position;
         Vitesse = vitesse;
         MasseInverse = masseInverse;
         VolumeDeCollision = volumeCollision;

         EstImmuable = false;
      }

      public ObjetPhysique(Game game, Vector3 position, Vector3 vitesse, float masseInverse)
         : base(game)
      {
         Position = position;
         Vitesse = vitesse;
         MasseInverse = masseInverse;
         VolumeDeCollision = new SphereCollision(this.Position, RAYON_DÉFAUT);

         EstImmuable = false;
      }

      public ObjetPhysique(Game game, DescriptionObjetPhysique description, Vector3 position)
         : base(game)
      {
         Position = position;
         Vitesse = description.Vitesse;
         MasseInverse = description.MasseInverse;
         VolumeDeCollision = new SphereCollision(this.Position, RAYON_DÉFAUT);
         Charge = description.Charge;

         EstImmuable = description.EstImmuable;
      }

      public ObjetPhysique(Game game, Vector3 position)
         : base(game)
      {
         Position = position;
         Vitesse = Vector3.Zero;
         MasseInverse = 1f;
         EstImmuable = false;
         VolumeDeCollision = new SphereCollision(this.Position, RAYON_DÉFAUT);
      }

      public override void Initialize()
      {
         ÉtatsPhysiques = new List<ÉtatPhysique>();
         ÉtatsPhysiques.Add(new ÉtatPhysique(Game, 0, null));


         ListeCollision = new List<IPhysique>();
         EstTangible = true;
         OptionJeu = Game.Services.GetService(typeof(Options)) as Options;

         VolumeDeCollision.Initialize();
         Forces = new List<Vector3>();
         ForceRésultante = Vector3.Zero;

         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
      }

      public void Intégrer(float deltaT)
      {
         if (!EstImmuable && MasseInverse != 0)
         {
            CalculerForceRésultantes();
            Accélération = ForceRésultante * MasseInverse;
            Vitesse += Accélération * deltaT;

            Position += Vitesse * deltaT;
         }

         Forces.Clear();
         ListeCollision.Clear();
      }

      void CalculerForceRésultantes()
      {
         ForceRésultante = Vector3.Zero;
         foreach (Vector3 v in Forces)
         {
            ForceRésultante += v;
         }

         ForceRésultante += ForceGravitationnelle;

         Forces.Clear();
      }

      public void AjouterForce(Vector3 force)
      {
         if (MasseInverse != 0 && force != Vector3.Zero)
            Forces.Add(force);
      }

      public virtual Collider GetCollider()
      {
         return new SphereCollision(this.Position, 2);
      }

      public virtual void EnCollision(IPhysique autre)
      {
         ListeCollision.Add(autre);

         if (this.EstTangible && autre.GetObjetPhysique().EstTangible)
         {
            Vector3 norm = autre.GetCollider().Normale(this.Position);
            //La norme est corrigé pour gérer la collision des deux bords de l'objet
            if (Vector3.Dot((autre.GetCollider().Center - this.Position), norm) >= 0)
                norm = -norm;

            this.SetVitesse(CustomMathHelper.Réfléchir(this.Vitesse, norm) * ATTÉNUATION_VERTICAL * 0.95f);

            CorrigerPosition(norm);
         }
         if (autre is VolumeDeForce)
         {
            AjouterForce((autre as VolumeDeForce).GetForce(this.Position));
         }
      }

      public void SetVitesse(Vector3 vitesse)
      {
         Vitesse = vitesse;
      }

      public virtual void SetPosition(Vector3 nouvellePosition)
      {
         Position = nouvellePosition;
      }

      public virtual void SetRotation(Vector3 rotation)
      {
         Rotation = rotation;
      }

      void CorrigerPosition(Vector3 normale)
      {
         this.SetPosition(this.Position + normale * 0.01f * this.MasseInverse);
      }
   }
}
