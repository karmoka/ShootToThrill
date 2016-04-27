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
using AnimationAux;

namespace AtelierXNA
{
    public class MAvatar : Entité, IPositionable, IModele3d, IPhysique
    {
       string NomModeleBase { get; set; }
       string NomModeleAnimé { get; set; }

        private float IntervalMAJ { get; set; }
        private float TempsDepuisMAJ { get; set; }
        protected float Rotation { get; set; }
        protected int indexArme;
        protected int Vie { get; set; }
        protected int VieMax { get; set; }
        protected bool EstEnMouvement { get; set; }
        protected bool EstAnimé { get; set; }

        public IModele3d ComposanteGraphique { get; private set; }
        public ObjetPhysique ComposantePhysique { get; private set; }
        protected List<IArme> ListeArme { get; set; }
        private AnimatedModel AnimationModèle { get; set; }

       /// <summary>
       /// Avatar dont le graphique ne bouge pas
       /// </summary>
       /// <param name="game"></param>
       /// <param name="composanteGraphique"></param>
       /// <param name="composantePhysique"></param>
        public MAvatar(Game game, IModele3d composanteGraphique, ObjetPhysique composantePhysique)
            : base(game)
        {
            ComposantePhysique = composantePhysique;
            ComposanteGraphique = composanteGraphique;
        }
       /// <summary>
       /// Avatar avec une seule animation qui est jouer lorsqu'il marche
       /// </summary>
       /// <param name="game"></param>
       /// <param name="ModèleADeBase"></param>
       /// <param name="ModèleAnimé"></param>
       /// <param name="composantePhysique"></param>
        public MAvatar(Game game, string modeleBase, string modeleAnimé, ObjetPhysique composantePhysique)
           : base(game)
        {
           ComposantePhysique = composantePhysique;
           NomModeleBase = modeleBase;
           NomModeleAnimé = modeleAnimé;
           EstAnimé = true;
        }

        public override void Initialize()
        {
           if(EstAnimé)
           {
              ComposanteGraphique = new AnimatedModel(Game, NomModeleBase, Vector3.Up * 3);
              AnimationModèle = new AnimatedModel(Game, NomModeleAnimé, Vector3.Up * 3);
              AnimationModèle.Initialize();
           }

           TypeEnt = TypeEntité.Avatar;
           ComposanteGraphique.Initialize();
           ComposantePhysique.Initialize();
            ListeArme = new List<IArme>();
            Vie = 100;
            VieMax = 100;
            IntervalMAJ = 1 / 60f;
            TempsDepuisMAJ = 0;
            IndexArme = 0;
            Rotation = 0;
            EstEnMouvement = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
           if(EstAnimé)
           {
              AnimationClip clip = AnimationModèle.Clips[0];
              AnimationPlayer player = (ComposanteGraphique as AnimatedModel).PlayClip(clip);
              player.Looping = true;
           }

           base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            TempsDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(TempsDepuisMAJ >= IntervalMAJ)
            {
                BougerAvatar();
                BougerArme();

                ComposanteGraphique.SetPosition(ComposantePhysique.Position);

                TempsDepuisMAJ = 0;
            }
            if (EstEnMouvement)
            {
               ComposanteGraphique.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public ObjetPhysique GetObjetPhysique()
        {
           return ComposantePhysique;
        }

        protected virtual void BougerAvatar()
        {

        }
        protected virtual void BougerArme()
        {
            if (ListeArme.Count != 0)
            {
                ListeArme[IndexArme].ChangerPosition(this);
                ListeArme[IndexArme].ChangerRotation(this);
            }
        }
        protected virtual void GérerCollisions()
        {
           for (int i = 0; i < ComposantePhysique.ListeCollision.Count; ++i)
           {

           }
        
        }

        public void AjouterArme(IArme arme)
        {
           if(ListeArme.Count(x => x == arme) < 1)
              ListeArme.Add(arme);
        }
       public void RerirerArme(IArme arme)
        {
           ListeArme.RemoveAll(x => x == arme);
        }

        public void AjouterMunition()
        {

        }

        public void Attaquer()
        {
            if (ListeArme.Count != 0)
                ListeArme[IndexArme].Attaquer();
        }
        public void Recharger()
        {
            if (ListeArme.Count != 0)
                ListeArme[IndexArme].Recharger();
        }

        public override void Draw(GameTime gameTime)
        {
            ComposanteGraphique.Draw(gameTime);
            ComposantePhysique.Draw(gameTime);

            base.Draw(gameTime);
        }

        public Vector3 Position
        {
            get { return ComposantePhysique.Position; }
        }
        public void SetPosition(Vector3 nouvellePosition)
        {
            ComposantePhysique.SetPosition(nouvellePosition);
            ComposanteGraphique.SetPosition(nouvellePosition);
        }
        public Vector3 Vitesse
        {
            get { return ComposantePhysique.Vitesse; }
            set
            {
                ComposantePhysique.SetVitesse(value);
            }
        }

        protected int IndexArme
        {
            get { return indexArme; }
            set
            {
                if (value >= 0 && value < ListeArme.Count)
                    indexArme = value;
            }
        }

        public void SetCaméra(Caméra cam)
        {
            ComposanteGraphique.SetCaméra(cam);
        }

        public void SetRotation(Vector3 rotation)
        {
            ComposanteGraphique.SetRotation(rotation);
            ComposantePhysique.SetRotation(rotation);
        }

       public Collider GetCollider()
        {
           return new SphereCollision(this.Position, 2f);
        }
    }
}
