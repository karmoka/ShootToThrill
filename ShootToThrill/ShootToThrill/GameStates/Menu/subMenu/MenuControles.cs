using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{

    public class MenuControles : MenuBase
    {
       const int MARGE = 10;
       const string NOM_IMAGE_CONTROLES = "…cranControles";
       Texture2D ImagesControles { get; set; }
       Rectangle RectangleAffichage { get; set; }


       public MenuControles(Game game, Vector2 position, InformationGame informationJeu)
            : base(game, position,informationJeu)
        {

        }

       public override void Initialiser()
        {
           base.Initialiser();

            AjouterBouton("Back", OnBackPressed);
            ListeBouton[IndexComposante].Changer…tat();
            ImagesControles = GestionnaireTextutes.Find(NOM_IMAGE_CONTROLES);
            RectangleAffichage = new Rectangle(MARGE, MARGE, OptionJeu.WindowWidth - 2 * MARGE, OptionJeu.WindowHeight - 2 * MARGE);
        }

        public void OnBackPressed(object sender, EventArgs eventArgs)
        {
           ManagerMessage.Ajouter…vÈnement((int)Message.GameState_SubMenuPop);
           Cleanup();
        }

        public override void Cleanup()
        {

           base.Cleanup();
        }

        public override void Draw(GameTime gameTime, float ordre)
        {
           spritebatch.Draw(ImagesControles, RectangleAffichage, Color.White);
           base.Draw(gameTime, ordre);
        }
    }
}
