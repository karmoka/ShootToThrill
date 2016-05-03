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
   /// Gère l'affichage et l'update des éléments graphiques du jeu. Dessine la scene selon les différents viewports
   /// </summary>
   public class ModelManager : DrawableGameComponent
   {
      Options OptionsJeu { get; set; }
      List<IModele3d> ListeModele { get; set; }
      List<Caméra> ListeCamera { get; set; }

      Viewport ViewportDéfaut { get; set; }

      public ModelManager(Game game)
         : base(game)
      {
         ListeModele = new List<IModele3d>();
         ListeCamera = new List<Caméra>();
      }

      public override void Initialize()
      {
         OptionsJeu = Game.Services.GetService(typeof(Options)) as Options;

         foreach (IModele3d i in ListeModele)
         {
            i.Initialize();
         }

         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
      }

      public void AjouterModele(IModele3d modèle)
      {
          ListeModele.Add(modèle);
      }

      public void EnleverModèle(IModele3d modèle)
      {
         ListeModele.Remove(modèle);
      }

      public void AjouterCaméra(Caméra caméra)
      {
         ListeCamera.Add(caméra);
      }

      public void AjouterCaméra(List<Caméra> caméras)
      {
         foreach (Caméra c in caméras)
         {
            ListeCamera.Add(c);
         }
      }

      public override void Update(GameTime gameTime)
      {
         for (int i = 0; i < ListeModele.Count; ++i)
         {
            ListeModele[i].Update(gameTime);
         }
         for (int i = 0; i < ListeCamera.Count; ++i)
         {
            ListeCamera[i].Update(gameTime);
         }

         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
         //DepthStencilState depthBufferState = new DepthStencilState();
         //depthBufferState.DepthBufferEnable = true;
         //GraphicsDevice.DepthStencilState = depthBufferState;

         foreach (CaméraTracing c in ListeCamera)
         {
            GraphicsDevice.Viewport = (c as CaméraTracing).CameraViewPort;

            foreach (IModele3d i in ListeModele)
            {
               i.SetCaméra(c);
               i.Draw(gameTime);
            }
         }

         GraphicsDevice.Viewport = OptionsJeu.ViewportDéfaut;

         base.Draw(gameTime);
      }
   }
}
