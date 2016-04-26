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
   public enum Sons { Menu, Menu_Selectionné, GameState_Change, GamePlay, Tir_Pistol, Tir_Shotgun, Tir_Mitraillette, Tir_Railgun, PickUp}

   public struct ItemAudio
   {
      public Cue audioCue;
      public bool Joue;
      public bool EnPause;
      public string Nom;
   };

   public class ManagerAudio : Microsoft.Xna.Framework.DrawableGameComponent
   {
      //string[] NomSons = { "MusiqueMenu", "SonsSelectionné","ChangementState","MusiqueGameplay","TirPistol","TirShotgun","TirMitraillette","TirRailgun","PickUpObjet"};
      const string RÉPERTOIRE_SONS = "Sounds";
      RessourcesManager<SoundEffect> GestionnaireAudio;
      List<ItemAudio> ListeSons { get; set; }

      private AudioEngine engine { get; set; }
      private WaveBank waveBank { get; set; }
      private SoundBank soundBank { get; set; }

      public ManagerAudio(Game game)
         : base(game)
      {
         engine = new AudioEngine("Content\\Sounds\\AudioEngine.xgs");
         soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
         waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");

         ListeSons = new List<ItemAudio>();
      }

      public void AjouterSons(string nom)
      {
         ItemAudio i = new ItemAudio{ audioCue = soundBank.GetCue(nom), EnPause =false, Joue = false, Nom = nom};
         ListeSons.Add(i);
      }
      public void JouerSons(string nom)
      {
         int position = -1;
         position = ListeSons.FindIndex(x => x.Nom == nom);

         if(position >= 0)
         {
            ItemAudio i = ListeSons[position];
            i.Joue = true;
            i.EnPause = false;
            ListeSons[position] = i;
            ListeSons[position].audioCue.Play();
         }
      }

      public void PauserSons(string nom)
      {
         int position = -1;
         position = ListeSons.FindIndex(x => x.Nom == nom);

         if (position >= 0)
         {
            ItemAudio i = ListeSons[position];
            i.Joue = false;
            i.EnPause = true;
            ListeSons[position] = i;
            ListeSons[position].audioCue.Pause();
         }
      }
      public void RésumerSons(string nom)
      {
         int position = -1;
         position = ListeSons.FindIndex(x => x.Nom == nom);

         if (position >= 0 && ListeSons[position].EnPause)
         {
            ItemAudio i = ListeSons[position];
            i.Joue = true;
            i.EnPause = false;
            ListeSons[position] = i;
            ListeSons[position].audioCue.Resume();
         }
      }
      public void ArrêterSons(string nom)
      {
         int position = -1;
         position = ListeSons.FindIndex(x => x.Nom == nom);

         if (position >= 0 && ListeSons[position].Joue)
         {
            ItemAudio i = ListeSons[position];
            i.Joue = false;
            i.EnPause = false;
            ListeSons[position] = i;
            ListeSons[position].audioCue.Stop(AudioStopOptions.Immediate);
         }
      }

      public void ArrêterTout()
      {
         foreach(ItemAudio i in ListeSons)
         {
            ArrêterSons(i.Nom);
         }
      }
      public void PauseTout()
      {
         foreach (ItemAudio i in ListeSons)
         {
            PauserSons(i.Nom);
         }
      }
      public override void Initialize()
      {
         base.Initialize();
      }

      protected override void LoadContent()
      {

         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
         engine.Update();
         base.Update(gameTime);
      }
   }
}
