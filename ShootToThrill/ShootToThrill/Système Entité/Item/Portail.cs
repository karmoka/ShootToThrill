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
        public Portail(Game game, Vector3 positionInitiale, float rayon, string nomMod�le, float intervalMAJ)
            : base(game, positionInitiale, rayon, nomMod�le, intervalMAJ, false)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void ActiverItem()
        {
            base.ActiverItem();
        }

        public override void D�sactiverItem()
        {
            base.D�sactiverItem();
        }
    }
}
