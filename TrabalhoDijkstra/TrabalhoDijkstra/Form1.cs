using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabalhoDijkstra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int escala; Random random = new Random();
        private void MostrarRota(int[,] mapa, List<int> rota, int total)
        {
            int escala;
            escala = pictureBox1.Width / total;
            SolidBrush Solid = new SolidBrush(cor);
            Graphics Draw = pictureBox1.CreateGraphics();
            int p = 0;
            bool zero = false;
            for (int i = 0; i < total; i++)
            {
                for (int j = 0; j < total; j++)
                {
                    if (p < rota.Count)
                    {
                        if (rota.Contains(mapa[i, j]))//se a posicao atual da matriz for igual a alguma posicao da rota
                        {
                            Font fonte = new Font("Arial", escala / 5);
                            int y = (i * escala);// + escala / 4;
                            int x = (j * escala);// + escala / 4;

                            if (mapa[i, j] != 0)
                            {
                                string texto = mapa[i, j].ToString();
                                Solid.Color = Color.Green;
                                Draw.FillEllipse(Solid, x, y, escala / 3, escala / 3);
                                Solid.Color = Color.Black;
                                Draw.DrawString(texto, fonte, Solid, x + escala / 20, y + escala / 30);
                            }
                            if (mapa[i, j] == 0 && zero == false)
                            {
                                string texto = mapa[i, j].ToString();
                                Solid.Color = Color.Green;
                                Draw.FillEllipse(Solid, x, y, escala / 3, escala / 3);
                                Solid.Color = Color.Black;
                                Draw.DrawString(texto, fonte, Solid, x + escala / 20, y + escala / 30);
                                Solid.Color = Color.LightGray;
                                y = (i * escala) + escala / 4;
                                x = (j * escala) + escala / 4;
                                Draw.FillEllipse(Solid, x + escala / 13, y + escala / 13, escala / 3, escala / 3);
                                zero = true;
                            }
                            p++;
                        }
                    }
                }
            }
        }

        int vertices;
        private void BuscarCaminho(Grafo g, int vertices, int inicio, int fim, int[,] mapa)
        {
            int distancia, n, atual, coluna;
            int[,] tab = new int[vertices, 3];
            //0-Visitado 1-Distancia 2 - Anterior
            for (n = 0; n < vertices; n++)
            {
                tab[n, 0] = 0;
                tab[n, 1] = int.MaxValue;
                tab[n, 2] = 0;
            }
            tab[inicio, 1] = 0;//define a posicao inicial na tabela de peso como 0
            MostrarTabela(tab);

            atual = inicio;
            do
            {
                tab[atual, 0] = 1; //marca o no como visitado
                for (coluna = 0; coluna < vertices; coluna++)
                {
                    if (g.PegarMatriz(atual, coluna) != 0)//Verifica se tem conexao com outro vertice
                    {//calcula distancia
                        distancia = g.PegarMatriz(atual, coluna) + tab[atual, 1];//peso do proximo veritce + o que ja tem
                        if (distancia < tab[coluna, 1])
                        {
                            tab[coluna, 1] = distancia;
                            tab[coluna, 2] = atual;
                        }
                    }
                }
                //vertice com a menor distancia e que ainda nao foi visitado
                int indiceMenor = -1;
                int distanciaMenor = int.MaxValue;
                for (int x = 0; x < vertices; x++)
                {
                    if (tab[x, 1] < distanciaMenor && tab[x, 0] == 0)//Verifica se tem algum caminho com a distancia menor que a atual
                    {
                        indiceMenor = x;
                        distanciaMenor = tab[x, 1];
                    }
                }
                atual = indiceMenor;
            } while (atual != -1);
            MostrarTabela(tab);

            List<int> rota = new List<int>();
            while (fim != inicio)
            {
                rota.Add(fim);//adiciona de tras pra frente a rota para depois ser invertida e exibida...
                fim = tab[fim, 2];
            }
            rota.Add(inicio);
            rota.Reverse();
            foreach (int pos in rota)
            {
                Console.WriteLine("{0}->", pos);
            }
            Console.WriteLine();
            int total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(vertices) / 2)); //arredonda os valores para cima 
            lblCaminho.Text = "Caminho: [Asfalto] " + string.Join(" -> ", rota);//exibe o caminho
            MostrarRota(mapa, rota, total);
        }
        Color cor;
        private void DesenharQuadrados(Color cor, int vertices)
        {
            pictureBox1.Size = new Size(500, 500);//ajusta tamanho inicial
            escala = pictureBox1.Width / vertices;
            pictureBox1.Size = new Size(vertices * escala, vertices * escala);
            SolidBrush Solid = new SolidBrush(cor);
            Graphics Draw = pictureBox1.CreateGraphics();
            Draw.Clear(Color.LightGray);//limpa a picturebox
            for (int coluna = 0; coluna < vertices * escala; coluna += escala)
            {
                for (int linha = 0; linha < vertices * escala; linha += escala)
                {
                    Draw.FillRectangle(Solid, linha, coluna, 1, linha);//colunas
                    Draw.FillRectangle(Solid, linha, coluna, coluna, 1);//linhas
                }
            }
        }
        public static void MostrarTabela(int[,] tab)
        {
            int n = 0;
            for (n = 0; n < tab.GetLength(0); n++)
            {
                Console.WriteLine("{0}-> {1}\t{2}\t{3}", n, tab[n, 0], tab[n, 1], tab[n, 2]);

            }
            Console.WriteLine("-------");
            Console.WriteLine("[vert]~[peso]~[ant]");
        }

        private void AddGrafo(Grafo g, int vertices)
        {
            //matriz que guarda os caminhos e seus pesos (n x n)
            int peso;
            for (int x = 0; x < vertices; x++)
            {
                for (int y = 0; y < vertices; y++)
                {
                    peso = random.Next(0, 5);//peso/area = [0 = não há caminho, [1] = asfalto, [2] = grama, [3] = areia, [4] água
                    g.Adicionar(x, y, peso);//adiciona as areas e obstaculos(pesos)
                    g.Adicionar(y, x, peso);//2x para poder criar o caminho de volta (ex: [1] -> [2] & [2] -> [1])
                }
            }
        }
        private void MostrarMapa(bool coiote, bool player, int linha, int coluna, int vertices)
        {
            //0 = obstaculos 1 = asfalto 2 = grama 3 = areia 4 = agua 
            escala = pictureBox1.Width / vertices;
            SolidBrush Solid = new SolidBrush(cor);
            Graphics Draw = pictureBox1.CreateGraphics();
            Font fonte;
            string texto;
            int x, y;
            x = (coluna * escala) + escala / 4;
            y = (linha * escala) + escala / 4;
            if (player)
            {
                if (coiote)
                {
                    Solid.Color = Color.Red;
                    fonte = new Font("Arial", escala / 3);
                    texto = "C";
                    Draw.FillEllipse(Solid, x, y, escala / 2, escala / 2);
                    x += 1;
                    Solid.Color = Color.Black;
                    Draw.DrawString(texto, fonte, Solid, x, y);
                }
                else
                {
                    Solid.Color = Color.DeepSkyBlue;
                    fonte = new Font("Arial", escala / 3);
                    texto = "P";
                    Draw.FillEllipse(Solid, x, y, escala / 2, escala / 2);
                    x += 2;
                    Solid.Color = Color.Black;
                    Draw.DrawString(texto, fonte, Solid, x, y);
                }
            }
            else
            {
                texto = "*";
                Solid.Color = Color.Black;
                fonte = new Font("Arial", escala / 2);
                Draw.DrawString(texto, fonte, Solid, x, y);
            }
        }
        private void GerarMapa(int vertices, Grafo g)
        {
            int total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(vertices) / 2)); //arredonda os valores para cima 
            int[,] matrizMapa = new int[total, total];
            int[] pIF = new int[2]; //pos inicial e final
            int count = 0, countPos = 0;
            //gera as posicoes dos vertices na matriz 
            string valores;
            List<string> lista = new List<string>();
            while (count != vertices)
            {
                valores = random.Next(0, total).ToString();
                valores += "," + random.Next(0, total).ToString();
                if (!lista.Contains(valores.ToString()))//gera valor sem repetir
                {
                    lista.Add(valores);

                    string[] arrCoords = valores.Split(',');//separa as coodenadas x e y 
                    int l = Convert.ToInt32(arrCoords[0]);
                    int c = Convert.ToInt32(arrCoords[1]);
                    if (count >= vertices - 2)//Define a penultima e a ultima posição como inicial e final...
                    {
                        countPos++;
                        matrizMapa[l, c] = countPos; // 1 = inicial, 2 = final 
                        if (countPos == 2)//chama o metodo de busca qnd ja tiver os 2 valores (ini e fim)
                        {

                            MostrarMapa(false, true, l, c, total);
                        }
                        else if (countPos == 1)
                        {
                            MostrarMapa(true, true, l, c, total);
                        }

                    }
                    else
                    {
                        matrizMapa[l, c] = count + 3; // outros vertices...
                    }
                    count++;
                }
            }

            Console.WriteLine("-- Matriz Mapa --");
            for (int i = 0; i < total; i++)
            {

                for (int j = 0; j < total; j++)
                {
                    Console.Write("\t{0}", matrizMapa[i, j]);//exibe a matriz mapa
                    if (matrizMapa[i, j] == 0 && matrizMapa[i, j] == 0)
                    {

                        MostrarMapa(false, false, i, j, total);//adiciona os *
                    }

                }
                Console.WriteLine();
            }
            BuscarCaminho(g, vertices, 1, 2, matrizMapa);//busca caminho 1 -> 2
        }

        private void button5_Click(object sender, EventArgs e)
        {
            vertices = random.Next(4, 15) + 1;
            int total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(vertices) / 2)); //arredonda os valores para cima 
            Grafo g = new Grafo(vertices);
            AddGrafo(g, vertices);
            DesenharQuadrados(Color.Black, total);
            g.MostrarMatriz();
            GerarMapa(vertices, g);
        }
    }
}
