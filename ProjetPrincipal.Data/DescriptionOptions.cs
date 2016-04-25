using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetPrincipal.Data
{
    public class DescriptionOptions
    {
        public float IntervalMAJStandard { get;  set; }

        public int WindowWidth { get;  set; }
        public int WindowHeight { get;  set; }

        public float CameraHeightOffset { get;  set; }
        public float CameraDistanceOffset { get;  set; }

        public int ViePersonnageMax { get;  set; }

        public float SensibilitéFriction { get;  set; }
        public float RatioDistance3dMetre { get;  set; } //5 unité de distance par metre
        public Vector3 Gravité { get;  set; }

        public string NomFontMenu { get;  set; }
    }
}
