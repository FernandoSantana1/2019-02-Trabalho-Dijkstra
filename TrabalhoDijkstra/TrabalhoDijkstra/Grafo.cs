using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoDijkstra
{
    class Grafo
    {
        private int[,] matriz;
        private int[] caminho;
        private int no;
        public Grafo(int _no)
        {
            no = _no;
            matriz = new int[no, no];
            caminho = new int[no];
        }
        public void Adicionar(int lin, int col, int peso)
        {
            matriz[lin, col] = peso;//guarda o peso na matriz 
        }
        public void MostrarMatriz()
        {
            int n = 0;
            int m = 0;
            Console.WriteLine("-- Matriz de Peso --");
            for (n = 0; n < no; n++)
            {
                Console.Write("\t{0}", n);//borda
            }
            Console.WriteLine();
            for (n = 0; n < no; n++)
            {
                Console.Write(n);//borda esquerda
                for (m = 0; m < no; m++)
                {
                    Console.Write("\t{0}", matriz[n, m]);//matriz
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine("----------");
        }
        public int PegarMatriz(int linha, int coluna)
        {
            return matriz[linha, coluna];
        }

    }
}
