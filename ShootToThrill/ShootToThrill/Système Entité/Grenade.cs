using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{

   public class Grenade : Entité, IPhysique, IModele3d
   {
      IModele3d ComposanteGraphique { get; set; }
      ObjetPhysique ComposantePhysique { get; set; }

      const float MASSE_INVERSE_GRENADE = 1/2f;
      const float RAYON_GRENADE = 1.25f;
      const float RAYON_EXPLOSION = 2f;

      //, position, vitesse, MASSE_INVERSE_GRENADE, new SphereCollision(position,RAYON_GRENADE)
      public Grenade(Game game, Vector3 position, Vector3 vitesse, string NomModèle)
         : base(game)
      {
         ComposanteGraphique = new MObjetDeBase(game, NomModèle, 1f, Vector3.Zero, position);
         ComposantePhysique = new ObjetPhysique(game, position);//new ObjetPhysique(game, position, vitesse, MASSE_INVERSE_GRENADE);
      }

      public override void Initialize()
      {
         ComposanteGraphique.Initialize();
         ComposantePhysique.Initialize();

         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         ComposanteGraphique.SetPosition(ComposantePhysique.Position);
         ComposantePhysique.Update(gameTime);

         base.Update(gameTime);
      }

      public ObjetPhysique GetObjetPhysique()
      {
         return ComposantePhysique;
      }
      public Collider GetCollider()
      {
         return ComposantePhysique.GetCollider();// new SphereCollision(ComposantePhysique.Position, RAYON_GRENADE);
      }
      public void SetCaméra(Caméra cam)
      {
         ComposanteGraphique.SetCaméra(cam);
      }
      public void SetPosition(Vector3 position)
      {
         ComposanteGraphique.SetPosition(position);
         ComposantePhysique.SetPosition(position);
      }
      public void SetRotation(Vector3 rotation)
      {
          ComposanteGraphique.SetRotation(rotation);
          ComposantePhysique.SetRotation(rotation);
      }
      public override void Draw(GameTime gameTime)
      {
         ComposanteGraphique.Draw(gameTime);
         ComposantePhysique.Draw(gameTime);
         base.Draw(gameTime);
      }

   }
}
