using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    public interface IArme
    {
        void Attaquer();
        void Recharger();

        void ChangerPosition(IPositionable i);
        void ChangerRotation(IPositionable i);

    }
}
