using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ProjetPrincipal.Data
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DescriptionFusil
    {
        public const int MUNITION_MIN = 0,
                         UNE_MUNITION = 1;
        public bool AMunitionInfini { get; set; }
        public int MunitionTotalRestant { get; set; }
        public int MunitionTotalMax { get; set; }
        public int MunitionMaxDansChargeur { get;  set; }
        public int MunitionRestantDansChargeur { get;  set; }
        public int MunitionDansCaisseMunitions { get;  set; }
        public int MunitionInitial { get;  set; }
        public float IntervalRechargement { get;  set; }
        public string NomArme { get;  set; }
        public string NomModèle { get; set; }
        public float Cadence { get;  set; }
        public int Dommage { get;  set; }
        public float Portée { get;  set; }
        public float AngleDeTir { get;  set; }
        public bool Area { get;  set; }
        public int NbBallesParTir { get; set; }
    }
}
