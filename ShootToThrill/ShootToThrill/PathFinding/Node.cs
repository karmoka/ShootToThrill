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

    public class Node : DrawableGameComponent, IHeapObjet<Node>, IModele3d
    {
        int _heapIndex;
        public bool EstSurfacePourMarcher { get; private set; }
        public Vector3 PositionMonde { get; private set; }
        public int GrilleX { get; private set; }
        public int GrilleZ { get; private set; }
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost
        {
            get
            {
                return GCost + HCost;
            }
        }
        public Node Parent { get; private set; }
        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }
        public void SetPosition(Vector3 position)
        {

        }
        public void SetRotation(Vector3 rotation)
        {

        }
        public CarréColoré CarréColoré { get; private set; }

        public Node(Game game, bool walkable, Vector3 positionMonde, int grilleX, int grilleZ)
            : base(game)
        {
            EstSurfacePourMarcher = walkable;
            PositionMonde = positionMonde;
            GrilleX = grilleX;
            GrilleZ = grilleZ;
            CarréColoré = new CarréColoré(game, 0.9f, Vector3.Zero, positionMonde, Color.White, Vector3.One, 3f);
            CarréColoré.Initialize();
        }

        public int CompareTo(Node nodeComparée)
        {
            int comparaison = FCost.CompareTo(nodeComparée.FCost);
            if (comparaison == 0)
            {
                comparaison = HCost.CompareTo(nodeComparée.HCost);
            }
            return -comparaison;
        }

        public void SetParent(Node parent)
        {
            Parent = parent;
        }

        public void SetGCost(int gCost)
        {
            GCost = gCost;
        }

        public void SetHCost(int hCost)
        {
            HCost = hCost;
        }

        public Vector3 GetPosition()
        {
            return CarréColoré.Position;
        }

        public void InitialiserSommets(Color couleur)
        {
            CarréColoré.InitialiserSommets(couleur);
        }

        public void SetCaméra(Caméra cam)
        {
            CarréColoré.SetCaméra(cam);
        }

        public override void Draw(GameTime gameTime)
        {
            CarréColoré.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}