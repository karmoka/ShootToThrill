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
    public class Item : ModelPhysique
    {
        float TempsDepuisMAJ { get; set; }
        float IntervalMAJ { get; set; }
        
        //public Item(Game game, string nomMod�le, float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalMAJ)
        //    : base(game, nomMod�le, �chelleInitiale, rotationInitiale, positionInitiale)
        //{
        //    IntervalMAJ = intervalMAJ;
        //}

        public Item(Game game, Vector3 positionInitiale, float rayon, string nomMod�le, float intervalMAJ)
            : base(game, positionInitiale, rayon, nomMod�le)
        {
            IntervalMAJ = intervalMAJ;
        }

        public override void Initialize()
        {
            TempsDepuisMAJ = 0;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsDepuisMAJ >= IntervalMAJ)
            {
                G�rerAnimation();
                TempsDepuisMAJ = 0;
            }

            base.Update(gameTime);
        }
        protected virtual void G�rerAnimation()
        {
            
        }

        public virtual void ActiverItem()
        {

        }

        public virtual void D�sactiverItem()
        {

        }
    }
}
