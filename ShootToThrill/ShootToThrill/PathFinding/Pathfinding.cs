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
    public class Pathfinding : DrawableGameComponent
    {
        GrilleUniverselle GrilleUniverselle { get; set; }
        RequêtePathManager RequêtePathManager { get; set; }

        public Pathfinding(Game game) 
         : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            RequêtePathManager = Game.Services.GetService(typeof(RequêtePathManager)) as RequêtePathManager;
            GrilleUniverselle = Game.Services.GetService(typeof(GrilleUniverselle)) as GrilleUniverselle;
            base.LoadContent();
        }

        public void StartFindingPath(Vector3 positionDépart, Vector3 positionFin)
        {
            GrilleUniverselle.SetPositions(positionDépart, positionFin);
            TrouverPath(positionDépart, positionFin);
        }

        void TrouverPath(Vector3 positionDépart, Vector3 positionCible)
        {
            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;
            Node nodeDépart = GrilleUniverselle.NodePositionMonde(positionDépart);
            Node nodeCible = GrilleUniverselle.NodePositionMonde(positionCible);

            if (nodeCible.EstSurfacePourMarcher && nodeDépart.EstSurfacePourMarcher)
            {
                Heap<Node> openSet = new Heap<Node>((int)GrilleUniverselle.TailleMondeGrille.X * (int)GrilleUniverselle.TailleMondeGrille.Z);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(nodeDépart);

                while (openSet.Count > 0)
                {
                    Node nodeActuel = openSet.EnleverPremier();
                    closedSet.Add(nodeActuel);

                    if (nodeActuel == nodeCible)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node voisin in GrilleUniverselle.GetVoisins(nodeActuel))
                    {
                        if (!voisin.EstSurfacePourMarcher || closedSet.Contains(voisin))
                        {
                            continue;
                        }
                        int CoutNouveauMouvementVoisin = nodeActuel.GCost + GetDistance(nodeActuel, voisin);
                        if (CoutNouveauMouvementVoisin < voisin.GCost || !openSet.Contains(voisin))
                        {
                            voisin.SetGCost(CoutNouveauMouvementVoisin);
                            voisin.SetHCost(GetDistance(voisin, nodeCible));
                            voisin.SetParent(nodeActuel);
                            if (!openSet.Contains(voisin))
                            {
                                openSet.Add(voisin);
                            }
                            else
                            {
                                openSet.UpdateObjet(voisin);
                            }
                        }
                    }
                }
            }
            if (pathSuccess)
            {
                wayPoints = RetracerPath(nodeDépart, nodeCible);
            }
            RequêtePathManager.FinishingProcessingPath(wayPoints, pathSuccess);
        }

        Vector3[] RetracerPath(Node nodeDépart, Node nodeFin)
        {
            List<Node> path = new List<Node>();
            Node nodeActuelle = nodeFin;
            while (nodeActuelle != nodeDépart)
            {
                path.Add(nodeActuelle);
                nodeActuelle = nodeActuelle.Parent;
            }
            path.Add(nodeActuelle);
            Vector3[] wayPoints = SimplifyPath(path);
            Array.Reverse(wayPoints);
            path.Reverse();
            GrilleUniverselle.SetPath(path);
            return wayPoints;
        }

        public Node GetPositionActuelleMonde(Vector3 position)
        {
            return GrilleUniverselle.NodePositionMonde(position);
        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> wayPoints = new List<Vector3>();
            Vector2 ancienneDirection = Vector2.Zero;
            wayPoints.Add(path[0].PositionMonde);// + new Vector3(0.5f, 0, 0.5f));
            for (int i = 1; i < path.Count; ++i)
            {
                Vector2 nouvelleDrirection = new Vector2(path[i - 1].GrilleX - path[i].GrilleX, path[i - 1].GrilleZ - path[i].GrilleZ);
                if (nouvelleDrirection != ancienneDirection)
                {
                    wayPoints.Add(path[i].PositionMonde);// + new Vector3(0.5f, 0, 0.5f));
                }
                ancienneDirection = nouvelleDrirection;
            }
            return wayPoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Math.Abs(nodeA.GrilleX - nodeB.GrilleX);
            int distanceY = Math.Abs(nodeA.GrilleZ - nodeB.GrilleZ);
            return distanceX > distanceY ? 14 * distanceY + 10 * (distanceX - distanceY) : 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}