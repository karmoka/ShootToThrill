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
    public class SupportFusil : Item
    {
        public Fusil Fusil { get; private set; }
        public Fusil AncienFusil { get; private set; }
        Effect Lumière { get; set; }
        bool EstSupportFusilActivé
        {
            get
            {
                return Fusil != null;
            }
        }
        //public SupportFusil(Game jeu, string nomModèle, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalMAJ, Fusil fusil)
        //    : base(jeu, nomModèle, échelleInitiale, rotationInitiale, positionInitiale, intervalMAJ)
        public SupportFusil(Game jeu, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ, Fusil fusil)
            : base(jeu, positionInitiale, rayon, nomModèle, intervalMAJ)
        {
            Fusil = fusil;
        }

        public override void Initialize()
        {
            Fusil.Initialize();
            AncienFusil = Fusil;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void ChangerFusil(Fusil fusil)
        {
            Fusil = fusil;
            Fusil.Initialize();
            AncienFusil = Fusil;
        }

        protected override void GérerAnimation()
        {
            if(EstSupportFusilActivé)
            {
                //Activer la lumière
                Fusil.Animer();
            }
            else
            {
                //Désactivé la lumière
            }
        }

        public override void ActiverItem()
        {
            Fusil = AncienFusil;
        }

        public override void DésactiverItem()
        {
            Fusil = null;
        }
    }
}
