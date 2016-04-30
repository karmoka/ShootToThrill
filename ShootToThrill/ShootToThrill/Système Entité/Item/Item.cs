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
        const float INTERVAL_DISPATITION = 10f;
        float TempsDepuisMAJ { get; set; }
        float IntervalMAJ { get; set; }
        float TempsDepuisApparition { get; set; }
        bool EstTemporaire { get; set; }
        
        //public Item(Game game, string nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalMAJ)
        //    : base(game, nomModèle, échelleInitiale, rotationInitiale, positionInitiale)
        //{
        //    IntervalMAJ = intervalMAJ;
        //}

        public Item(Game game, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ, bool estTemporaire)
            : base(game, positionInitiale, rayon, nomModèle)
        {
            IntervalMAJ = intervalMAJ;
            EstTemporaire = estTemporaire;
        }

        public override void Initialize()
        {
            TempsDepuisMAJ = 0;
            TempsDepuisApparition = 0;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            float tempsDepuisMAJ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsDepuisMAJ += tempsDepuisMAJ;
            TempsDepuisApparition += tempsDepuisMAJ;
            if (TempsDepuisMAJ >= IntervalMAJ)
            {
                GérerAnimation();
                if (EstTemporaire && TempsDepuisApparition >= INTERVAL_DISPATITION)
                {
                    DésactiverItem();
                }
                TempsDepuisMAJ = 0;
            }

            base.Update(gameTime);
        }
        protected virtual void GérerAnimation()
        {
            
        }

        public virtual void ActiverItem()
        {

        }

        public virtual void DésactiverItem()
        {
            //(Game.Services.GetService(typeof(MoteurPhysique)) as MoteurPhysique).EnleverObjet(this);
            //(Game.Services.GetService(typeof(ModelManager)) as ModelManager).EnleverModèle(this);
        }
    }
}
