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
    public class Jeu : DrawableGameComponent, IModele3d
    {
        Round Round { get; set; }
        public Portail PortailJoueur { get; private set; }
        public Portail PortailEnnemi { get; private set; }
        float TempsDepuisDernierRound { get; set; }
        float IntervalEntreRound { get; set; }
        MoteurPhysique MoteurPhysique { get; set; }
        Interrupteur Interrupteur { get; set; }
        public List<Item> ListeItem { get; private set; }
        int NbJoueur { get; set; }
        public Jeu(Game game, int nbJoueur)
            : base(game)
        {
            NbJoueur = nbJoueur;
        }

        public void SetPosition(Vector3 position)
        {

        }

        public void SetRotation(Vector3 rotation)
        {

        }

        public override void Initialize()
        {
            ListeItem = new List<Item>();
            Round = new Round(Game, NbJoueur);
            Round.Initialize();
            TempsDepuisDernierRound = 0;
            IntervalEntreRound = 10;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MoteurPhysique = Game.Services.GetService(typeof(MoteurPhysique)) as MoteurPhysique;
            base.LoadContent();
        }

        public void SetPositionInterrupteur(Vector3 position)
        {
            Interrupteur = new Interrupteur(Game, position, 1f, "Interrupteur", 0.05f);
            Interrupteur.Initialize();
            ListeItem.Add(Interrupteur);
        }

        public void SetPositionPortailJoueur(Vector3 position)
        {
            PortailJoueur = new Portail(Game, position, 0, "PortailMonstre", 0.05f);
            PortailJoueur.Initialize();
            ListeItem.Add(PortailJoueur);
        }

        public void SetPositionPortailEnnemi(Vector3 position)
        {
            PortailEnnemi = new Portail(Game, position, 0, "PortailMonstre", 0.05f);
            PortailEnnemi.Initialize();
            ListeItem.Add(PortailEnnemi);
            Round.SetPositionPortailEnnemi(position);
        }

        public override void Update(GameTime gameTime)
        {
            UpdaterEnnemi(gameTime);
            UpdaterJoueur(gameTime);
            UpdaterInterrupteur(gameTime);
            //Round.Update(gameTime);
            base.Update(gameTime);
        }

        void UpdaterEnnemi(GameTime gameTime)
        {
            if (Round.EnnemisTousCréés)
            {
                TempsDepuisDernierRound += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsDepuisDernierRound >= IntervalEntreRound)
                {
                    if (Round.AucunEnnemi)
                    {
                        PortailEnnemi.ActiverItem();
                        Round.ResetVariable();
                        Round.Enabled = true;
                    }
                    else if (PortailEnnemi.EstPortailActif)
                    {
                        PortailEnnemi.DésactiverItem();
                    }
                    TempsDepuisDernierRound = 0;
                }
            }
        }

        void UpdaterJoueur(GameTime gameTime)
        {
            if (Round.NombreRound % 5 == 0)
            {
                PortailJoueur.ActiverItem();
            }
            else if (PortailJoueur.EstPortailActif)
            {
                PortailJoueur.DésactiverItem();
            }
        }

        void UpdaterInterrupteur(GameTime gameTime)
        {
            if (Round.NombreRound % 10 == 0)
            {
                Interrupteur.ActiverItem();
            }
            else if (Interrupteur.EstInterrupteurActif)
            {
                Interrupteur.DésactiverItem();
            }
        }

        public void SetCaméra(Caméra cam)
        {
            foreach (Item i in ListeItem)
            {
                i.SetCaméra(cam);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Item i in ListeItem)
            {
                i.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}