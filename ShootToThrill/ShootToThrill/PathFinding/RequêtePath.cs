using System;
using System.Collections;
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
using System.Diagnostics;

namespace AtelierXNA
{
    public struct RequêtePath
    {
        public Vector3 DébutPath;
        public Vector3 FinPath;
        public Action<Vector3[], bool> CallBack;

        public RequêtePath(Vector3 débutPath, Vector3 finPath, Action<Vector3[], bool> callBack)
        {
            DébutPath = débutPath;
            FinPath = finPath;
            CallBack = callBack;
        }
    }
}
