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

   public class Grenade : EntitéGraphiqueEtPhysique
   {
       const string TEXTURE_GRENADE = "tile_12";

      const float MASSE_INVERSE_GRENADE = 1/2f;
      const float RAYON_GRENADE = 1.25f;
      const float RAYON_EXPLOSION = 2f;

      //, position, vitesse, MASSE_INVERSE_GRENADE, new SphereCollision(position,RAYON_GRENADE)
      public Grenade(Game game, Vector3 position, Vector3 vitesse, string NomModèle)
         : base(game, new MObjetDeBaseAniméEtÉclairé(game,NomModèle,TEXTURE_GRENADE,1f,Vector3.Zero,position,"Spotlight",new Lumière(game),1/60),new ObjetPhysique(game,position))
      {

      }

      public override void Initialize()
      {


         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         ComposanteGraphique.SetPosition(ComposantePhysique.Position);
         ComposantePhysique.Update(gameTime);

         GérerCollisions();

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
      protected void GérerCollisions()
      {
          foreach (IPhysique i in ComposantePhysique.ListeCollision)
          {
              if(i is CubeAdditionnable)
              {
                  TacheAcide a = new TacheAcide(Game, "Acid", this.Position, new Vector2(3, 3), 1);
                  a.Initialize();
                  this.Dispose();
              }
          }
      }
     
      public override void Draw(GameTime gameTime)
      {
         ComposanteGraphique.Draw(gameTime);
         ComposantePhysique.Draw(gameTime);
         base.Draw(gameTime);
      }

   }
}
