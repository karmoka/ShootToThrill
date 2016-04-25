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
        Portail PortailJoueur { get; set; }
        Portail PortailEnnemi { get; set; }
        float TempsDepuisDernierRound { get; set; }
        float IntervalEntreRound { get; set; }
        MoteurPhysique MoteurPhysique { get; set; }
        SupportFusil SupportFusilBazooka { get; set; }
        SupportFusil SupportFusilRailgun { get; set; }
        SupportFusil SupportFusilMachinegun { get; set; }
        SupportFusil SupportFusilShotgun { get; set; }
        SupportFusil SupportFusilPistol { get; set; }
        Interrupteur Interrupteur { get; set; }
        public List<Item> ListeItem { get; private set; }
        public Jeu(Game game)
            : base(game)
        {
        }
        public void SetPosition(Vector3 position)
        {

        }

        public override void Initialize()
        {
            ListeItem = new List<Item>();
            Round = new Round(Game);
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
            PortailJoueur = new Portail(Game, new Vector3(30, -8, 30), 0, "PortailJoueur", 0.05f);
            PortailJoueur.Initialize();
            PortailJoueur.SetPosition(position);
            ListeItem.Add(PortailJoueur);
        }

        public void SetPositionPortailEnnemi(Vector3 position)
        {
            PortailEnnemi = new Portail(Game, new Vector3(30, -8, 30), 0, "PortailMonstre", 0.05f);
            PortailEnnemi.Initialize();
            PortailEnnemi.SetPosition(position);
            ListeItem.Add(PortailEnnemi);
        }

        public void SetPositionSupportFusilBazooka(Vector3 position)
        {
            Fusil bazooka = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Bazooka"), position + Vector3.Up, 0.005f, 0.02f);
            SupportFusilBazooka = new SupportFusil(Game, position, 0.2f, "SupportFusil", 0.05f, bazooka);
            SupportFusilBazooka.Initialize();
            ListeItem.Add(SupportFusilBazooka);
            ListeItem.Add(bazooka);
            //SupportFusilBazooka.ChangerFusil(bazooka);
        }

        public void SetPositionSupportFusilRailgun(Vector3 position)
        {
            Fusil railgun = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Railgun"), position + Vector3.Up, 0.005f, 0.02f);
            SupportFusilRailgun = new SupportFusil(Game, position, 0.2f, "SupportFusil", 0.05f, railgun);
            SupportFusilRailgun.Initialize();
            ListeItem.Add(SupportFusilRailgun);
            ListeItem.Add(railgun);
            //SupportFusilRailgun.ChangerFusil(railgun);
        }

        public void SetPositionSupportFusilMachinegun(Vector3 position)
        {
            Fusil machinegun = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Machinegun"), position + Vector3.Up, 0.005f, 0.02f);
            SupportFusilMachinegun = new SupportFusil(Game, position, 0.2f, "SupportFusil", 0.05f, machinegun);
            SupportFusilMachinegun.Initialize();
            ListeItem.Add(SupportFusilMachinegun);
            ListeItem.Add(machinegun);
            //SupportFusilMachinegun.ChangerFusil(machinegun);
        }

        public void SetPositionSupportFusilShotgun(Vector3 position)
        {
            Fusil shotgun = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Shotgun"), position + Vector3.Up, 0.005f, 0.02f);
            SupportFusilShotgun = new SupportFusil(Game, position, 0.2f, "SupportFusil", 0.05f, shotgun);
            SupportFusilShotgun.Initialize();
            ListeItem.Add(SupportFusilShotgun);
            ListeItem.Add(shotgun);
            //SupportFusilShotgun.ChangerFusil(shotgun);
        }

        public void SetPositionSupportFusilPistol(Vector3 position)
        {
            Fusil pistol = new Fusil(Game, Game.Content.Load<DescriptionFusil>("Description/Pistol"), position + Vector3.Up, 0.005f, 0.02f);
            SupportFusilPistol = new SupportFusil(Game, position, 0.2f, "SupportFusil", 0.05f, pistol);
            SupportFusilPistol.Initialize();
            ListeItem.Add(SupportFusilPistol);
            ListeItem.Add(pistol);
            //SupportFusilPistol.ChangerFusil(pistol);
        }

        public override void Update(GameTime gameTime)
        {
            if (Round.EnnemisTousCréés)
            {
                TempsDepuisDernierRound += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TempsDepuisDernierRound >= IntervalEntreRound)
                {
                    if (Round.AucunEnnemi)
                    {
                        ActiverPortail(PortailEnnemi);
                        Round.ResetVariable();
                        Round.Enabled = true;
                    }
                    else
                    {
                        DésactiverPortail(PortailEnnemi);
                    }
                    TempsDepuisDernierRound = 0;
                }
            }
            if (Round.NombreRound % 10 == 0)
            {
                Interrupteur.ActiverItem();
            }
            //Round.Update(gameTime);
            base.Update(gameTime);
        }

        void ActiverPortail(Portail portail)
        {
            portail.ActiverItem();
        }

        void DésactiverPortail(Portail portail)
        {
            portail.DésactiverItem();
        }

        void ActiverSupportFusil(SupportFusil supportFusil, Fusil fusil)
        {
            supportFusil.ActiverItem();
            ListeItem.Add(fusil);
            MoteurPhysique.AjouterObjet(fusil);
        }

        void DésactiverSupportFusil(SupportFusil supportFusil, Fusil fusil)
        {
            supportFusil.DésactiverItem();
            ListeItem.Remove(fusil);
            MoteurPhysique.EnleverObjet(fusil);
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