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
using ProjetPrincipal.Data;

namespace AtelierXNA
{
    /// <summary>
    /// Gère toute la logique du Gameplay, incluant les différents système qui sont nécessaire
    /// </summary>
    public class GamePlayState : DrawableGameComponent, GameState
    {
        Options OptionsJeu { get; set; }
        GrilleUniverselle GrilleUniverselle { get; set; }
        InformationGame InformationJeu { get; set; }

        public bool EstActivé { get; set; }
        public bool EstDétruit { get; set; }
        bool OptionActivé { get; set; }

        SpriteBatch GestionnaireSprites { get; set; }
        List<MJoueur> ListeJoueur { get; set; }
        Viewport[] TableauViewports { get; set; }

        MMoteurPhysique ManagerPhysique { get; set; }
        ManagerAudio ManagerDeSons { get; set; }
        ModelManager ManagerModèle { get; set; }
        ScreenManager ManagerScreen { get; set; }
        IOManager GestionnaireInput { get; set; }
        MessageManager ManagerMessage { get; set; }
        Pathfinding TrouveurDeChemin { get; set; }
        RequêtePathManager RequeteDeChemin { get; set; }

        Jeu jeu { get; set; }
        Lumière LumièreJeu { get; set; }
        BillboardColoréTracing t { get; set; }

        VolumeDeForce test { get; set; }

        public GamePlayState(Game game, InformationGame informationJeu)
            : base(game)
        {
            InformationJeu = informationJeu;
            ListeJoueur = new List<MJoueur>();
        }

        public void Initialiser()
        {
            OptionActivé = false;
            EstActivé = true;
            EstDétruit = false;

            //test = new CubeDeForce(Game, new Vector3(3, 2, 3), Vector3.One, new Vector3(0, 10, 0));
            test = new SphereDeForce(Game, new Vector3(3,2,3), 1f, new Vector3(0, 20, 0));
            test.Initialize();

            InitialiserManagers();
            LoaderSons();
            CréerJoueurs();
            GénérerViewports();
            InitialiserJoueur();

            ManagerPhysique.AjouterObjet(test);
            ManagerModèle.AjouterModele(test);

            t = new BillboardColoréTracing(Game, 1f, Vector3.Zero, Vector3.One * 3, new Vector2(5, 5), Color.Red, 1 / 60f, ListeJoueur[0]);
            t.Initialize();
            ManagerModèle.AjouterModele(t);

            LoaderMap();

            //ManagerDeSons.JouerSons("Menu");
        }

        public void GénérerViewports()
        {
            TableauViewports = new Viewport[InformationJeu.NBJoueur];
            switch (InformationJeu.NBJoueur)
            {
                case (1):
                    TableauViewports[0] = new Viewport(0, 0, OptionsJeu.WindowWidth, OptionsJeu.WindowHeight);
                    break;
                case (2):
                    TableauViewports[0] = new Viewport(0, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight);
                    TableauViewports[1] = new Viewport(OptionsJeu.WindowWidth / 2, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight);
                    break;
                case (3):
                    TableauViewports[0] = new Viewport(0, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    TableauViewports[1] = new Viewport(OptionsJeu.WindowWidth / 2, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    TableauViewports[2] = new Viewport(0, OptionsJeu.WindowHeight / 2, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    break;
                case (4):
                    TableauViewports[0] = new Viewport(0, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    TableauViewports[1] = new Viewport(OptionsJeu.WindowWidth / 2, 0, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    TableauViewports[2] = new Viewport(0, OptionsJeu.WindowHeight / 2, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    TableauViewports[3] = new Viewport(OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2, OptionsJeu.WindowWidth / 2, OptionsJeu.WindowHeight / 2);
                    break;
            }
        }
       void LoaderSons()
        {
           ManagerDeSons.AjouterSons("Menu");
        }

        /// <summary>
        /// Load la carte en mémoire ainsi que les différents services qui lui sont nécessaire
        /// </summary>
        void LoaderMap()
        {
            GrilleUniverselle = new GrilleUniverselle(Game, new List<string>() { "Test" + InformationJeu.IDMap + "_1" });
            Jeu jeu = new Jeu(Game);

            Game.Services.AddService(typeof(GrilleUniverselle), GrilleUniverselle);
            Game.Services.AddService(typeof(Jeu), jeu);

            Game.Components.Add(GrilleUniverselle);
            Game.Components.Add(jeu);

            ManagerModèle.AjouterModele(GrilleUniverselle);
            foreach (CubeAdditionnable c in GrilleUniverselle.ListeCube)
            {
                ManagerPhysique.AjouterObjet(c);
            }

        }
        /// <summary>
        /// Créer les instances des joueurs selon l'avatar sélectionné
        /// </summary>
        void CréerJoueurs()
        {
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                MObjetDeBase o = new MObjetDeBase(Game, "Scene2", 1, Vector3.Zero, Vector3.One );
                ListeJoueur.Add(new MJoueur(Game, o, new ObjetPhysique(Game, new Vector3(i, 2+i, i)*3), (PlayerIndex)i));
                //DescriptionJoueur description = Game.Content.Load<DescriptionJoueur>("Description/Joueur" + InformationJeu.idPlayers[i]);
                //ListeJoueur.Add(new Joueur(Game, description, (PlayerIndex)i));
            }
           //LumièreJeu = new Lumière(Game, new Vector3(5,1,5), Vector3.One, 4, 4, Vector3.One, Vector4.One / 10);

           //ObjetDeBaseAniméEtÉclairé o = new ObjetDeBaseAniméEtÉclairé(Game, "untitled", "UIRaph", 1f, Vector3.Zero, Vector3.Up, "Phong", LumièreJeu, 1 / 60f);
           //MObjetDeBase o = new MObjetDeBase(Game, "Scene2", 1, Vector3.Zero, Vector3.Up * 3);
           //ListeJoueur.Add(new MJoueur(Game, "butterfly", "butterfly", new ObjetPhysique(Game, Vector3.Up * 3), PlayerIndex.One));
           //ListeJoueur.Add(new MJoueur(Game, o, new ObjetPhysique(Game, Vector3.Up * 3), PlayerIndex.One));
        }
        /// <summary>
        /// Initilise les joueurs et les services qui leurs sont liés.
        /// </summary>
        void InitialiserJoueur()
        {
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                ListeJoueur[i].Initialize();
                //ListeJoueur[i].ChangerCouleur(InformationJeu.CouleursJoueurs[i]);
                CaméraTracing cam = new CaméraTracing(Game, ListeJoueur[i].Position, Vector3.Up, ListeJoueur[i], TableauViewports[i]);

                ManagerModèle.AjouterCaméra(cam);
                ManagerModèle.AjouterModele(ListeJoueur[i]);
                ManagerPhysique.AjouterObjet(ListeJoueur[i]);
            }
        }
        void InitialiserManagers()
        {
            OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
            GestionnaireInput = Game.Services.GetService(typeof(IOManager)) as IOManager;
            ManagerMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            GestionnaireSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            TrouveurDeChemin = new Pathfinding(Game);
            RequeteDeChemin = new RequêtePathManager(Game);
            ManagerPhysique = new MMoteurPhysique(Game, OptionsJeu.IntervalMAJStandard);
            ManagerModèle = new ModelManager(Game);
            ManagerScreen = new ScreenManager(Game, Vector2.Zero, InformationJeu);
            ManagerDeSons = new ManagerAudio(Game);

            ManagerScreen.Initialize();

            Game.Components.Add(TrouveurDeChemin);
            Game.Components.Add(RequeteDeChemin);
            Game.Components.Add(ManagerPhysique);
            Game.Components.Add(ManagerModèle);
            Game.Components.Add(ManagerScreen);
            Game.Components.Add(ManagerDeSons);

            Game.Services.AddService(typeof(ModelManager), ManagerModèle);
            Game.Services.AddService(typeof(MMoteurPhysique), ManagerPhysique);
            Game.Services.AddService(typeof(Pathfinding), TrouveurDeChemin);
            Game.Services.AddService(typeof(RequêtePathManager), RequeteDeChemin);
        }

        /// <summary>
        /// Retire les services et composante de la liste
        /// </summary>
        public void Cleanup()
        {
            Game.Components.Remove(TrouveurDeChemin);
            Game.Components.Remove(RequeteDeChemin);
            Game.Components.Remove(ManagerPhysique);
            Game.Components.Remove(ManagerModèle);
            Game.Components.Remove(ManagerScreen);
            Game.Components.Remove(GrilleUniverselle);
            Game.Components.Remove(jeu);

            Game.Services.RemoveService(typeof(ModelManager));
            Game.Services.RemoveService(typeof(MoteurPhysique));
            Game.Services.RemoveService(typeof(MMoteurPhysique));
            Game.Services.RemoveService(typeof(Pathfinding));
            Game.Services.RemoveService(typeof(RequêtePathManager));
            Game.Services.RemoveService(typeof(GrilleUniverselle));
            Game.Services.RemoveService(typeof(Jeu));

            ManagerPhysique.Dispose();
            ManagerModèle.Dispose();
            ManagerScreen.Dispose();
        }

        public void Pause()
        {
            ManagerPhysique.Pause();

            EstActivé = false;
        }

        public void Résumer()
        {
            ManagerPhysique.Résumer();

            EstActivé = true;
        }

        bool bidon = true;
        public void Update(GameTime gametime)
        {
           float time = (float)gametime.ElapsedGameTime.TotalSeconds;
            if (GestionnaireInput.EstOptionSélectionner((PlayerIndex)0))
            {
                if (OptionActivé)
                {
                    ManagerMessage.AjouterÉvénement((int)Message.GameState_Pop);
                    OptionActivé = false;
                }
                else
                {
                    ManagerMessage.AjouterÉvénement((int)Message.GameState_Option);
                    OptionActivé = true;
                }
            }

           if(bidon && GestionnaireInput.EstNouvelleTouche(Keys.G, PlayerIndex.One))
           {
              Grenade g = new Grenade(Game, new Vector3(0,4,0), Vector3.Up * 3, "Scene2");
              g.Initialize();
              ManagerModèle.AjouterModele(g);
              ManagerPhysique.AjouterObjet(g);
              bidon = false;
           }
        }

        public void Draw(GameTime gametime, float ordre)
        {
            base.Draw(gametime);
        }
    }
}
