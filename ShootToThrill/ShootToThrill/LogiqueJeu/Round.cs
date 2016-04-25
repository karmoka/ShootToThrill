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
    public class Round : DrawableGameComponent
    {
        ModelManager ManagerModèle { get; set; }
        MMoteurPhysique MoteurPhysique { get; set; }
        int NombreJoueur
        {
            get
            {
                return MoteurPhysique.ListePhysique.Count(x => x is Joueur);
            }
        }
        float NombreEnnemi { get; set; }
        float Difficulté { get; set; }
        int Thresh { get; set; }
        public int NombreRound { get; private set; }
        int CptEnnemi { get; set; }
        float TempsDepuisDernierEnnemi { get; set; }
        float IntervalApparitionEnnemi { get; set; }
        float TimeToCompletRound
        {
            get
            {
                return 30 * NombreRound;
            }
        }
        public bool AucunEnnemi
        {
            get
            {
                return MoteurPhysique.ListePhysique.Count(x => x is Ennemi) == 0;
            }
        }
        public bool EnnemisTousCréés
        {
            get
            {
                return CptEnnemi == NombreEnnemi;
            }
        }

        public Round(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            NombreRound = 1;
            NombreEnnemi = 10;
            Thresh = 80;
            DéterminerVitesseApparition();
            TempsDepuisDernierEnnemi = 0;
            CptEnnemi = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerModèle = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            MoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
            Difficulté = 0.1f * NombreJoueur;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                TempsDepuisDernierEnnemi += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsDepuisDernierEnnemi >= IntervalApparitionEnnemi)
                {
                    if (CptEnnemi < NombreEnnemi)
                    {
                        NouvelEnnemi();
                        ++CptEnnemi;
                    }
                    else
                    {
                        Enabled = false;
                    }
                    TempsDepuisDernierEnnemi = 0;
                }
            }
            base.Update(gameTime);
        }

        public void ResetVariable()
        {
            ++NombreRound;
            CptEnnemi = 0;
            AugmenterGrowRate();
            AugmenterNombreEnnemi();
            DéterminerVitesseApparition();
        }

        public void AugmenterGrowRate()
        {
            Difficulté *= NombreEnnemi;
        }

        void AugmenterNombreEnnemi()
        {
            if (NombreEnnemi < Thresh)
            {
                NombreEnnemi = (float)Math.Ceiling(NombreEnnemi + Difficulté);
            }
            else
            {
                NombreEnnemi += (float)(1 / Math.Sqrt(NombreEnnemi));
            }
            //return (int)((0.15 * Round) * (24 + 6 * (NombreJoueur - 1)));
        }

        void DéterminerVitesseApparition()
        {
            IntervalApparitionEnnemi = 0.5f * TimeToCompletRound / NombreEnnemi;
        }

        void NouvelEnnemi()
        {
            Random GénérateurObjetDrop = new Random();
            int itemDrop = GénérateurObjetDrop.Next(0, 20);
            int fusilDrop = GénérateurObjetDrop.Next(30, 40);
            Ennemi unEnnemi = null;
            if (NombreRound % 5 == 0 && CptEnnemi % 5 == 0)
            {
                unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 10f, 4 / 5f, "Boss", 300, 30, fusilDrop);
            }
            else
            {
                unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 2f, 4 / 5f, "Scene2", 100, 5, itemDrop);
            }
            unEnnemi.Initialize();
            //MoteurPhysique.AjouterObjet(unEnnemi);
            //ManagerModèle.AjouterModele(unEnnemi);
        }
    }
}