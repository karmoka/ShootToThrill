using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetPrincipal.Data
{
    public class DescriptionObjetPhysique
    {
        public float MasseInverse { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Vitesse { get; set; }
        public float Charge { get; set; }
        public bool EstImmuable { get; set; }
    }
}
