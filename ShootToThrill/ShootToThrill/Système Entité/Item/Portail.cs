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
    public class Portail : Item
    {
        const int NB_JOUEUR_MIN = 2;
        public bool EstPortailActif { get; private set; }
        float TempsDepuisMAJ { get; set; }
        float IntervalMAJ { get; set; }
        Lumière LumièreJeu { get; set; }
        MMoteurPhysique MMoteurPhysique { get; set; }
        InformationGame InformationGame { get; set; }
        List<MJoueur> ListeMJoueur { get; set; }
        int Cpt { get; set; }
        public Portail(Game game, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ)
            : base(game, positionInitiale, rayon, nomModèle, intervalMAJ, false)
        {
        }

        public override void Initialize()
        {
            EstPortailActif = false;
            TempsDepuisMAJ = 0;
            IntervalMAJ = 1f;
            ListeMJoueur = new List<MJoueur>();
            Cpt = 1;
            LumièreJeu = new Lumière(Game, Vector3.Zero, Color.Red.ToVector3(), 1, 1, Vector3.One, Vector4.One);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            InformationGame = Game.Services.GetService(typeof(InformationGame)) as InformationGame;
            MMoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
            base.LoadContent();
        }

        public override void ActiverItem()
        {
            Cpt = 1;
            ListeMJoueur = new List<MJoueur>();
            EstPortailActif = true;
        }

        public override void DésactiverItem()
        {
            EstPortailActif = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (EstPortailActif)
            {
                TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsDepuisMAJ >= IntervalMAJ)
                {
                    CréerJoueur();
                    TempsDepuisMAJ = 0;
                    ++Cpt;
                }
            }
 	        base.Update(gameTime);
        }

        void CréerJoueur()
        {
            foreach (MJoueur j in MMoteurPhysique.ListePhysique)
            {
                if (j.EstMort && Cpt == (int)j.IndexJoueur && InformationGame.NBJoueur >= NB_JOUEUR_MIN)
                {
                    j.Respawn();
                    //    MObjetDeBaseAniméEtÉclairé o = new MObjetDeBaseAniméEtÉclairé(Game, "ship2", "RectangleBleu", 0.001f, Vector3.Zero, Vector3.One, "Spotlight", LumièreJeu, 1 / 60f);
                    //    ListeMJoueur.Add(new MJoueur(Game, o, new ObjetPhysique(Game, Position), (PlayerIndex)Cpt));
                }
            }
        }
    }
}
