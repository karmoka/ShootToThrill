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
    class Interrupteur : Item
    {
        public bool EstInterrupteurActif { get; private set; }
        public Interrupteur(Game game, Vector3 positionInitiale, float rayon, string nomModèle, float intervalMAJ)
            : base(game, positionInitiale, rayon, nomModèle, intervalMAJ, false)
        {
        }

        public override void Initialize()
        {
            EstInterrupteurActif = false;
            base.Initialize();
        }

        public override void ActiverItem()
        {
            EstInterrupteurActif = true;
            base.ActiverItem();
        }

        public override void DésactiverItem()
        {
            EstInterrupteurActif = false;
            base.DésactiverItem();
        }

        public void ChangerGravité()
        {
            //OptionJeu.ChangerGravité();
            DésactiverItem();
        }
    }
}
