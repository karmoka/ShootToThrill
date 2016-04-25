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
   public class OptionState : MenuBase
   {
      public OptionState(Game game, Vector2 position, InformationGame informationJeu )
         :base(game,position,informationJeu)
      {

      }
      public override void Initialiser()
      {
         AjouterBouton("Back", OnBackPressed);
         AjouterBouton("Retourner au Menu", OnMenuTransistionPressed);
         AjouterBouton("Quitter", OnQuitterPressed);


         ListeBouton[IndexComposante].ChangerÉtat();

         base.Initialiser();
      }

      public void OnBackPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.AjouterÉvénement((int)Message.GameState_Pop);
      }
      public void OnMenuTransistionPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.AjouterÉvénement((int)Message.GameState_TransitionMenu);
         Cleanup();
      }
      public void OnQuitterPressed(object sender, EventArgs eventArgs)
      {
         ManagerMessage.AjouterÉvénement((int)Message.QuitterJeu);
      }

      public override void Cleanup()
      {
         base.Cleanup();
      }

      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
      }
   }
}
