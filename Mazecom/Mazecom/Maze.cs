using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
    public class Maze
    {
        //Comenzando con la estrctura del laberinto.
        int[,] maze;
        int rows, columns;
        Random random;
       
        
        public Maze(int rows, int columns)
        {
            //inicializando variables.
            this.rows = rows;
            this.columns = columns;
            maze=new int[rows, columns];
            random = new Random();
            maze[12, 12] = 1; // centro vacio para condicion de victoria.
            
        }
        //metodo que imprime el laberinto en consola.
        public void PrintMaze()
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.Write(maze[i, j] == 1 ? "  " : " #"); //1 es un pasillo y 0 una pared
                }
                Console.WriteLine();
            }
        }
        //metodo para seleccionar un punto de partida aleatorio en cada esquina exclyendo los bordes.
        public int[,] SpawnPoints()
        {
              
            int spawnPoint = random. Next(1,5);
            int newRows, newColumns; // variables para guardar los valores y utilizarlos en otros players.
            
                if (spawnPoint == 1)
                {
                    maze[1, 1] = 1;
                    newRows = 1;
                    newColumns = 1;
                }
                else if (spawnPoint == 2)
                {
                    maze[maze.GetLength(0) - 2, 1] = 1;
                    newRows = maze.GetLength(0) - 2;
                    newColumns = 1;  
                }
                else if (spawnPoint == 3)
                {
                    maze[1, maze.GetLength(1) - 2] = 1;
                    newRows = 1;
                    newColumns = maze.GetLength(1) - 2;
                }
                else
                {
                    maze[maze.GetLength(0) - 2, maze.GetLength(1) - 2] = 1;
                    newRows = maze.GetLength(0) - 2;
                    newColumns = maze.GetLength(1) - 2;
                }
            return maze;
        }

        
    }
    
}
