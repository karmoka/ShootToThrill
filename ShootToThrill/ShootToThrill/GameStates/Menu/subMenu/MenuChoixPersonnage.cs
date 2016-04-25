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
    public class MenuChoixPersonnage : MenuBase
    {
        Vector2[] PositionsThumbnails { get; set; }
        Vector2[] OffsetFleches { get; set; }
        Vector2 DimensionFleche = new Vector2(20, 20);
        bool[] PlayerActivé { get; set; }
        int[] IndexPlayers { get; set; }
        Color[] CouleursFleches = new Color[4] { Color.Red, Color.Blue, Color.Green, Color.Yellow };
        SpriteColorable[] Fleches { get; set; }

        const float MARGE = 10;
        Color couleur { get; set; }
        string NomTexture { get; set; }

        int[] TableauPersonnage { get; set; }
        Sprite[] Thumbnails { get; set; }
        Vector2 DimensionImage { get; set; }

        bool AInitialiser { get; set; }

        public MenuChoixPersonnage(Game game, Vector2 position, InformationGame informationJeu)
            : base(game, position, informationJeu)
        {
            AInitialiser = false;
        }


        public override void Initialiser()
        {
            base.Initialiser();

            PlayerActivé = new bool[4] { true, false, false, false };
            IndexPlayers = new int[4] { 0, 0, 0, 0 };

            DimensionImage = new Vector2(100, 100);
            TableauPersonnage = new int[OptionJeu.NbAvatar];

            CalculerOffsetFleches();
            LoaderThumbnail();
            LoaderFleche();

            AjouterBouton("Commencer", OnCommencerPressed);
            AjouterBouton("Back", OnBackPressed);
            ListeBouton[IndexComposante].ChangerÉtat();
        }

        /// <summary>
        /// Calcul la position de chaque fleche relative aux thumbnails
        /// </summary>
        void CalculerOffsetFleches()
        {
            OffsetFleches = new Vector2[4];

            for (int i = 0; i < 4; ++i)
            {
                OffsetFleches[i] = new Vector2(0, 50) + new Vector2((i - 2) * DimensionImage.X / 4 + 10, 0);
            }
        }

        /// <summary>
        /// Créer et Ajoute les sprites de thumbnail aux Components
        /// </summary>
        void LoaderThumbnail()
        {
            for (int i = 0; i < TableauPersonnage.Length; ++i)
            {
                TableauPersonnage[i] = i;
            }
            Thumbnails = new Sprite[OptionJeu.NbAvatar];
            PositionsThumbnails = new Vector2[OptionJeu.NbAvatar];

            for (int i = 0; i < TableauPersonnage.Length; ++i)
            {
                PositionsThumbnails[i] = new Vector2(100, 100) + new Vector2(DimensionImage.X * i, 0);
                Thumbnails[i] = new Sprite(Game, "ThumbnailP" + i, PositionsThumbnails[i], DimensionImage);
                Thumbnails[i].Initialize();
                Game.Components.Add(Thumbnails[i]);
            }
        }

        /// <summary>
        /// Créer et ajoute les sprites des fleches aux components
        /// </summary>
        void LoaderFleche()
        {
            Fleches = new SpriteColorable[4];
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                Fleches[i] = new SpriteColorable(Game, "fleche", PositionsThumbnails[IndexPlayers[i]] + new Vector2(0, 100), DimensionFleche, CouleursFleches[i]);
                Game.Components.Add(Fleches[i]);
            }
        }


        public void OnCommencerPressed(object sender, EventArgs eventArgs)
        {
            ManagerMessage.AjouterÉvénement((int)Message.GameState_TransistionGamePlay);
            Cleanup();
        }

        //On assume que le gamestate est en focus
        public void OnBackPressed(object sender, EventArgs eventArgs)
        {
            ManagerMessage.AjouterÉvénement((int)Message.GameState_SubMenuPop);
            Cleanup();
        }

        public override void Cleanup()
        {
            for (int i = 0; i < TableauPersonnage.Length; ++i)
            {
                Game.Components.Remove(Thumbnails[i]);
            }
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                Game.Components.Remove(Fleches[i]);
            }
            base.Cleanup();
        }
        public override void Pause()
        {
            EstActivé = false;
            couleur = Color.DarkRed;
        }

        public override void Résumer()
        {
            EstActivé = true;
            couleur = Color.Red;
        }

        public override void Update(GameTime gameTime)
        {
            IdentifierPlayerActif();
            VérifierInput();
            ChangerPositionFleches();

            base.Update(gameTime);
        }

        void ChangerPositionFleches()
        {
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                Fleches[i].Déplacer(PositionsThumbnails[IndexPlayers[i]] + OffsetFleches[i]);
            }
        }

        /// <summary>
        /// Vérifie l'input et change les index
        /// </summary>
        void VérifierInput()
        {
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                if (GestionnaireInput.VersDroite((PlayerIndex)i) && IndexPlayers[i] < OptionJeu.NbAvatar - 1)
                {
                    IndexPlayers[i]++;
                    InformationJeu.SetPlayerAvatar(i, IndexPlayers[i]);
                }
                if (GestionnaireInput.VersGauche((PlayerIndex)i) && IndexPlayers[i] > 0)
                {
                    IndexPlayers[i]--;
                    InformationJeu.SetPlayerAvatar(i, IndexPlayers[i]);
                }
            }
        }

        /// <summary>
        /// Idnetifie si un player c'est ajouté et est actif
        /// </summary>
        void IdentifierPlayerActif()
        {
            for (int i = 0; i < (int)PlayerIndex.Four; ++i)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                {
                    PlayerActivé[i] = true;
                }
                else
                {
                    PlayerActivé[i] = false;
                }
            }
        }
    }
}
