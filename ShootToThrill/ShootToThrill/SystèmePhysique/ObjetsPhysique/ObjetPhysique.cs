// Auteur :       Raphael Croteau
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

      const float RAYON_DÉFAUT = 1.25f;

       public List<IPhysique> ListeCollision { get; private set; }
       protected Options OptionJeu { get; private set; }

       public bool EstTangible { get; protected set; }
       private Vector3 _position;
       public Vector3 Position
       {
           get { return _position; }
           private set
           {
               _position = value;
               if (float.IsNaN(value.X))
                   throw new NotFiniteNumberException();
           }
       }
       private Vector3 Rotation { get; set; }
      protected bool EstImmuable { get; set; }
      public Vector3 Vitesse { get; protected set; }
      private Vector3 Accélération { get; set; }
      private Vector3 ForceRésultante { get; set; }
      private List<Vector3> Forces { get; set; }
      private Vector3 ForceGravitationnelle { get; set; }
      protected float MasseInverse { get; private set; }
      private Collider VolumeDeCollision { get; set; }

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
         :base(game)
      {
         Position = position;
         Vitesse = vitesse;
         MasseInverse = masseInverse;
         VolumeDeCollision = new SphereCollision(this.Position, RAYON_DÉFAUT);

         EstImmuable = false;
      }

      public ObjetPhysique(Game game, DescriptionObjetPhysique description)
          : base(game)
      {
          Position = description.Position;
          Vitesse = description.Vitesse;
          MasseInverse = description.MasseInverse;
          VolumeDeCollision = new SphereCollision(this.Position, RAYON_DÉFAUT);

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
         ListeCollision = new List<IPhysique>();
          EstTangible = true;
          OptionJeu = Game.Services.GetService(typeof(Options)) as Options;

          VolumeDeCollision.Initialize();
          ForceGravitationnelle = OptionJeu.Gravité;
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
         if(MasseInverse != 0)
         {
            CalculerForceRésultantes();
            Accélération = ForceRésultante * MasseInverse;
            Vitesse += Accélération * deltaT;

            Position += Vitesse * deltaT;
         }

         ListeCollision.Clear();
      }

       void CalculerForceRésultantes()
       {
          ForceRésultante = Vector3.Zero;
           foreach(Vector3 v in Forces)
           {
               ForceRésultante += v;
           }

           ForceRésultante += ForceGravitationnelle;

           Forces.Clear();
       }

       public void AjouterForce(Vector3 force)
       {
          if(MasseInverse != 0)
            Forces.Add(force);
       }

       public virtual Collider GetCollider()
       {
          return new SphereCollision(this.Position, 2);
       }

      public virtual void EnCollision(IPhysique autre)
      {
         ListeCollision.Add(autre);

         if(this.EstTangible && autre.GetObjetPhysique().EstTangible)
         {
            Vector3 norm = autre.GetCollider().Normale(this.Position);
            //La norme est corrigé pour gérer la collision des deux bords de l'objet
            if (Vector3.Dot((autre.GetCollider().Center - this.Position), norm) >= 0)
               norm = -norm;

            this.SetVitesse(CustomMathHelper.Réfléchir(this.Vitesse, norm) * 0.95f);

            CorrigerPosition(this, autre.GetObjetPhysique(), norm);
         }
         if(autre is VolumeDeForce)
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

      void CorrigerPosition(ObjetPhysique A, ObjetPhysique B, Vector3 normale)
      {
         this.SetPosition(this.Position + normale * 0.01f * this.MasseInverse);
      }
   }
}
