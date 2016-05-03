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

        GestionnairesLumières GestionnaireDeLumières { get; set; }
        MMoteurPhysique ManagerPhysique { get; set; }
        ManagerAudio ManagerDeSons { get; set; }
        ModelManager ManagerModèle { get; set; }
        ScreenManager ManagerScreen { get; set; }
        IOManager GestionnaireInput { get; set; }
        MessageManager ManagerMessage { get; set; }
        Pathfinding TrouveurDeChemin { get; set; }
        RequêtePathManager RequeteDeChemin { get; set; }

        Jeu Jeu { get; set; }
        Lumière LumièreJeu { get; set; }

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

            InitialiserManagers();
            LoaderSons();
            LoaderMap(); // Doit être exécuter avant les joueurs pour donner la position du portail
            CréerJoueurs();
            GénérerViewports();
            InitialiserJoueur();

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

            TrouveurDeChemin = new Pathfinding(Game);
            RequeteDeChemin = new RequêtePathManager(Game); 
            Jeu = new Jeu(Game, InformationJeu.NBJoueur);
            GrilleUniverselle = new GrilleUniverselle(Game, new List<string>() { "Test" + InformationJeu.IDMap + "_1" });

            Game.Services.AddService(typeof(Pathfinding), TrouveurDeChemin);
            Game.Services.AddService(typeof(RequêtePathManager), RequeteDeChemin);
            Game.Services.AddService(typeof(Jeu), Jeu);
            Game.Services.AddService(typeof(GrilleUniverselle), GrilleUniverselle);

            Game.Components.Add(TrouveurDeChemin);
            Game.Components.Add(RequeteDeChemin);
            Game.Components.Add(Jeu);
            Game.Components.Add(GrilleUniverselle);

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
           DescriptionAvatar description;

            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                description = Game.Content.Load<DescriptionAvatar>("Description/Avatar" + i);
                ListeJoueur.Add(new MJoueur(Game, description, Jeu.PortailJoueur.Position, (PlayerIndex)i));
            }
        }
        /// <summary>
        /// Initilise les joueurs et les services qui leurs sont liés.
        /// </summary>
        void InitialiserJoueur()
        {
            for (int i = 0; i < InformationJeu.NBJoueur; ++i)
            {
                ListeJoueur[i].Initialize();
                GestionnaireDeLumières.AjouterLumières(new LumièreTracing(Game, ListeJoueur[i].Position, new Vector3(255, 0, 0), 10, ListeJoueur[i]));
                ManagerModèle.AjouterCaméra(new CaméraTracing(Game, ListeJoueur[i].Position, Vector3.Up, ListeJoueur[i], TableauViewports[i]));

                ManagerModèle.AjouterModele(ListeJoueur[i]);
                ManagerPhysique.AjouterObjet(ListeJoueur[i]);
            }

            //GestionnaireDeLumières.AjouterLumières(new Lumière(Game, new Vector3(5, 5, 5), Color.Blue.ToVector3(), 20, 0, Vector3.Zero, Vector4.Zero));
        }
        void InitialiserManagers()
        {
            OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;
            GestionnaireInput = Game.Services.GetService(typeof(IOManager)) as IOManager;
            ManagerMessage = Game.Services.GetService(typeof(MessageManager)) as MessageManager;
            GestionnaireSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeLumières = Game.Services.GetService(typeof(GestionnairesLumières)) as GestionnairesLumières;

            ManagerPhysique = new MMoteurPhysique(Game, OptionsJeu.IntervalMAJStandard);
            ManagerModèle = new ModelManager(Game);
            ManagerScreen = new ScreenManager(Game, Vector2.Zero, InformationJeu);
            ManagerDeSons = new ManagerAudio(Game);

            ManagerScreen.Initialize();

            Game.Services.AddService(typeof(ModelManager), ManagerModèle);
            Game.Services.AddService(typeof(MMoteurPhysique), ManagerPhysique);
            
            Game.Components.Add(ManagerPhysique);
            Game.Components.Add(ManagerModèle);
            Game.Components.Add(ManagerScreen);
            Game.Components.Add(ManagerDeSons);

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
            Game.Components.Remove(Jeu);
            Game.Components.Remove(GrilleUniverselle);

            Game.Services.RemoveService(typeof(ModelManager));
            Game.Services.RemoveService(typeof(MMoteurPhysique));
            Game.Services.RemoveService(typeof(MMoteurPhysique));
            Game.Services.RemoveService(typeof(Pathfinding));
            Game.Services.RemoveService(typeof(RequêtePathManager));
            Game.Services.RemoveService(typeof(Jeu));
            Game.Services.RemoveService(typeof(GrilleUniverselle));

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
        }

        public void Draw(GameTime gametime, float ordre)
        {
            base.Draw(gametime);
        }
    }
}
