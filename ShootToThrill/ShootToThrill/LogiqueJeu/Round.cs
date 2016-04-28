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
        ModelManager ManagerMod�le { get; set; }
        MMoteurPhysique MoteurPhysique { get; set; }
        Options Options { get; set; }
        int NombreJoueur
        {
            get
            {
                return MoteurPhysique.ListePhysique.Count(x => x is Joueur);
            }
        }
        float NombreEnnemi { get; set; }
        float Difficult� { get; set; }
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
        public bool EnnemisTousCr��s
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
            D�terminerVitesseApparition();
            TempsDepuisDernierEnnemi = 0;
            CptEnnemi = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ManagerMod�le = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
            MoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
            Options = Game.Services.GetService(typeof(Options)) as Options;
            Difficult� = 0.1f * NombreJoueur;
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
            D�terminerVitesseApparition();
        }

        public void AugmenterGrowRate()
        {
            Difficult� *= NombreEnnemi;
        }

        void AugmenterNombreEnnemi()
        {
            if (NombreEnnemi < Thresh)
            {
                NombreEnnemi = (float)Math.Ceiling(NombreEnnemi + Difficult�);
            }
            else
            {
                NombreEnnemi += (float)(1 / Math.Sqrt(NombreEnnemi));
            }
            //return (int)((0.15 * Round) * (24 + 6 * (NombreJoueur - 1)));
        }

        void D�terminerVitesseApparition()
        {
            IntervalApparitionEnnemi = 0.5f * TimeToCompletRound / NombreEnnemi;
        }

        void NouvelEnnemi()
        {
            Random G�n�rateurObjetDrop = new Random();
            Ennemi unEnnemi = null;
            if (NombreRound % 10 == 0 && CptEnnemi == NombreEnnemi - 1)
            {
                unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 10f, 4 / 5f, "Boss2", 1000, 15, 0);
            }
            else if (NombreRound % 5 == 0 && CptEnnemi % 5 == 0)
            {
                int fusilDrop = G�n�rateurObjetDrop.Next(0, Math.Min(NombreRound / 5 * 10, 41));
                unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 10f, 4 / 5f, "Boss", 300, 30, fusilDrop);
            }
            else
            {
                int itemDrop = G�n�rateurObjetDrop.Next(0, 20);
                switch (G�n�rateurObjetDrop.Next(0, 3))
                {
                    case 0:
                        unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 3f, 4 / 5f, "Ennemi1", 120, 8, itemDrop);
                        break;
                    case 1:
                        unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 6f, 4 / 5f, "Ennemi2", 100, 10, itemDrop);
                        break;
                    case 2:
                        unEnnemi = new Ennemi(Game, new Vector3(38, 1, 28), Vector3.Zero, 1 / 2f, 4 / 5f, "Ennemi3", 80, 5, itemDrop);
                        break;
                }
            }
            unEnnemi.Initialize();
            //MoteurPhysique.AjouterObjet(unEnnemi);
            //ManagerMod�le.AjouterModele(unEnnemi);
        }
    }
}