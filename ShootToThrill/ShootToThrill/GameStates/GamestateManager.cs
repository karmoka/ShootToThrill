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

namespace AtelierXNA 
{
    /// <summary>
    /// Gère les différentes gamestates, update et draw selon leurs états
    /// </summary>
   public class GameStateManager : DrawableGameComponent
   {
      List<GameState> PileGamestate { get; set; }

      public GameStateManager(Game game)
         :base(game)
      {
         PileGamestate = new List<GameState>();
      }

      public GameState Peek()
      {
         return PileGamestate[PileGamestate.Count - 1];
      }

      public void Push(GameState GameState)
      {
         if (PileGamestate.Count != 0)
            Peek().Pause();

         PileGamestate.Add(GameState);
         Peek().Initialiser();
      }

       public void ClearStates()
      {
         for (int i = 0; i < PileGamestate.Count; ++i)
         {
            PileGamestate[i].Cleanup();
         }

          PileGamestate.Clear();
      }

      public void Pop()
      {
         if (PileGamestate.Count != 0)
         {
            PileGamestate[PileGamestate.Count - 1].Cleanup();
            PileGamestate.RemoveAt(PileGamestate.Count - 1);
         }

         if (PileGamestate.Count != 0)
            Peek().Résumer();
      }

      public void Switch(GameState GameState)
      {
         Pop();
         Push(GameState);
         Peek().Initialiser();
      }

      public override void Draw(GameTime gameTime)
      {
         if (PileGamestate.Count != 0)
         {
            float ordre = 0;

            foreach (GameState g in PileGamestate)
            {
               g.Draw(gameTime, ordre);
               ordre += 0.1f;
            }
         }
      }

      public override void Update(GameTime gameTime)
      {
         //if (PileGamestate.Count != 0)
         //{
         //   foreach (GameState g in PileGamestate)
         //   {
         //      g.Update(gameTime);
         //   }
         //}
          for (int i = 0; i < PileGamestate.Count;++i)
          {
              PileGamestate[i].Update(gameTime);
          }

          PileGamestate.RemoveAll(x => x.EstDétruit == true);
      }
   }
}
