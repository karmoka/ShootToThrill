using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
   class wip_Main : Microsoft.Xna.Framework.Game
   {
      Options options { get; set; }

      const float INTERVAL_MAJ_STANDARD = 1 / 60f;
      GraphicsDeviceManager graphics;

      //GameStates
      GamePlayState GamePlay { get; set; }
      SplashState Splash { get; set; }
      MenuState Menu { get; set; }
      OptionState Option { get; set; }

      //Services
      RessourcesManager<Model> GestionnaireDeModèle { get; set; }
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      SpriteBatch GestionnaireDeSprites { get; set; }
      MessageManager ManagerDeMessage { get; set; }
      GameStateManager ManagerGameState { get; set; }
      InputManager GestionnaireInput;
      IOManager GestionInput { get; set; }

      //Notifications
      Notification nGameStateChanged { get; set; }
      Notification nQuitterJeu { get; set; }

      //InformationGameplay
      InformationGame InformationJeu { get; set; }

      public wip_Main()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";

         IsMouseVisible = true;
         IsFixedTimeStep = false;
         graphics.SynchronizeWithVerticalRetrace = false;

         options = new Options(this, Content.Load<DescriptionOptions>("Options"));

         graphics.PreferredBackBufferWidth = options.WindowWidth;
         graphics.PreferredBackBufferHeight = options.WindowHeight;
      }

      protected override void Initialize()
      {
         InformationJeu = new InformationGame(1,1);

         InitializerServices();
         InitializerNotifications();

         ManagerDeMessage.AjouterÉvénement((int)Message.GameState_TransistionGamePlay);

         base.Initialize();
      }

      protected override void LoadContent()
      {
         InitializerGameStates();
         base.LoadContent();
      }

      public void InitializerGameStates()
      {
         Menu = new MenuState(this, new Vector2(100, 400), InformationJeu);
         Splash = new SplashState(this, INTERVAL_MAJ_STANDARD);
         Option = new OptionState(this, new Vector2(options.WindowWidth / 2, options.WindowHeight / 2), InformationJeu);
      }

      public void InitializerServices()
      {
         CaméraFixe caméra = new CaméraFixe(this, new Vector3(10, 10, 10), Vector3.Zero, Vector3.Up);

         GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effets");
         GestionnaireDeModèle = new RessourcesManager<Model>(this, "Modèle");
         GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
         GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
         GestionnaireDeSprites = new SpriteBatch(GraphicsDevice);
         ManagerGameState = new GameStateManager(this);
         ManagerDeMessage = new MessageManager(this);
         GestionnaireInput = new InputManager(this);
         GestionInput = new IOManager(this, options.IntervalMAJStandard);

         Services.AddService(typeof(Options), options);
         Services.AddService(typeof(EntitySystem), new EntitySystem(this));
         Services.AddService(typeof(InputManager), GestionnaireInput);
         Services.AddService(typeof(SpriteBatch), GestionnaireDeSprites);
         Services.AddService(typeof(GameStateManager), ManagerGameState);
         Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
         Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèle);
         Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
         Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
         Services.AddService(typeof(MessageManager), ManagerDeMessage);
         Services.AddService(typeof(Caméra), caméra);
         Services.AddService(typeof(IOManager), GestionInput);

         Components.Add(GestionnaireInput);
         Components.Add(ManagerGameState);
         Components.Add(new Afficheur3D(this));
         Components.Add(ManagerDeMessage);
         Components.Add(caméra);
         Components.Add(GestionInput);
      }

      public void InitializerNotifications()
      {
         //Les evenement qui appelerons OnQuitterJeu
         nQuitterJeu = new Notification();
         nQuitterJeu.SetCallBack(OnQuitterJeu);
         nQuitterJeu.AjouterAuSystem((int)Message.QuitterJeu, ManagerDeMessage);

         nGameStateChanged = new Notification();
         nGameStateChanged.SetCallBack(OnGameStateChanged);

         //Les evenement qui appelerons OnGameStateChanged
         nGameStateChanged.AjouterAuSystem(Message.GameState_GamePlay, ManagerDeMessage);
         nGameStateChanged.AjouterAuSystem(Message.GameState_Menu, ManagerDeMessage);
         nGameStateChanged.AjouterAuSystem(Message.GameState_TransistionGamePlay, ManagerDeMessage);
         nGameStateChanged.AjouterAuSystem(Message.GameState_TransitionMenu, ManagerDeMessage);
         nGameStateChanged.AjouterAuSystem(Message.GameState_Pop, ManagerDeMessage);
         nGameStateChanged.AjouterAuSystem(Message.GameState_Option, ManagerDeMessage);
      }

      protected override void Update(GameTime gameTime)
      {
         if (GestionnaireInput.EstNouvelleTouche(Keys.P))
         {
            ManagerGameState.Pop();
         }


         base.Update(gameTime);
      }

      /// <summary>
      /// Gère la transistion entre les différentrs gamestates selon les messages que le reste du programme lui envoie
      /// </summary>
      /// <param name="id"></param>
      public void OnGameStateChanged(int id)
      {
         switch (id)
         {
            case ((int)Message.GameState_GamePlay):
               InformationJeu.IDMap = 2;
               InformationJeu.AjouterJoueur(PlayerIndex.One);
               InformationJeu.SetPlayerAvatar(0, 0);
               ManagerGameState.Push(GamePlay);
               ManagerGameState.Push(new TimerState(this, new Vector2(options.WindowWidth / 2, options.WindowHeight / 2), 5, "Arial20"));
               break;
            case ((int)Message.GameState_Menu):
               ManagerGameState.Push(new StateImageBackground(this, "splash_screen"));
               ManagerGameState.Push(Menu);
               break;
            case ((int)Message.GameState_TransistionGamePlay):
               GamePlay = new GamePlayState(this, InformationJeu);
               ManagerGameState.ClearStates();
               ManagerGameState.Push(new SplashState(this, INTERVAL_MAJ_STANDARD, Message.GameState_GamePlay, 2));
               break;
            case ((int)Message.GameState_TransitionMenu):
               ManagerGameState.ClearStates();
               ManagerGameState.Push(new SplashState(this, INTERVAL_MAJ_STANDARD, Message.GameState_Menu, 2));
               break;
            case ((int)Message.GameState_Pop):
               ManagerGameState.Pop();
               break;
            case ((int)Message.GameState_Option):
               ManagerGameState.Push(Option);
               break;
         }
      }

      public void OnQuitterJeu(int id)
      {
         this.Exit();
      }

      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.White);

         GestionnaireDeSprites.Begin();
         base.Draw(gameTime);
         GestionnaireDeSprites.End();
      }

   }
}
