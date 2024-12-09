using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Maze mz = new Maze(25, 25);//crea el laberinto cuadrado de 15x15
            mz.SpawnPoints();
            mz.PrintMaze();
            Console.ReadKey();
        }
    }
}
