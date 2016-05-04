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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MenuChoixMap : MenuBase
    {
        bool EstInitialisé = false;
        Vector2 DimensionBoiteTexte = new Vector2(300, 200);
        TextboxWImage DescriptionMap { get; set; }

        public MenuChoixMap(Game game, Vector2 position, InformationGame informationJeu)
            : base(game, position, informationJeu)
        {

        }


        public override void Initialiser()
        {
            base.Initialiser();


            AjouterBouton("PacMan", OnButtonPressed);
            AjouterBouton("Plat", OnButtonPressed);
            AjouterBouton("Mini Golf", OnButtonPressed);
            AjouterBouton("Pebbles", OnButtonPressed);
            AjouterBouton("Back", OnBackPressed);
            ListeBouton[IndexComposante].ChangerÉtat();

            DescriptionMap = new TextboxWImage(Game, "buttonBack", "test1_1", "ArialDebug", new Vector2(OptionJeu.WindowWidth - DimensionBoiteTexte.X/2, DimensionBoiteTexte.Y/2), DimensionBoiteTexte);
            Game.Components.Add(DescriptionMap);

        }

        public void OnButtonPressed(object sender, EventArgs eventArgs)
        {
            InformationJeu.IDMap = IndexComposante+1;
            ManagerMessage.AjouterÉvénement((int)Message.GameState_ChoixPersoMenu);
        }

        //On assume que le gamestate est en focus
        public void OnBackPressed(object sender, EventArgs eventArgs)
        {
            ManagerMessage.AjouterÉvénement((int)Message.GameState_SubMenuPop);
            Cleanup();
        }

        public override void Cleanup()
        {
            DescriptionMap.CleanUp();
        }

        public override void Update(GameTime gameTime)
        {
            //Pas de description pour l'option back
            if (IndexComposante != ListeBouton.Count - 1)
            {
                Game.Window.Title = IndexComposante.ToString();
                DescriptionMap.LoaderDocumentTxt("Content/Maps/" + IndexComposante + "Description.txt");
                DescriptionMap.LoaderImage("test"+ (IndexComposante +1).ToString() + "_1");
            }

            base.Update(gameTime);
        }
    }
}
