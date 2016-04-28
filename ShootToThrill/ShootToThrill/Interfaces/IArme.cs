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

        void SetPosition(IPositionable i);
        void SetRotation(IPositionable i);

    }
}
