using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
   public class GestionnairesLumières : GameComponent
   {
      List<Lumière> ListeLumières { get; set; }

      public GestionnairesLumières(Game game)
         :base(game)
      {
         ListeLumières = new List<Lumière>();
      }

      public Vector3[] Positions()
      {
         Vector3[] positions = new Vector3[ListeLumières.Count];
         for(int i=0;i<ListeLumières.Count;++i )
         {
            positions[i] = ListeLumières[i].Position;
         }
         return positions;
      }
      public Vector3[] Couleurs()
      {
         Vector3[] couleurs = new Vector3[ListeLumières.Count];
         for (int i = 0; i < ListeLumières.Count; ++i)
         {
            couleurs[i] = ListeLumières[i].Couleur;
         }
         return couleurs;
      }

      public float[] Rayons()
      {
         float[] rayons = new float[ListeLumières.Count];
         for (int i = 0; i < ListeLumières.Count; ++i)
         {
            rayons[i] = ListeLumières[i].Rayon;
         }
         return rayons;
      }
      public int NombreLumières()
      {
         return ListeLumières.Count;
      }

      public void AjouterLumières(Lumière l)
      {
         ListeLumières.Add(l);
      }

      public override void Update(GameTime gameTime)
      {
         for (int i = 0; i < ListeLumières.Count;++i )
         {
            if( ListeLumières[i] is LumièreTracing)
            {
               (ListeLumières[i] as LumièreTracing).Update();
            }
         }

         base.Update(gameTime);
      }
   }
}
