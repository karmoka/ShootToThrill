using System;
using System.Collections;
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
using System.Diagnostics;

namespace AtelierXNA
{
    public class RequêtePathManager : DrawableGameComponent
    {
        Queue<RequêtePath> QueueRequêtePath = new Queue<RequêtePath>();
        RequêtePath RequêtePathActuel { get; set; }
        static RequêtePathManager Instance { get; set; }
        Pathfinding Pathfinding { get; set; }
        bool EstPathEnProcessus { get; set; }

        public RequêtePathManager(Game jeu)
            : base(jeu)
        {
            Instance = this;
        }

        public override void Initialize()
        {
            EstPathEnProcessus = false;
            Instance = this;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Pathfinding = Game.Services.GetService(typeof(Pathfinding)) as Pathfinding;
            base.LoadContent();
        }

        public static void PathRequête(Vector3 départPath, Vector3 finPath, Action<Vector3[], bool> callback)
        {
            RequêtePath nouvelleRequêtePath = new RequêtePath(départPath, finPath, callback);
            Instance.QueueRequêtePath.Enqueue(nouvelleRequêtePath);
            Instance.TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!EstPathEnProcessus && QueueRequêtePath.Count > 0)
            {
                RequêtePathActuel = QueueRequêtePath.Dequeue();
                EstPathEnProcessus = true;
                Pathfinding.StartFindingPath(RequêtePathActuel.DébutPath, RequêtePathActuel.FinPath);
            }
        }

        public void FinishingProcessingPath(Vector3[] path, bool succes)
        {
            RequêtePathActuel.CallBack(path, succes);
            EstPathEnProcessus = false;
            TryProcessNext();
        }
    }
}