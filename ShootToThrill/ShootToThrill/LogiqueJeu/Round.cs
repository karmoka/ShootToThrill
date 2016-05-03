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
        MMoteurPhysique MMoteurPhysique { get; set; }
        ModelManager Mod�leManager { get; set; }
        int NbJoueur { get; set; }
        Vector3 PositionPortailEnnemi { get; set; }
        public int NombreRound { get; private set; }
        int CptEnnemi { get; set; }
        float TempsDepuisDernierEnnemi { get; set; }
        float IntervalApparitionEnnemi
        {
            get
            {
                return 0.5f * TimeToCompletRound / NombreEnnemi;
            }
        }
        float TimeToCompletRound
        {
            get
            {
                return 30 * NombreRound;
            }
        }
        int NombreEnnemi
        { 
            get
            {
                return (int)Math.Pow(NombreRound, 2) + 2 * NbJoueur * NombreRound;
            }
        }
        public bool AucunEnnemi
        {
            get
            {
                return MMoteurPhysique.ListePhysique.Count(x => x is MEnnemi) == 0;
            }
        }
        public bool EnnemisTousCr��s
        {
            get
            {
                return CptEnnemi == NombreEnnemi;
            }
        }

        public Round(Game game, int nbJoueur)
            : base(game)
        {
            NbJoueur = nbJoueur;
        }

        public override void Initialize()
        {
            Enabled = true;
            NombreRound = 1;
            TempsDepuisDernierEnnemi = 0;
            CptEnnemi = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MMoteurPhysique = Game.Services.GetService(typeof(MMoteurPhysique)) as MMoteurPhysique;
            Mod�leManager = Game.Services.GetService(typeof(ModelManager)) as ModelManager;
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
        }

        void NouvelEnnemi()
        {
            Random G�n�rateurObjetDrop = new Random();
            if (NombreRound % 10 == 0 && CptEnnemi == NombreEnnemi - 1)
            {
                Cr�erEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Boss1"), 0);
            }
            else if (NombreRound % 5 == 0 && CptEnnemi % 5 == 0)
            {
                int fusilDrop = G�n�rateurObjetDrop.Next(0, Math.Min(NombreRound / 5 * 10, 41));
                Cr�erEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Boss0"), fusilDrop);
            }
            else
            {
                int itemDrop = 4; //G�n�rateurObjetDrop.Next(0, 20);
                switch (G�n�rateurObjetDrop.Next(0, 1))//3))
                {
                    case 0:
                        Cr�erEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi1"), itemDrop);
                        break;
                    case 1:
                        Cr�erEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi2"), itemDrop);
                        break;
                    case 2: 
                        Cr�erEnnemi(Game.Content.Load<DescriptionEnnemi>("Description/Ennemi3"), itemDrop);
                        break;
                }
            }
        }

        void Cr�erEnnemi(DescriptionEnnemi description, int itemDrop)
        {
            Lumi�re lumi�reJeu = new Lumi�re(Game, Vector3.Zero, Color.Red.ToVector3(), 1, 1, Vector3.One, Vector4.One);
            MObjetDeBaseAnim�Et�clair� m = new MObjetDeBaseAnim�Et�clair�(Game, description.NomEnnemi, "RectangleBleu", 0.5f, Vector3.Zero, Vector3.One, "Spotlight", lumi�reJeu, 1 / 60f);
            ObjetPhysique o = new ObjetPhysique(Game, PositionPortailEnnemi, Vector3.Zero, description.MasseInverse);
            MEnnemi unEnnemi = new MEnnemi(Game, m, o, itemDrop, description.VieMax, description.Domage);
            unEnnemi.Initialize();
            Mod�leManager.AjouterModele(unEnnemi);
            MMoteurPhysique.AjouterObjet(unEnnemi);
        }
    }
}