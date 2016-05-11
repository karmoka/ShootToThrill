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
    class JoueurScreenManager : DrawableGameComponent
    {
        int[] EspaceX1 { get; set; }
        int[] EspaceX2 { get; set; }
        int[] EspaceY1 { get; set; }
        int[] EspaceY2 { get; set; }

        MJoueur Joueur { get; set; }
        int IndexJoueur { get { return (int)Joueur.IndexJoueur; } }
        Vector2 Échelle { get; set; }

        Rectangle RectangleVieRestante { get; set; }
        Rectangle RectangleVieMax { get; set; }
        Rectangle RectangleVieNoir { get; set; }
        Rectangle RectangleBleu { get; set; }
        Rectangle RectangleMunitionRestante { get; set; }
        Rectangle RectangleMunitionMaxChargeur { get; set; }
        Rectangle RectangleMunitionNoir { get; set; }
        Rectangle RectangleRecharge { get; set; }
        Rectangle RectangleScore { get; set; }


        Texture2D ImageVie { get; set; }
        Texture2D ImageMunition { get; set; }
        Texture2D ImageFusil { get; set; }
        Texture2D ImageNoir { get; set; }
        Texture2D ImageBleu { get; set; }
        Texture2D ImageMunitionRechargeMax { get; set; }
        Texture2D ImageMunitionRechargeMin { get; set; }
        Texture2D ImageScore { get; set; }
        Rectangle RectangleFusil { get; set; }

        List<Texture2D> ListeImageRechage { get; set; }
        List<Rectangle> ListeRectangleRecharge { get; set; }

        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        SpriteFont ArialFont { get; set; }

        public JoueurScreenManager(Game game, MJoueur joueur)
            : base(game)
        {
            Joueur = joueur;
        }

        #region Initialize
        public override void Initialize()
        {
            base.Initialize();
            InitialiserEspaceX();
            InitialiserEspaceY();
            InitialiserRectangles();
            InitialiserÉchelle();
        }

        protected override void LoadContent()
        {   
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            //ImageFusil = GestionnaireDeTextures.Find(Joueur.ArmeSélectionnée.NomArme);
            //ImageMunitionRechargeMax = GestionnaireDeTextures.Find("MunitionRechargeMax");
            //ImageMunitionRechargeMin = GestionnaireDeTextures.Find("MunitionRechargeMin");
            ImageNoir = GestionnaireDeTextures.Find("RectangleNoir");
            ImageVie = GestionnaireDeTextures.Find("RectangleRouge");
            ImageMunition = GestionnaireDeTextures.Find("RectangleVert");
            ImageScore = GestionnaireDeTextures.Find("RectangleBlanc");
            ImageBleu = GestionnaireDeTextures.Find("UIRaph");
            base.LoadContent();
        }

        void InitialiserEspaceX()
        {
            EspaceX1 = new int[6];
            EspaceX1[0] = 0;
            EspaceX1[1] = EspaceX1[0] + 2;
            EspaceX1[2] = EspaceX1[1] + 5;
            EspaceX1[3] = EspaceX1[2] + 5;
            EspaceX1[4] = EspaceX1[3] + 15;
            EspaceX1[5] = EspaceX1[4] + 1;

            int largeur = EspaceX1[EspaceX1.Length - 1];
            int variationLargeur = Game.Window.ClientBounds.Width / 5 / largeur;

            for (int cpt = 0; cpt < EspaceX1.Length; ++cpt)
            {
                EspaceX1[cpt] *= variationLargeur;
            }

            EspaceX2 = new int[8];
            EspaceX2[0] = 0;
            EspaceX2[1] = EspaceX2[0] + 1 * variationLargeur;
            EspaceX2[2] = EspaceX2[1] + 1 * variationLargeur;
            EspaceX2[3] = EspaceX2[2] + 1 * variationLargeur;
            EspaceX2[4] = EspaceX2[3] + 1 * variationLargeur;
            EspaceX2[5] = EspaceX2[4] + 1 * variationLargeur;
            EspaceX2[6] = EspaceX2[5] + 1 * variationLargeur;
            EspaceX2[7] = EspaceX2[6] + 1 * variationLargeur;

            if (IndexJoueur % 2 != 0)
            {
                int[] espaceX1 = new int[EspaceX1.Length];
                int[] espaceX2 = new int[EspaceX2.Length];
                for (int cpt = 0; cpt < EspaceX1.Length; ++cpt)
                {
                    espaceX1[cpt] = Game.Window.ClientBounds.Width - EspaceX1[espaceX1.Length - 1 - cpt];
                }
                for (int cpt = 0; cpt < EspaceX2.Length; ++cpt)
                {
                    espaceX2[cpt] = Game.Window.ClientBounds.Width - EspaceX2[espaceX2.Length - 1 - cpt];
                }
                EspaceX1 = espaceX1;
                EspaceX2 = espaceX2;
            }
        }

        void InitialiserEspaceY()
        {
            EspaceY1 = new int[9];
            EspaceY1[0] = 0;
            EspaceY1[1] = EspaceY1[0] + 1;
            EspaceY1[2] = EspaceY1[1] + 10;
            EspaceY1[3] = EspaceY1[2] + 3; //2
            EspaceY1[4] = EspaceY1[3] + 1;
            EspaceY1[5] = EspaceY1[4] + 2;
            EspaceY1[6] = EspaceY1[5] + 1;
            EspaceY1[7] = EspaceY1[6] + 1;
            EspaceY1[8] = EspaceY1[7] + 1;

            int hauteurInitiale = IndexJoueur >= 3 ? Game.Window.ClientBounds.Height / 2 : 0;
            int hauteur = EspaceY1[EspaceY1.Length - 1];
            int variationHauteur = Game.Window.ClientBounds.Height / 5 / hauteur;
            
            for (int cpt = 0; cpt < EspaceY1.Length; ++cpt)
            {
                EspaceY1[cpt] = EspaceY1[cpt] * variationHauteur + hauteurInitiale;
            }

            EspaceY2 = new int[9];
            EspaceY2[0] = 0 + hauteurInitiale;
            EspaceY2[1] = EspaceY2[0] + 1 * variationHauteur;
            EspaceY2[2] = EspaceY2[1] + 3 * variationHauteur;
            EspaceY2[3] = EspaceY2[2] + 2 * variationHauteur;
            EspaceY2[4] = EspaceY2[3] + 3 * variationHauteur;
            EspaceY2[5] = EspaceY2[4] + 2 * variationHauteur;
            EspaceY2[6] = EspaceY2[5] + 3 * variationHauteur;
            EspaceY2[7] = EspaceY2[6] + 1 * variationHauteur;
            EspaceY2[8] = EspaceY2[7] + 1 * variationHauteur;

        }

        void InitialiserRectangles()
        {
            if (IndexJoueur % 2 == 0)
            {
                //RectangleFusil = new Rectangle(EspaceX1[1], EspaceY1[1], EspaceX1[2] - EspaceX1[1], EspaceY1[2] - EspaceY1[1]);

                RectangleMunitionMaxChargeur = new Rectangle(EspaceX1[3], EspaceY2[1], EspaceX1[4] - EspaceX1[3], EspaceY2[2] - EspaceY2[1]);

                RectangleVieMax = new Rectangle(EspaceX1[3], EspaceY2[3], EspaceX1[4] - EspaceX1[3], EspaceY2[4] - EspaceY2[3]);
               
                RectangleScore = new Rectangle(EspaceX1[3], EspaceY2[5], EspaceX1[4] - EspaceX1[3], EspaceY2[6] - EspaceY2[5]);
            }
            else
            {
                //RectangleFusil = new Rectangle(EspaceX1[3], EspaceY1[1], EspaceX1[4] - EspaceX1[3], EspaceY1[2] - EspaceY1[1]);

                RectangleMunitionMaxChargeur = new Rectangle(EspaceX1[1], EspaceY2[1], EspaceX1[2] - EspaceX1[1], EspaceY2[2] - EspaceY2[1]);
                
                RectangleVieMax = new Rectangle(EspaceX1[1], EspaceY2[3], EspaceX1[2] - EspaceX1[1], EspaceY2[4] - EspaceY2[3]);
                
                RectangleScore = new Rectangle(EspaceX1[1], EspaceY2[5], EspaceX1[2] - EspaceX1[1], EspaceY2[6] - EspaceY2[5]);
            }

            RectangleBleu = new Rectangle(EspaceX1[0], EspaceY2[0], EspaceX1[5] - EspaceX1[0], EspaceY1[8] - EspaceY1[0]);//y2
            //RectangleBleu2 = new Rectangle(EspaceX2[0], EspaceY1[5], EspaceX2[7] - EspaceX2[0], EspaceY1[8] - EspaceY1[5]);
            
            //InitialiserRecharge();
            
            RectangleMunitionNoir = new Rectangle(RectangleMunitionMaxChargeur.X + 1, RectangleMunitionMaxChargeur.Y + 1,
                                                      RectangleMunitionMaxChargeur.Width - 2, RectangleMunitionMaxChargeur.Height - 2);
            RectangleMunitionRestante = new Rectangle(RectangleMunitionMaxChargeur.X + 1, RectangleMunitionMaxChargeur.Y + 1,
                                                      RectangleMunitionMaxChargeur.Width - 2, RectangleMunitionMaxChargeur.Height - 2);
            
            RectangleVieNoir = new Rectangle(RectangleVieMax.X + 1, RectangleVieMax.Y + 1, RectangleVieMax.Width - 2, RectangleVieMax.Height - 2);
            RectangleVieRestante = new Rectangle(RectangleVieMax.X + 1, RectangleVieMax.Y + 1, RectangleVieMax.Width - 2, RectangleVieMax.Height - 2);

        }

        void InitialiserRecharge()
        {
            ListeRectangleRecharge = new List<Rectangle>();
            ListeImageRechage = new List<Texture2D>();

            int cpt = 0;
            if (Joueur.ArmeSélectionnée.NombreRechargeMax % 2 == 0)
            {
                for (int y = 0; y < 2; ++y)
                {
                    for (int x = 0; x < Joueur.ArmeSélectionnée.NombreRechargeMax / 2; ++x)
                    {
                        RectangleRecharge = new Rectangle(EspaceX2[1 + 2 * x], EspaceY1[3 + 2 * y], EspaceX2[2 + 2 * x] - EspaceX2[1 + 2 * x], EspaceY1[4 + 2 * y] - EspaceY1[3 + 2 * y]);
                        ListeRectangleRecharge.Add(RectangleRecharge);
                        if (cpt < Joueur.ArmeSélectionnée.NombreRechargeRestante)
                        {
                            ListeImageRechage.Add(ImageMunitionRechargeMax);
                        }
                        else
                        {
                            ListeImageRechage.Add(ImageMunitionRechargeMin);
                        }
                        ++cpt;
                    }
                }
            }
            else
            {
                for (int y = 0; y < 2; ++y)
                {
                    for (int x = 0; x < Math.Floor((float)Joueur.ArmeSélectionnée.NombreRechargeMax / 2) + y; ++x)
                    {
                        ListeRectangleRecharge.Add(RectangleRecharge);
                        if (cpt < Joueur.ArmeSélectionnée.NombreRechargeRestante)
                        {
                            ListeImageRechage.Add(ImageMunitionRechargeMax);
                        }
                        else
                        {
                            ListeImageRechage.Add(ImageMunitionRechargeMin);
                        }
                        ++cpt;
                    }
                }
            }
        }

        void InitialiserÉchelle()
        {
            Vector2 dimensionString = ArialFont.MeasureString(Joueur.Vie + " / " + Joueur.VieMax);
            Échelle = new Vector2(RectangleVieRestante.Width / dimensionString.X, RectangleVieRestante.Height / dimensionString.Y);
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            //UpdaterFusil();
            //UpdaterRecharge();
            Initialize();
            UpdaterMunition();
            UpdaterVie();

            base.Update(gameTime);
        }

        void UpdaterFusil()
        {
            Texture2D imageFusilActuel = GestionnaireDeTextures.Find(Joueur.ArmeSélectionnée.NomArme);
            if (ImageFusil != imageFusilActuel)
            {
                ImageFusil = imageFusilActuel;
                InitialiserRecharge();
            }
        }

        void UpdaterRecharge()
        {
            for (int cpt = 0; cpt < ListeImageRechage.Count; ++cpt)
            {
                if (cpt < Joueur.ArmeSélectionnée.NombreRechargeRestante)
                {
                    ListeImageRechage[cpt] = ImageMunitionRechargeMax;
                }
                else
                {
                    ListeImageRechage[cpt] = ImageMunitionRechargeMin;
                }
            }
        }

        void UpdaterMunition()
        {
            RectangleMunitionRestante = new Rectangle(RectangleMunitionRestante.X, RectangleMunitionRestante.Y,
                                                      RectangleMunitionNoir.Width * Joueur.ArmeSélectionnée.MunitionRestantDansChargeur /
                                                      Joueur.ArmeSélectionnée.MunitionMaxDansChargeur, RectangleMunitionRestante.Height);
        }

        void UpdaterVie()
        {
            RectangleVieRestante = new Rectangle(RectangleVieRestante.X, RectangleVieRestante.Y, RectangleVieNoir.Width * Joueur.Vie / Joueur.VieMax, RectangleVieRestante.Height);
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            DrawArrièrePlan();
            //DrawFusil();
            //DrawRecharge();
            DrawMunition();
            DrawVie();
            DrawScore();            
            base.Draw(gameTime);
        }

        void DrawArrièrePlan()
        {
            if(IndexJoueur %2 == 0)
            {
                GestionSprites.Draw(ImageBleu, RectangleBleu, Color.Blue);
            }
            else
            {
                //Si le joeur se trouve dans un viewport pair(coller a droite), on flip son afficheur de vie/munition/etc
                GestionSprites.Draw(ImageBleu, RectangleBleu, new Rectangle(0,0,ImageBleu.Width,ImageBleu.Height), Color.Blue,0,Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
        }

        void DrawFusil()
        {
            GestionSprites.Draw(ImageFusil, RectangleFusil, Color.White);
        }

        void DrawRecharge()
        {
            for (int cpt = 0; cpt < ListeRectangleRecharge.Count; ++cpt)
            {
                GestionSprites.Draw(ListeImageRechage[cpt], ListeRectangleRecharge[cpt], Color.White);
            }
        }

        void DrawMunition()
        {
            GestionSprites.Draw(ImageMunition, RectangleMunitionMaxChargeur, Color.Green);
            GestionSprites.Draw(ImageNoir, RectangleMunitionNoir, Color.Black);
            GestionSprites.Draw(ImageMunition, RectangleMunitionRestante, Color.Green);
            //DrawString("\u221E" , RectangleMunitionRestante);
            DrawString(Joueur.ArmeSélectionnée.MunitionRestantDansChargeur + " / " + Joueur.ArmeSélectionnée.MunitionMaxDansChargeur, RectangleMunitionNoir);
        }

        void DrawVie()
        {
            GestionSprites.Draw(ImageVie, RectangleVieMax, Color.Red);
            GestionSprites.Draw(ImageNoir, RectangleVieNoir, Color.Black);
            GestionSprites.Draw(ImageVie, RectangleVieRestante, Color.Red);
            DrawString(Joueur.Vie + " / " + Joueur.VieMax, RectangleVieNoir);
        }
        
        void DrawScore()
        {
            GestionSprites.Draw(ImageScore, RectangleScore, Color.Purple);
            DrawString("Score: " + Joueur.Score, RectangleScore);
        }

        void DrawString(string message, Rectangle rectangle)
        {
            Vector2 dimensionString = ArialFont.MeasureString(message);
            Vector2 positionString = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2) - dimensionString / 2 * Échelle;
            GestionSprites.DrawString(ArialFont, message, positionString, Color.White, 0f, Vector2.Zero, Échelle, SpriteEffects.None, 0);
        }
        #endregion
    }
}
