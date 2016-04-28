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
    class EnnemiScreenManager : DrawableGameComponent
    {
        const float MARGE = 0.05f,
                    LONGUEUR = 1f,
                    HAUTEUR = 0.2f,
                    LARGEUR = 0;
        const int NB_COULEUR = 2;
        RectangleVieColoré RectangleVieColoré { get; set; }
        Ennemi Ennemi { get; set; }
        public EnnemiScreenManager(Game game, Ennemi ennemi)
            : base(game)
        {
            Ennemi = ennemi;
        }

        public override void Initialize()
        {
            base.Initialize();
            Color[] tableauCouleur = new Color[NB_COULEUR] { Color.Red, Color.Black };
            RectangleVieColoré = new RectangleVieColoré(Game, 1, Vector3.Zero, Ennemi.Position + 0.5f * Vector3.Up, tableauCouleur, new Vector3(LONGUEUR, HAUTEUR, LARGEUR), new Vector3(LONGUEUR - 2 * MARGE, HAUTEUR - 2 * MARGE, LARGEUR), 0.5f);
            RectangleVieColoré.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            RectangleVieColoré.InitialiserSommets(Ennemi.Position + 0.5f * Vector3.Up, (LONGUEUR - MARGE) * Ennemi.Vie / Ennemi.VieMax);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            RectangleVieColoré.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void ChangerCaméra(Caméra cam)
        {
            RectangleVieColoré.SetCaméra(cam);
        }
    }
}