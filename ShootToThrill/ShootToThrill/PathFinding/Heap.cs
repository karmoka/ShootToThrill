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
    public class Heap<T> where T : IHeapObjet<T>
    {
        T[] TableauObjets { get; set; }
        int NombreObjetsActuel { get; set; }
        public int Count
        {
            get
            {
                return NombreObjetsActuel;
            }
        }

        public Heap(int tailleMaxHeap)
        {
            TableauObjets = new T[tailleMaxHeap];
        }

        public bool Contains(T unObjet)
        {
            return Equals(TableauObjets[unObjet.HeapIndex], unObjet);
        }

        public void Add(T unObjet)
        {
            unObjet.HeapIndex = NombreObjetsActuel;
            TableauObjets[NombreObjetsActuel] = unObjet;
            SortUp(unObjet);
            ++NombreObjetsActuel;
        }

        public T EnleverPremier()
        {
            T premierObjet = TableauObjets[0];
            --NombreObjetsActuel;
            TableauObjets[0] = TableauObjets[NombreObjetsActuel];
            TableauObjets[0].HeapIndex = 0;
            SortDown(TableauObjets[0]);
            return premierObjet;
        }

        public void UpdateObjet(T unObjet)
        {
            SortUp(unObjet);
        }

        void SortDown(T unObjet)
        {
            while (true)
            {
                int indexEnfantGauche = unObjet.HeapIndex * 2 + 1;
                int indexEnfantDroit = unObjet.HeapIndex * 2 + 2;
                int unAutreIndex = 0;
                if (indexEnfantGauche < NombreObjetsActuel)
                {
                    unAutreIndex = indexEnfantGauche;
                    if (indexEnfantDroit < NombreObjetsActuel)
                    {
                        if (TableauObjets[indexEnfantGauche].CompareTo(TableauObjets[indexEnfantDroit]) < 0)
                        {
                            unAutreIndex = indexEnfantDroit;
                        }
                    }
                    if (unObjet.CompareTo(TableauObjets[unAutreIndex]) < 0)
                    {
                        Swap(unObjet, TableauObjets[unAutreIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        void SortUp(T unObjet)
        {
            int indexParent = (unObjet.HeapIndex - 1) / 2;
            while (true)
            {
                T objetParent = TableauObjets[indexParent];
                if (unObjet.CompareTo(objetParent) > 0)
                {
                    Swap(unObjet, objetParent);
                }
                else
                {
                    break;
                }
                indexParent = (unObjet.HeapIndex - 1) / 2;
            }
        }

        void Swap(T objetA, T objetB)
        {
            TableauObjets[objetA.HeapIndex] = objetB;
            TableauObjets[objetB.HeapIndex] = objetA;
            int indexObjetA = objetA.HeapIndex;
            objetA.HeapIndex = objetB.HeapIndex;
            objetB.HeapIndex = indexObjetA;
        }
    }
}