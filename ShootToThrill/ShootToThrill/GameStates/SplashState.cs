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
    class SplashState : GameComponent, GameState
    {
        const float DURÉE_SPLASH_DÉFAUT = 0;
        Options OptionsJeu { get; set; }
        Message ProchainGameState { get; set; }
        const Message PROCHAIN_GAMESTATE_DEFAUT = Message.GameState_Menu;
        const float INCRÉMENT_FADE = 1 / 100f;

        public bool EstActivé { get; set; }
        public bool EstDétruit { get; set; }

        Color CouleurFade { get; set; }
        float fade { get; set; }

        float IntervalMAJ { get; set; }
        float TempsDepuisMAJ { get; set; }
        float DuréeSplash { get; set; }
        float TempsDepuisCréation { get; set; }

        RessourcesManager<Texture2D> GestionnaireTextures { get; set; }
        MessageManager GestionnaireMessage { get; set; }
        SpriteBatch GestionnaireSprites { get; set; }
        Texture2D ImageBack { get; set; }

        public SplashState(Game game, float intervalMAJ)
            : base(game)
        {
            IntervalMAJ = intervalMAJ;
            ProchainGameState = PROCHAIN_GAMESTATE_DEFAUT;
            DuréeSplash = DURÉE_SPLASH_DÉFAUT;
        }
        public SplashState(Game game, float intervalMAJ, Message prochainGameState)
            : base(game)
        {
            IntervalMAJ = intervalMAJ;
            ProchainGameState = prochainGameState;
            DuréeSplash = DURÉE_SPLASH_DÉFAUT;
        }
        public SplashState(Game game, float intervalMAJ, Message prochainGameState, float duréeSplash)
            : base(game)
        {
            IntervalMAJ = intervalMAJ;
            ProchainGameState = prochainGameState;
            DuréeSplash = duréeSplash;
        }

        public void Initialiser()
        {
            EstActivé = true;
            EstDétruit = false;
            CouleurFade = new Color(255, 255, 255, 0);
            TempsDepuisMAJ = 0;
            TempsDepuisCréation = 0;

            OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
            GestionnaireTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionnaireMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            GestionnaireSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            ImageBack = GestionnaireTextures.Find("splash_screen");
        }

        public void Cleanup()
        {
            EstDétruit = true;
        }

        public void Pause()
        {
            EstActivé = false;
        }

        public void Résumer()
        {
            EstActivé = true;
        }

        public void Update(GameTime gametime)
        {
            TempsDepuisMAJ += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (TempsDepuisMAJ >= IntervalMAJ)
            {
                DéterminerFade();
                CouleurFade = new Color(CouleurFade.R, CouleurFade.G, CouleurFade.B, fade);

                if (fade == 0 && !EstActivé)
                {
                    EstDétruit = true;
                    this.Dispose();
                    GestionnaireMessage.AjouterÉvénement((int)ProchainGameState);
                }
                if (fade > 1)
                {
                    TempsDepuisCréation += TempsDepuisMAJ;
                }
                if (TempsDepuisCréation > DuréeSplash)
                {
                    EstActivé = false;
                }

                TempsDepuisMAJ = 0;
            }
        }

        void DéterminerFade()
        {
            if (EstActivé)
            {
                fade = fade > 1 ? fade : fade + INCRÉMENT_FADE;
            }
            else
            {
                fade = fade < 0 ? 0 : fade - INCRÉMENT_FADE;
            }
        }

        public void Draw(GameTime gametime, float ordre)
        {
            GestionnaireSprites.Draw(ImageBack, new Rectangle(0, 0, OptionsJeu.WindowWidth, OptionsJeu.WindowHeight), CouleurFade);
        }
    }
}
