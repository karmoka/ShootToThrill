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
    public class GrilleUniverselle : GameComponent, IModele3d
    {
        const int SOL = 0;
        Color PLANCHER = Color.White,
            MUR = Color.Black,
            PORTAIL_JOUEUR = Color.Lime,
            PORTAIL_ENNEMI = Color.Red,
            INTERRUPTEUR = Color.Blue;

        RessourcesManager<Texture2D> GestionnaireDeTexture { get; set; }
        Texture2D Image�tage { get; set; }
        Jeu Jeu { get; set; }
        Color[,] Couleurs { get; set; }
        List<string> Liste�tage { get; set; }
        CubeAdditionnable[, ,] TableauCube { get; set; }
        int �tage { get; set; }
        public List<CubeAdditionnable> ListeCube { get; private set; }
        bool MontrerGizmosSurGrille { get; set; }
        public List<Node> Path { get; private set; }
        public Vector3 TailleMondeGrille { get; private set; }
        Node[, ,] TableauNode { get; set; }
        public Vector3 PositionCible { get; private set; }
        public Vector3 PositionActuelle { get; private set; }
        Vector3 CentreMap { get; set; }
        float �chelle { get; set; }
        public GrilleUniverselle(Game game, List<string> liste�tage)
            : base(game)
        {
            Liste�tage = liste�tage;
        }

        public void SetPosition(Vector3 position)
        {

        }

        public void SetRotation(Vector3 rotation)
        {

        }

        public override void Initialize()
        {
            MontrerGizmosSurGrille = false;
            Path = new List<Node>();
            PositionCible = new Vector3(1, 3, 1);
            PositionActuelle = new Vector3(38, 2, 28);
            �tage = SOL;
            ListeCube = new List<CubeAdditionnable>();
            �chelle = 1f;
            Jeu = Game.Services.GetService(typeof(Jeu)) as Jeu;
            GestionnaireDeTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            foreach (string s in Liste�tage)
            {
                Ajouter�tage(s);
            }
            InitialiserMap();

            base.Initialize();
        }


        #region Image
        void Cr�erSol()
        {
            TailleMondeGrille = new Vector3(Image�tage.Width, Liste�tage.Count + 1, Image�tage.Height);
            TableauNode = new Node[Image�tage.Width, Liste�tage.Count + 1, Image�tage.Height];

            //Tableau de cubes additionnables, lors de son initialisation, il y a un cube par pixel noir dans une image Bitmap.
            TableauCube = new CubeAdditionnable[Image�tage.Width, Liste�tage.Count + 1, Image�tage.Height];
            for (int x = 0; x < Image�tage.Width; ++x)
            {
                for (int z = 0; z < Image�tage.Height; ++z)
                {
                    Vector3 position = new Vector3(x, �tage, z);
                    CubeAdditionnable cube = new CubeAdditionnable(Game, �chelle, Vector3.Zero, position, Color.BlueViolet, Vector3.One, 0);
                    cube.Initialize();
                    TableauCube[x, �tage, z] = cube;
                    TableauNode[x, �tage, z] = new Node(Game, false, position, x, z);
                }
            }
        }

        //Fonction � Mikael
        void Ajouter�tage(string nomImage�tage)
        {
            Image�tage = GestionnaireDeTexture.Find(nomImage�tage);
            if (�tage == 0)
            {
                Cr�erSol();
            }
            �tage = int.Parse(nomImage�tage.Split('_')[1]);
            Couleurs = TextureVers2DColor();
            Cr�er�tage(nomImage�tage);
        }

        //Fonction � Mikael 
        Color[,] TextureVers2DColor()
        {
            Color[] tableau1D = new Color[Image�tage.Width * Image�tage.Height];
            Image�tage.GetData<Color>(tableau1D);

            Color[,] tableau2D = new Color[Image�tage.Width, Image�tage.Height];
            for (int i = 0; i < Image�tage.Width; i++) //Convert!
            {
                for (int j = 0; j < Image�tage.Height; j++)
                {
                    tableau2D[i, j] = tableau1D[i + j * Image�tage.Width];
                }
            }
            return tableau2D;
        }

        //Fonction � Mikael 
        void Cr�er�tage(string nomImage�tage)
        {
            for (int x = 0; x < Couleurs.GetLength(0); ++x)
            {
                for (int z = 0; z < Couleurs.GetLength(1); ++z)
                {
                    Vector3 position = new Vector3(x, �tage, z);
                    if (Couleurs[x, z] == PLANCHER)
                    {
                        TableauNode[x, �tage, z] = new Node(Game, TableauCube[x, �tage - 1, z] != null, position, x, z);
                    }
                    else if (Couleurs[x, z] == MUR)
                    {
                        CubeAdditionnable cube = new CubeAdditionnable(Game, �chelle, Vector3.Zero, position, Color.BlueViolet, Vector3.One, 0);
                        cube.Initialize();
                        TableauCube[x, �tage, z] = cube;
                        TableauNode[x, �tage, z] = new Node(Game, false, position, x, z);
                    }
                    else if (Couleurs[x, z] == PORTAIL_JOUEUR)
                    {
                        TableauNode[x, �tage, z] = new Node(Game, TableauCube[x, �tage - 1, z] != null, position, x, z);
                        Jeu.SetPositionPortailJoueur(position);
                    }
                    else if (Couleurs[x, z] == PORTAIL_ENNEMI)
                    {
                        TableauNode[x, �tage, z] = new Node(Game, TableauCube[x, �tage - 1, z] != null, position, x, z);
                        Jeu.SetPositionPortailEnnemi(position);
                    }
                    else if (Couleurs[x, z] == INTERRUPTEUR)
                    {
                        TableauNode[x, �tage, z] = new Node(Game, TableauNode[x, �tage - 1, z] != null, position, x, z);
                        Jeu.SetPositionInterrupteur(position);
                    }
                }
            }
        }
        #endregion

        #region Map
        void InitialiserMap()
        {
            foreach (CubeAdditionnable c in TableauCube)
            {
                if (c != null)
                {
                    ListeCube.Add(c);
                }
            }

            //L'image Bitmap vue de haut peut �tre s�par�e en colonnes et en lignes
            AdditionnerColonnes();
            AdditionnerLignes();
            Additionner�tages();
            AjouterBordures();
        }



        //Additionne tout les cubes qui se situe dans la m�me colonne
        void AdditionnerColonnes()
        {
            for (int y = 0; y < TableauCube.GetLength(1); ++y)
            {
                for (int z = 0; z < TableauCube.GetLength(2); ++z)
                {
                    for (int x = 0; x < TableauCube.GetLength(0) - 1; ++x)
                    {
                        //Pour chaque cube dans la liste, ces deux "if" d�terminent si les cubes existent et peuvent �tre additionn�s. 
                        if (TableauCube[x, y, z] != null && TableauCube[x + 1, y, z] != null)
                        {
                            if (EstAdditionnable(TableauCube[x, y, z], TableauCube[x + 1, y, z]))
                            {
                                //Ici il y a une surcharge de l'op�rateur "+", cette fonctione se trouve dans la classe CubeAdditionnable,
                                //elle additionne les dimensions de deux cubes et elle retourne un nouveau cube avec ces dimensions.
                                CubeAdditionnable nouveauCube = TableauCube[x, y, z] + TableauCube[x + 1, y, z];
                                nouveauCube.Initialize();
                                //Les anciens prismes sont supprim�s pour laisser place au nouveau.
                                ListeCube.Remove(TableauCube[x, y, z]);
                                ListeCube.Remove(TableauCube[x + 1, y, z]);
                                ListeCube.Add(nouveauCube);

                                //Dans le tableau des cubes, toutes les cases occup�es par les 
                                //anciens prismes contiennent maintenant la m�me information, soit le nouveau cube.
                                //Cette boucle regarde toutes les cases de la m�me colonne et qui sont occup�es par les anciens prismes. 
                                for (int k = 0; k < nouveauCube.CubeColor�.Dimension.X; ++k)
                                {
                                    TableauCube[x + 1 - k, y, z] = nouveauCube;
                                }

                            }
                        }
                    }
                }
            }
        }


        //M�me principe que pour les colonnes, sauf que c'est pour les lignes
        void AdditionnerLignes()
        {
            for (int y = 0; y < TableauCube.GetLength(1); ++y)
            {
                for (int x = 0; x < TableauCube.GetLength(0); ++x)
                {
                    for (int z = 0; z < TableauCube.GetLength(2) - 1; ++z)
                    {
                        if (TableauCube[x, y, z] != null && TableauCube[x, y, z + 1] != null)
                        {
                            //Ici le if est diff�rent car il se peut que deux prismes voulant s'additionner 
                            //n'aient pas les bonnes dimensions en coordonn�e X.
                            if (EstAdditionnable(TableauCube[x, y, z], TableauCube[x, y, z + 1]) && BonneDimensionX(TableauCube[x, y, z], TableauCube[x, y, z + 1]))
                            {
                                CubeAdditionnable nouveauCube = TableauCube[x, y, z] + TableauCube[x, y, z + 1];
                                nouveauCube.Initialize();
                                ListeCube.Remove(TableauCube[x, y, z]);
                                ListeCube.Remove(TableauCube[x, y, z + 1]);
                                ListeCube.Add(nouveauCube);


                                //Cette boucle regarde toutes les cases dans la m�me surface Largeur * Longueur du nouveau cube.
                                for (int k = 0; k < nouveauCube.CubeColor�.Dimension.X; ++k)
                                {
                                    for (int h = 0; h < nouveauCube.CubeColor�.Dimension.Z; ++h)
                                    {
                                        TableauCube[x + k, y, z + 1 - h] = nouveauCube;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void Additionner�tages()
        {
            for (int x = 0; x < TableauCube.GetLength(0); ++x)
            {
                for (int y = 0; y < TableauCube.GetLength(1) - 1; ++y)
                {
                    for (int z = 0; z < TableauCube.GetLength(2); ++z)
                    {
                        if (TableauCube[x, y, z] != null && TableauCube[x, y + 1, z] != null)
                        {
                            //Ici le "if" est encore diff�rent car il regarde non seulement si les prismes ont les bonnes dimensions, mais aussi
                            // s'ils sont superpos�s parfaitement. 
                            if (BonneDirection(TableauCube[x, y, z], TableauCube[x, y + 1, z]) && BonneDimensionY(TableauCube[x, y, z], TableauCube[x, y + 1, z]))
                            {
                                CubeAdditionnable nouveauCube = TableauCube[x, y, z] + TableauCube[x, y + 1, z];
                                nouveauCube.Initialize();
                                ListeCube.Remove(TableauCube[x, y, z]);
                                ListeCube.Remove(TableauCube[x, y + 1, z]);
                                ListeCube.Add(nouveauCube);

                                //Cette boucle remplace toutes les cases occup�es par le volume du prisme. 
                                for (int g = 0; g < nouveauCube.CubeColor�.Dimension.Y; ++g)
                                {
                                    for (int h = 0; h < nouveauCube.CubeColor�.Dimension.X; ++h)
                                    {
                                        for (int k = 0; k < nouveauCube.CubeColor�.Dimension.Z; ++k)
                                        {
                                            TableauCube[x - h, y - g + 1, z - k] = nouveauCube;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Cette fonction ajoute quatres murs aux bords du terrain pour �viter que les avatars tombent en dehors du terrain. 
        void AjouterBordures()
        {
            CubeAdditionnable gauche = new CubeAdditionnable(Game, 1, Vector3.Zero, new Vector3(-1, 10, (Image�tage.Height / 2)), Color.Transparent, new Vector3(1, 20, Image�tage.Height), 0);
            CubeAdditionnable droite = new CubeAdditionnable(Game, 1, Vector3.Zero, new Vector3((Image�tage.Width) + 1, 10, (Image�tage.Height / 2)), Color.Transparent, new Vector3(1, 20, Image�tage.Height), 0);
            CubeAdditionnable haut = new CubeAdditionnable(Game, 1, Vector3.Zero, new Vector3((Image�tage.Width / 2), 10, -1), Color.Transparent, new Vector3(Image�tage.Width, 20, 1), 0);
            CubeAdditionnable bas = new CubeAdditionnable(Game, 1, Vector3.Zero, new Vector3((Image�tage.Width / 2), 10, (Image�tage.Height) + 1), Color.Transparent, new Vector3(Image�tage.Width, 20, 1), 0);

            gauche.Initialize();
            droite.Initialize();
            haut.Initialize();
            bas.Initialize();


            //Selon la position de la cam�ra et son orientation, ces deux murs devaient �tre transparent pour ne pas g�ner le joueur.
            droite.EstTransparent = true;
            bas.EstTransparent = true;

            ListeCube.Add(gauche);
            ListeCube.Add(droite);
            ListeCube.Add(haut);
            ListeCube.Add(bas);
        }

        //Cette fonction v�rifie si les dimensions sont bonnes. 
        bool EstAdditionnable(CubeAdditionnable cube1, CubeAdditionnable cube2)
        {
            Vector3 direction = cube1.Position - cube2.Position;
            return (direction.X != 0 && direction.Z == 0 || direction.Z != 0 && direction.X == 0);
        }

        //Celle-ci v�rifie que les prismes sont bien superpos�s.
        bool BonneDirection(CubeAdditionnable cube1, CubeAdditionnable cube2)
        {
            Vector3 direction = cube1.Position - cube2.Position;
            return direction.X == 0 && direction.Z == 0;
        }

        //Celle-ci v�rifie que les prismes ont la m�me dimension en coordonn�e X.
        bool BonneDimensionX(CubeAdditionnable cube1, CubeAdditionnable cube2)
        {
            return cube1.CubeColor�.Dimension.X == cube2.CubeColor�.Dimension.X;
        }

        //Celle-ci v�rifie que deux prismes ont la m�me surface en Longueur * Largeur. 
        bool BonneDimensionY(CubeAdditionnable cube1, CubeAdditionnable cube2)
        {
            return cube1.CubeColor�.Dimension.X == cube2.CubeColor�.Dimension.X && cube1.CubeColor�.Dimension.Z == cube2.CubeColor�.Dimension.Z;
        }
        #endregion

        //R�gion � Mikael
        #region Pathfinding
        public void SetPath(List<Node> path)
        {
            Path = path;
        }

        public void SetPositions(Vector3 positionActuelle, Vector3 positionCible)
        {
            PositionActuelle = positionActuelle;
            PositionCible = positionCible;
        }

        public List<Node> GetVoisins(Node node)
        {
            List<Node> voisins = new List<Node>();
            for (int x = -1; x <= 1; ++x)
            {
                for (int z = -1; z <= 1; ++z)
                {
                    if (Math.Abs(x) != Math.Abs(z))
                    {
                        int checkX1 = node.GrilleX + x;
                        int checkZ1 = node.GrilleZ + z;
                        if (checkX1 >= 0 && checkX1 < TailleMondeGrille.X && checkZ1 >= 0 && checkZ1 < TailleMondeGrille.Z)
                        {
                            if (GetNode(checkX1 - x, checkZ1).EstSurfacePourMarcher || GetNode(checkX1, checkZ1 - z).EstSurfacePourMarcher)
                            {
                                int hauteurNodeActuelle = GetYNode(node.GrilleX, node.GrilleZ);
                                int hauteurNodeVoisin = GetYNode(checkX1, checkZ1);
                                Node nodeVoisin = GetNode(checkX1, checkZ1);
                                if (nodeVoisin != null)
                                {
                                    if (hauteurNodeActuelle >= hauteurNodeVoisin || hauteurNodeActuelle + 1 == hauteurNodeVoisin)
                                    {
                                        voisins.Add(nodeVoisin);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return voisins;
        }

        public Node NodePositionMonde(Vector3 positionMonde)
        {
            float pourcentageX = positionMonde.X / TailleMondeGrille.X; //(positionMonde.X + TailleMondeGrille.X / 2) / TailleMondeGrille.X;
            float pourcentageZ = positionMonde.Z / TailleMondeGrille.Z; //(positionMonde.Z + TailleMondeGrille.Z / 2) / TailleMondeGrille.Z;
            pourcentageX = pourcentageX < 0 ? 0 : pourcentageX > 1 ? 1 : pourcentageX;
            pourcentageZ = pourcentageZ < 0 ? 0 : pourcentageZ > 1 ? 1 : pourcentageZ;
            int x = (int)Math.Round((TailleMondeGrille.X -1) * pourcentageX);//(int)Math.Round((TailleMondeGrille.X - 1) * pourcentageX);
            int z = (int)Math.Round((TailleMondeGrille.Z -1) * pourcentageZ); //(int)Math.Round((TailleMondeGrille.Z - 1) * pourcentageZ);
            return GetNode(x, z);
        }

        Node GetNode(int x, int z)
        {
            int h = 0;
            for (int y = 0; y < TableauNode.GetLength(1); ++y)
            {
                if (TableauNode[x, y, z] != null)
                {
                    if (TableauNode[x, y, z].EstSurfacePourMarcher)
                    {
                        h = y;
                        break;
                    }
                }
            }
            return TableauNode[x, h, z];
        }

        int GetYNode(int x, int z)
        {
            int h = 0;
            for (int y = 0; y < TableauNode.GetLength(1); ++y)
            {
                if (TableauNode[x, y, z] != null)
                {
                    if (TableauNode[x, y, z].EstSurfacePourMarcher)
                    {
                        h = y;
                        break;
                    }
                }
            }
            return h;
        }

        public void Draw(GameTime gameTime)
        {
            if (TableauNode != null && MontrerGizmosSurGrille)
            {
                Node nodeJoueur = NodePositionMonde(PositionCible);
                Node nodeActuelle = NodePositionMonde(PositionActuelle);
                for (int x = 0; x < TableauNode.GetLength(0); ++x)
                {
                    for (int z = 0; z < TableauNode.GetLength(2); ++z)
                    {
                        int y = GetYNode(x, z);
                        if (Path != null && TableauNode[x, y, z].EstSurfacePourMarcher)
                        {
                            if (Path.Any(n => n.PositionMonde == TableauNode[x, y, z].GetPosition() && n == nodeJoueur))
                            {
                                TableauNode[x, y, z].InitialiserSommets(Color.Green);
                            }
                            else if (TableauNode[x, y, z].GetPosition() == nodeActuelle.PositionMonde)
                            {
                                TableauNode[x, y, z].InitialiserSommets(Color.Yellow);
                            }
                            else if (Path.Any(n => n.PositionMonde == TableauNode[x, y, z].GetPosition()))
                            {
                                TableauNode[x, y, z].InitialiserSommets(Color.Blue);
                            }
                            else if (GetNode(x, z).EstSurfacePourMarcher)
                            {
                                TableauNode[x, y, z].InitialiserSommets(Color.White);
                            }
                            if (TableauNode[x, y, z].Carr�Color�.Couleur != Color.White)
                            {
                                TableauNode[x, y, z].Draw(gameTime);
                            }
                        }
                    }
                }
            }
            foreach (CubeAdditionnable c in ListeCube)
            {
                c.Draw(gameTime);
            }
            //base.Draw(gameTime);
        }
        #endregion

        public void SetCam�ra(Cam�ra cam)
        {
           if (MontrerGizmosSurGrille)
           {
              foreach (Node n in TableauNode)
              {
                 if (n != null)
                 {
                    n.SetCam�ra(cam);
                 }
              }
           }

            foreach (CubeAdditionnable c in ListeCube)
            {
                c.SetCam�ra(cam);
            }
            Jeu.SetCam�ra(cam);
        }
    }
}