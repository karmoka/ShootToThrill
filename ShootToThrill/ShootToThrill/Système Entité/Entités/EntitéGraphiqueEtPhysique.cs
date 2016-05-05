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
using ProjetPrincipal.Data;


namespace AtelierXNA
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class EntitéGraphiqueEtPhysique : Entité, IPhysique, IModele3d
   {
      string NOM_TEXTURE_DÉFAUT = "tile_12";

      bool PhysiqueActivé;
      bool GraphiqueActivé;

      protected MMoteurPhysique MoteurPhysique { get; set; }
      protected ModelManager ManagerModele { get; set; }

      public IModele3d ComposanteGraphique { get; private set; }
      public ObjetPhysique ComposantePhysique { get; private set; }

      public EntitéGraphiqueEtPhysique(Game game,DescriptionAvatar description, Vector3 position)
         : base(game)
      {
         ComposantePhysique = new ObjetPhysique(game, description.DescriptionComposantePhysique, position);
         ComposanteGraphique = new MObjetDeBaseAniméEtÉclairé(game, description.DescriptionComposanteGraphique, position, OptionsJeu.IntervalMAJStandard);
      }
      public EntitéGraphiqueEtPhysique(Game game, string NomModèle, Vector3 position)
         : base(game)
      {
         ComposantePhysique = new ObjetPhysique(game, position);
         ComposanteGraphique = new MObjetDeBaseAniméEtÉclairé(game, NomModèle, NOM_TEXTURE_DÉFAUT, 0.1f, new Vector3(0,-MathHelper.PiOver2,0), position, "Spotlight", new Lumière(game), OptionsJeu.IntervalMAJStandard); 
      }

      public EntitéGraphiqueEtPhysique(Game game,IModele3d composanteGraphique, ObjetPhysique composantePhysique)
         : base(game)
      {
         ComposanteGraphique = composanteGraphique;
         ComposantePhysique = composantePhysique;
      }

      public override void Initialize()
      {
         PhysiqueActivé = false;
         GraphiqueActivé = false;

         base.Initialize();
      }

      protected override void LoadContent()
      {
         MoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
         ManagerModele = Game.Services.GetService(typeof(ModelManager)) as ModelManager;

         ComposanteGraphique.Initialize();
         ComposantePhysique.Initialize();

         base.LoadContent();

         ChangerÉtatGraphique();
         ChangerÉtatPhysique();
      }

      protected void ChangerÉtatPhysique()
      {
         PhysiqueActivé = !PhysiqueActivé;

         if(PhysiqueActivé)
         {
            MoteurPhysique.AjouterObjet(this);
         }
         else
         {
            MoteurPhysique.EnleverObjet(this);
         }
      }

      protected void ChangerÉtatGraphique()
      {
         GraphiqueActivé = !GraphiqueActivé;

         if (GraphiqueActivé)
         {
            ManagerModele.AjouterModele(this);
         }
         else
         {
            ManagerModele.EnleverModèle(this);
         }
      }
      public override void Update(GameTime gameTime)
      {
         ComposanteGraphique.SetPosition(Position);

         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         ComposanteGraphique.Draw(gameTime);
         ComposantePhysique.Draw(gameTime);

         base.Draw(gameTime);
      }

      public Vector3 Position
      {
         get { return ComposantePhysique.Position; }
      }

      public void SetPosition(Vector3 position)
      {
         ComposantePhysique.SetPosition(position);
      }
      public void SetRotation(Vector3 rotation)
      {
         ComposantePhysique.SetRotation(rotation);
         ComposanteGraphique.SetRotation(rotation);
      }
      public virtual void SetCaméra(Caméra caméra)
      {
         ComposanteGraphique.SetCaméra(caméra);
      }
      public ObjetPhysique GetObjetPhysique()
      {
         return ComposantePhysique;
      }
      public virtual Collider GetCollider()
      {
         return new SphereCollision(Position, 1f);
      }
   }
}
