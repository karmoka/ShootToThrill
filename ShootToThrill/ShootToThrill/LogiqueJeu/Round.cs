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
    public class Round : DrawableGameComponent
    {
        MMoteurPhysique MoteurPhysique { get; set; }
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
        Vector3 PositionPortailEnnemi { get; set; }

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
            Difficulté = 0.1f * (Game.Services.GetService(typeof(InformationGame)) as InformationGame).NBJoueur;
            base.LoadContent();
        }

        public void SetPositionPortailEnnemi(Vector3 position)
        {
            PositionPortailEnnemi = position;
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
            if (NombreRound % 10 == 0 && CptEnnemi == NombreEnnemi - 1)
            {
                CréerEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Boss1"), 0);
            }
            else if (NombreRound % 5 == 0 && CptEnnemi % 5 == 0)
            {
                int fusilDrop = GénérateurObjetDrop.Next(0, Math.Min(NombreRound / 5 * 10, 41));
                CréerEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Boss0"), fusilDrop);
            }
            else
            {
                int itemDrop = GénérateurObjetDrop.Next(0, 20);
                switch (GénérateurObjetDrop.Next(0, 3))
                {
                    case 0:
                        CréerEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi1"), itemDrop);
                        break;
                    case 1:
                        CréerEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi2"), itemDrop);
                        break;
                    case 2: 
                        CréerEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi3"), itemDrop);
                        break;
                }
            }
        }

        void CréerEnnemi(DescriptionEnnemi description, int itemDrop)
        {
            Ennemi unEnnemi = new Ennemi(Game, PositionPortailEnnemi, Vector3.Zero, description, itemDrop);
            unEnnemi.Initialize();
        }
    }
}