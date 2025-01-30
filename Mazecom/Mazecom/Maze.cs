using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazecom
{


    class Maze
    {
        public int xMap = 300, yMap = 30;
        public int broadTile = 30, highTile = 30;
        public char[,] maze;

        public  void GenerarLaberinto(int n)
        {
            // Inicializar la matriz con paredes (#)
            maze = new char[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    maze[i, j] = '#';
                }
            }

            // Definir el centro de la matriz
            int centro = n / 2;

            // Función recursiva para generar el maze
            void CarvePath(int x, int y)
            {
                // Definir las direcciones posibles (arriba, derecha, abajo, izquierda)
                int[][] direcciones = new int[][]
                {
                new int[] {0, 1},
                new int[] {1, 0},
                new int[] {0, -1},
                new int[] {-1, 0}
                };
                Random rnd = new Random();
                direcciones = direcciones.OrderBy(d => rnd.Next()).ToArray(); // Aleatorizar las direcciones

                foreach (var d in direcciones)
                {
                    int nx = x + d[0], ny = y + d[1];
                    if (nx > 0 && ny > 0 && nx < n - 1 && ny < n - 1 && maze[nx, ny] == '#')
                    {
                        if (maze[nx + d[0], ny + d[1]] == '#' && nx + d[0] > 0 && ny + d[1] > 0 && nx + d[0] < n - 1 && ny + d[1] < n - 1)
                        {
                            maze[nx, ny] = ' ';
                            maze[nx + d[0], ny + d[1]] = ' ';
                            CarvePath(nx + d[0], ny + d[1]);
                        }
                    }
                }
            }

            // Comenzar desde una esquina y crear el camino al centro
            int startX = 1, startY = 1;
            maze[startX, startY] = ' ';
            CarvePath(startX, startY);

            // Asegurar que el centro esté conectado y marcado con 'S'
            maze[centro, centro] = 'S';

            // Conectar todos los pasillos al centro
            ConectarPasillosAlCentro(maze, centro, centro);

        }

        static void ConectarPasillosAlCentro(char[,] maze, int cx, int cy)
        {
            int n = maze.GetLength(0);
            bool[,] visitado = new bool[n, n];

            void DFS(int x, int y)
            {
                if (x < 0 || x >= n || y < 0 || y >= n || maze[x, y] == '#' || visitado[x, y])
                    return;

                visitado[x, y] = true;

                int[][] direcciones = new int[][]
                {
                new int[] {0, 1},
                new int[] {1, 0},
                new int[] {0, -1},
                new int[] {-1, 0}
                };

                foreach (var d in direcciones)
                {
                    DFS(x + d[0], y + d[1]);
                }
            }

            DFS(cx, cy);

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (maze[x, y] == ' ' && !visitado[x, y])
                    {
                        maze[x, y] = '#'; // Convertir los pasillos aislados en paredes
                    }
                }
            }
        }

        public static void Imprimirmaze(char[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

}