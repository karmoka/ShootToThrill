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
    public class TimerState : Microsoft.Xna.Framework.DrawableGameComponent, GameState
    {
        const int UNE_SECONDE = 1;
        
        int TempsDuDécompte { get; set; }
        int TempsActuel { get; set; }
        float TempsDepuisMAJ { get; set; }
        float IntervalMAJ { get; set; }
        float ScaleTexte { get; set; }
        string Texte { get; set; }
        string NomFonts { get; set; }

        public bool EstActivé { get; set; }
        public bool EstDétruit { get; set; }

        Vector2 Position { get; set; }
        Color couleur { get; set; }
        string NomTexture { get; set; }

        RessourcesManager<Texture2D> GestionnaireTextures { get; set; }
        RessourcesManager<SpriteFont> GestionnaireFonts { get; set; }
        MessageManager ManagerDeMessage { get; set; }
        SpriteBatch GestionnaireSprites { get; set; }
        
        Texture2D DummyTexture { get; set; }
        Texture2D ImageBack { get; set; }
        SpriteFont Fonts { get; set; }

        public TimerState(Game game, Vector2 position, int tempsDécompte, float intervalMAJ, string nomFonts)
            : base(game)
        {
            TempsDuDécompte = tempsDécompte;
            IntervalMAJ = intervalMAJ;
            Position = position;
            NomFonts = nomFonts;
        }
        public TimerState(Game game, Vector2 position, int tempsDécompte, string nomFonts)
            : base(game)
        {
            TempsDuDécompte = tempsDécompte;
            IntervalMAJ = UNE_SECONDE;
            Position = position;
            NomFonts = nomFonts;
        }

        public void Initialiser()
        {
            TempsDepuisMAJ = 0;
            TempsActuel = TempsDuDécompte;
            Texte = TempsActuel.ToString();

            EstActivé = false;
            EstDétruit = false;

            GestionnaireTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionnaireFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            GestionnaireSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            ManagerDeMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            Fonts = GestionnaireFonts.Find(NomFonts);
            //ImageBack = GestionnaireTextures.Find(NomTexture);

            DummyTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
            couleur = Color.Red;
        }

        public void Cleanup()
        {

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
            if(TempsDepuisMAJ >= IntervalMAJ)
            {
                if (TempsActuel == 0)
                {
                    ManagerDeMessage.AjouterÉvénement((int)Message.GameState_Pop);
                }

                ChangerNombre();

                TempsDepuisMAJ = 0;
            }
        }

        public void ChangerNombre()
        {
            TempsActuel--;
            Texte = TempsActuel.ToString();
        }

        public void Draw(GameTime gametime, float ordre)
        {
            GestionnaireSprites.DrawString(Fonts, Texte,  Position,Color.Black,0,Vector2.Zero,2,SpriteEffects.None,ordre);
        }
    }
}
