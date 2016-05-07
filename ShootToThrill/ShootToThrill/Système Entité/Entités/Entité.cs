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
   public enum TypeEntité { Entité, Avatar, Joueur, Ennemi, Boss, Fusil, Munition}

   public class Entité : Microsoft.Xna.Framework.DrawableGameComponent
   {
      public const int ID_AUCUN_PROPRIÉTAIRE = -1;
      public int IdPropriétaire { get; set; }

      protected Options OptionsJeu { get; set; }
      EntitySystem système { get; set; }
      public int UniqueId { get; private set; }
      public TypeEntité TypeEnt { get; protected set; }

      public Entité(Game game)
         : base(game)
      {
         TypeEnt = TypeEntité.Entité;
         OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
         IdPropriétaire = ID_AUCUN_PROPRIÉTAIRE;
      }

      public override void Initialize()
      {
         système = Game.Services.GetService(typeof(EntitySystem)) as EntitySystem;
         UniqueId = système.ObtenirId(this);

         base.Initialize();
      }
   }
}
