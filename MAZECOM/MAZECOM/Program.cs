using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
    internal class Program
    {
        static bool gameOver;
        static bool sessionEnded;
        static Sprites character;
        static Sprites[] items;
        static Sprites[,] walls;
        static int x, y;
        static int cantItems;
        static int points;
        static Font type;
        static Font type1;
        static Sound sound;
        static Maze maze;

        static void Main(string[] args)
        {
            InitializeSession();
            maze = new Maze();
            maze.GenerarLaberinto(21);
            
            // Imprimir el laberinto
            //Maze.Imprimirmaze(maze.maze);

            do
            {
                InitialMenu();

                if (!sessionEnded)
                {


                    InitializeGame(maze);

                    while (!gameOver)
                    {
                        
                        DrawPantalla(maze);
                        ComprobarEntradaUsuario();
                        ComprobarEstadoDelJuego();
                        PausaHastaFinDeFotograma();
                    }
                }
            }
            while (!sessionEnded);
        }
        private static void InitializeSession()
        {
            Sdl_Manager.Initialize(1280, 680, 24);
            sessionEnded = false;

            type = new Font("Datos\\Joystix.ttf", 18);
            type1 = new Font("Datos\\Joystix.ttf", 48);
        }
        private static void InitialMenu()
        {
            bool endMenu = false;
            Sdl_Manager.DeleteHiddenScreen();

            Sdl_Manager.WriteHiddenTxt("MAZE RUNER",
                450, 200, //coordenadas
                200, 200, 200, //colores
                type1);
            Sdl_Manager.WriteHiddenTxt("Pulsa SPACE para jugar",
                500, 350, //coordenadas
               180, 180, 180, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("Pulsa ESCAPE para salir",
                500, 400, //coordenadas
               160, 160, 160, //colores
               type);

            Sdl_Manager.DisplayHidden();

            do
            {
                Sdl_Manager.Pause(20);
                if (Sdl_Manager.KeyPressed(Sdl_Manager.keySpa))
                {
                    endMenu = true;

                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.keyEsc))
                {
                    endMenu = true;
                    sessionEnded = true;
                }
            }
            while (!endMenu);
        }
        private static void InitializeGame(Maze maze)
        {
            Random generador = new Random();
            





            character = new Sprites("Datos\\soul.png");
            character.SetBroadHigh(10, 10);
            x = 600;
            y = 300;




            walls = new Sprites[maze.maze.GetLength(0), maze.maze.GetLength(1)];

            cantItems = 10;
            items = new Sprites[cantItems];
            int item = 0;

            for (int i = 0; i < maze.maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.maze.GetLength(1); j++)
                {
                    if (maze.maze[i, j] == '#')
                    {
                        walls[i, j] = new Sprites("Datos\\ladrillo.png");
                        walls[i, j].MoverA(maze.xMap + j * maze.broadTile,
                            maze.yMap + i * maze.highTile);
                        walls[i, j].SetBroadHigh(maze.broadTile, maze.highTile);
                    }

                    if (maze.maze[i,j] == ' ')
                    {
                        if(item < items.Length)
                        {
                            items[item] = new Sprites("Datos\\soul.png");
                            items[item].MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                            items[item].SetBroadHigh(10, 10);
                            item++;
                        }
                    }
                }
            }
            /*
            for (int i = 0; i < cantItems; i++)
            {
                bool validPosition = false;
                do
                {
                    int xWalls = generador.Next(100, 1100);
                    int yWalls = generador.Next(100, 600);
                    validPosition = PossibleToMove(xWalls, yWalls, xWalls + maze.broadTile, yWalls + maze.highTile);
                    if (validPosition)
                    {
                        items[i] = new Sprites("Datos\\llave.png");
                        items[i].MoverA(xWalls, yWalls);
                        items[i].SetBroadHigh(60, 18);
                    }
                }while(!validPosition);
                
               
            }
            */
            
            gameOver = false;
            points = 0;


            sound = new Sound("Datos\\sound.mp3");
        }

        private static void DrawPantalla(Maze maze)
        {
            Sdl_Manager.DeleteHiddenScreen();

            for (int i = 0; i < maze.maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.maze.GetLength(1); j++)
                {
                    if (walls[i, j] !=null)
                    {
                        walls[i, j].Draw();
                    }
                }
            }

                        Sdl_Manager.WriteHiddenTxt(
                "points " + points,
                10, 10,
                255, 0, 0,
                type);


            for (int i = 0; i < cantItems; i++)
            {
                items[i].Draw();
            }

            character.MoverA(x, y);
            character.Draw();

            Sdl_Manager.DisplayHidden();
        }

        private static void ComprobarEntradaUsuario()
        {
            if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyLf))
                && PossibleToMove(x-3,y,x+48-3,y+45))
                x -= 3;
            if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyRg))
                && PossibleToMove(x+3,y,x+48+3,y+45))
                x += 3;
            if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyUp))
                && PossibleToMove(x,y-3,x+48,y+45-3))
                y -= 3;
            if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyDown))
                && PossibleToMove(x,y+3,x+48,y+45+3))
                y += 3;


            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyEsc))
                gameOver = true;
        }


        private static void ComprobarEstadoDelJuego()
        {
            for (int i = 0; i < cantItems; i++)
            {
                if (items[i].Collides(character))
                {
                    points += 10;
                    items[i].ActiveSet(false);
                    sound.Play();
                }
            }

           
        }

        private static void PausaHastaFinDeFotograma()
        {
            Sdl_Manager.Pause(20);
        }

        private static bool PossibleToMove(int xInit, int yInit, int xEnd, int yEnd)
        {
            for (int i = 0; i < maze.maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.maze.GetLength(1); j++)
                {
                    if (walls[i, j] != null)
                    {
                        if (walls[i, j].Collides(
                            xInit, yInit, xEnd, yEnd))
                             return false;
                    }
                }
            }
            return true;
        }
    }
}
