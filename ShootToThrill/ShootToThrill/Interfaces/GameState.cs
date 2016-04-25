using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    /// <summary>
    /// Décrit un state d'une state machine
    /// </summary>
    public interface GameState
    {
       bool EstActivé { get; set; }
       bool EstDétruit { get; set; }

       void Initialiser();
       void Cleanup();
       void Pause();
       void Résumer();

       void Update(GameTime gametime);
       void Draw(GameTime gametime, float ordre);
    }
}
