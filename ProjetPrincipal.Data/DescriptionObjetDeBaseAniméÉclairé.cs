using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjetPrincipal.Data
{
    public class DescriptionObjetDeBaseAniméÉclairé
    {
        public string NomModèle { get; set; }
        public string NomTextureModèle { get; set; }
        public string NomEffetAffichage { get; set; }
        public float Échelle { get; set; }
        public Vector3 Rotation { get; set; }
    }
}
