using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    public class Options : GameComponent
    {
        public Options(Game game)
            :base(game)
        {
            AssignerValeurDéfaut();
        }
        public Options(Game game, DescriptionOptions description)
            : base(game)
        {
            LireFichier(description);
        }

        void LireFichier(DescriptionOptions description)
        {
            IntervalMAJStandard = description.IntervalMAJStandard;
            WindowWidth = description.WindowWidth;
            WindowHeight = description.WindowHeight;

            CameraHeightOffset = description.CameraHeightOffset;
            CameraDistanceOffset = description.CameraDistanceOffset;

            ViePersonnageMax = description.ViePersonnageMax;
            SensibilitéFriction = description.SensibilitéFriction;
            RatioDistance3dMetre = description.RatioDistance3dMetre;

            Gravité = description.Gravité;
            NomFontMenu = description.NomFontMenu;

            NbAvatar = 4;
        }

        void AssignerValeurDéfaut()
        {
            IntervalMAJStandard = 1 / 60f;
            WindowWidth = 1000;
            WindowHeight = 700;

            CameraHeightOffset = 5;
            CameraDistanceOffset = 5;

            ViePersonnageMax = 100;
            SensibilitéFriction = 0.01f;
            RatioDistance3dMetre = 3f;

            Gravité = new Vector3(0, -9.8f, 0);
            NomFontMenu = "ArialDebug";

            NbAvatar = 4;
        }

        public float IntervalMAJStandard { get; private set; }

        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        public int NbAvatar { get; private set; }

        public float CameraHeightOffset { get; private set; }
        public float CameraDistanceOffset { get; private set; }

        public int ViePersonnageMax { get; private set; }

        public float SensibilitéFriction { get; private set; }
        public float RatioDistance3dMetre { get; private set; } //5 unité de distance par metre
        public Vector3 Gravité { get; private set; }

        public void ChangerGravité()
        {
            Gravité = new Vector3(Gravité.Y, Gravité.Z, Gravité.X);
        }

        public string NomFontMenu { get; private set; }

        public Viewport ViewportDéfaut
        {
            get { return new Viewport(0, 0, WindowWidth, WindowHeight);}
        }
    }

   public class GameConstants
   {
      static public float INTERVAL_MAJ_STANDARD = 1 / 60f;

      static public int WindowWidth = 1000;
      static public int WindowHeight = 700;

      static public float CameraHeightOffset = 5;
      static public float CameraDistanceOffset = 5;

      static public int ViePersonnageMax = 100;

      static public float SensibilitéFriction = 0.01f;
      static public float RatioDistance3dMetre = 3f; //5 unité de distance par metre
      static public Vector3 Gravité = new Vector3(0, -9.8f, 0) / RatioDistance3dMetre;

      static public string NomFontMenu = "ArialDebug";

      static public Viewport ViewportDéfaut = new Viewport(0, 0, WindowWidth, WindowHeight);

      static public int NB_AVATAR = 2;
   }

   public abstract class DefinitionGamePad
   {
      public abstract Keys KSaut { get; }
      public abstract Keys KDroit { get; }
      public abstract Keys KGauche { get; }
      public abstract Keys KHaut { get; }
      public abstract Keys KBas { get; }
      public abstract Keys KChangerArmeHaut { get; }
      public abstract Keys KChangerArmeBas { get; }
      public abstract ButtonState BTirer { get; }
      public abstract ButtonState BRecharger { get; }
      public abstract Keys KTirer { get; }
      public abstract Keys KRecharger { get; }
      public abstract Buttons BSaut { get; }
      //public abstract Buttons Direction { get; }

      public static DefinitionGamePad DistribuerCommandes(PlayerIndex p)
      {
         DefinitionGamePad déf = null;
         if (p == PlayerIndex.One)
         {
            déf = new DéfinitionControleJ1();
         }
         if (p == PlayerIndex.Two)
         {
            déf = new DéfinitionControleJ2(); 
         }
         if (p == PlayerIndex.Three)
         {
             déf = new DéfinitionControleJ1(); //TODO changer ca
         }
         if (p == PlayerIndex.Four)
         {
             déf = new DéfinitionControleJ2(); //TODO changer ca
         }

         return déf;
      }

   }

   public class DéfinitionControleJ1 : DefinitionGamePad
   {
      public override Keys KSaut { get { return Keys.L; } }
      public override Keys KDroit { get { return Keys.Right; } }
      public override Keys KGauche { get { return Keys.Left; } }
      public override Keys KHaut { get { return Keys.Up; } }
      public override Keys KBas { get { return Keys.Down; } }
      public override Keys KChangerArmeHaut { get { return Keys.NumPad1; } }
      public override Keys KChangerArmeBas { get { return Keys.NumPad2; } }
      public override Keys KTirer { get { return Keys.X; } }
      public override Keys KRecharger { get { return Keys.C; } }
      public override ButtonState BTirer { get { return Mouse.GetState().LeftButton; } }
      public override ButtonState BRecharger { get { return Mouse.GetState().RightButton; } }
      public override Buttons BSaut { get { return Buttons.A; } }

   }
   public class DéfinitionControleJ2 : DefinitionGamePad
   {
      public override Keys KSaut { get { return Keys.Space; } }
      public override Keys KDroit { get { return Keys.D; } }
      public override Keys KGauche { get { return Keys.A; } }
      public override Keys KHaut { get { return Keys.W; } }
      public override Keys KBas { get { return Keys.S; } }
      public override Keys KChangerArmeHaut { get { return Keys.Q; } }
      public override Keys KChangerArmeBas { get { return Keys.R; } }
      public override Keys KTirer { get { return Keys.N; } }
      public override Keys KRecharger { get { return Keys.M; } }
      public override ButtonState BTirer { get { return Mouse.GetState().XButton1; } }
      public override ButtonState BRecharger { get { return Mouse.GetState().XButton2; } }
      public override Buttons BSaut { get { return Buttons.A; } }
   }

   //public class ControlesJ1
   //{
   //   public static Keys JSaut = Keys.NumPad1;
   //   public static Keys JDroit = Keys.Right;
   //   public static Keys JGauche = Keys.Left;
   //   public static Keys JHaut = Keys.Up;
   //   public static Keys JBas = Keys.Down;
   //   public static ButtonState JTir = Mouse.GetState().LeftButton;
   //   public static Keys JChangerArmeHaut = Keys.NumPad1;
   //   public static Keys JChangerArmeBas = Keys.NumPad2;
   //}
   //public class ControlesJ2
   //{
   //   public static Keys JSaut = Keys.Space;
   //   public static Keys JDroit = Keys.D;
   //   public static Keys JGauche = Keys.A;
   //   public static Keys JHaut = Keys.W;
   //   public static Keys JBas = Keys.S;
   //   public static ButtonState JTir = Mouse.GetState().RightButton;
   //   public static Keys JChangerArmeHaut = Keys.Q;
   //   public static Keys JChangerArmeBas = Keys.R;
   //}
}
