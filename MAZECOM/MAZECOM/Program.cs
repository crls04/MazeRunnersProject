using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mazecom
{
    internal class Program
    {
        static bool gameOver;
        static bool sessionEnded;
        static Sprites[] items;
        static Sprites[,] walls;
        static int cantItems = 1;
        static int ConsumedItems = 0;
        static Font type;
        static Font type1;
        static Sound sound;
        static Maze maze;
        static PlayerData Player1,Player2;
        static string[] Tokens = new string[2];
        static int Count;
        static Token TokenSelected;
        static int Turno = 1;
        static void Main(string[] args)
        {
            Tokens[0] = "Datos\\RedHollow.png";
            Tokens[1] = "Datos\\BlueHollow.png";

            

            do
            {
                if (!gameOver)
                {
                    InitializeSession();
                maze = new Maze();
                maze.GenerarLaberinto(21);
                InitialMenu();
                InitialSelectedPlay();
                Player1 = new PlayerData("Carlos", Count, Tokens[0]);
                Player2 = new PlayerData("Javier", Count, Tokens[1]);
                InitialCharacteAndItems();
                    if (!sessionEnded)
                    {



                        InitializeGame(maze);

                        while (!gameOver)
                        {

                            DrawPantalla(maze);
                            ComprobarEntradaUsuario();
                            ComprobarEstadoDelJuego();
                            PausaHastaFinDeFotograma();

                            if (ConsumedItems == cantItems)
                            {
                                gameOver = true;
                            }
                        }

                    }
                }
                else
                {

                    Sdl_Manager.DeleteHiddenScreen();
                    if (Player1.Puntos > Player2.Puntos)
                    {
                        Sdl_Manager.WriteHiddenTxt("WIN RED",
                    450, 200, //coordenadas
                    200, 200, 200, //colores
                    type1);
                    }
                    else
                    {
                        Sdl_Manager.WriteHiddenTxt("WIN BLUE",
                    450, 200, //coordenadas
                    200, 200, 200, //colores
                    type1);
                    }
                    Sdl_Manager.DisplayHidden();
                    if (Sdl_Manager.KeyPressed(Sdl_Manager.keyEsc))
                    {
                        sessionEnded = true;
                    }
                    if (Sdl_Manager.KeyPressed(Sdl_Manager.keySpa))
                    {
                        gameOver = false ;
                        ConsumedItems = 0;
                    }
                }
            }
            while (!sessionEnded);
        }

        private static void InitialCharacteAndItems()
        {
            int colocados = 0;
            Random generador = new Random();
            while (colocados < Count)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    Player1.tokens[colocados].Move(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    colocados++;
                }
            }
            colocados = 0;
            while (colocados < Count)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    Player2.tokens[colocados].Move(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    colocados++;
                }
            }

            items = new Sprites[cantItems];
            int item = 0;
            while (item < cantItems)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    if (item < items.Length)
                    {
                        items[item] = new Sprites("Datos\\soul.png");
                        items[item].MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                        items[item].SetBroadHigh(10, 10);
                        item++;
                    }
                }
            }

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

        private static void InitialSelectedPlay()
        {
            bool endMenu = false;
            Sdl_Manager.DeleteHiddenScreen();

            Sdl_Manager.WriteHiddenTxt("MAZE RUNER",
                450, 200, //coordenadas
                200, 200, 200, //colores
                type1);
            Sdl_Manager.WriteHiddenTxt("Selecione la cantidad de fichas que desea",
                350, 350, //coordenadas
               180, 180, 180, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("Pulsa la tecla que indique la opcion que desea",
                310, 400, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("1- 1 Fichas",
                310, 450, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("2- 2 Fichas",
                310, 500, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("3- 3 Fichas",
                310, 550, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("4- 4 Fichas",
                700, 450, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("5- 5 Fichas",
                700, 500, //coordenadas
               160, 160, 160, //colores
               type);
            Sdl_Manager.WriteHiddenTxt("6- 6 Fichas",
                700, 550, //coordenadas
               160, 160, 160, //colores
               type);

            Sdl_Manager.DisplayHidden();

            do
            {
                Sdl_Manager.Pause(20);
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key1))
                {
                    endMenu = true;
                    Count = 1;
                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key2))
                {
                    endMenu = true;
                    Count = 2;
                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key3))
                {
                    endMenu = true;
                    Count = 3;
                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key4))
                {
                    endMenu = true;
                    Count = 4;
                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key5))
                {
                    endMenu = true;
                    Count = 5;
                }
                if (Sdl_Manager.KeyPressed(Sdl_Manager.key6))
                {
                    endMenu = true;
                    Count = 6;
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


            walls = new Sprites[maze.maze.GetLength(0), maze.maze.GetLength(1)];

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
                  
                }
            }
            gameOver = false;


            sound = new Sound("Datos\\sound.mp3");
        }

        private static void DrawPantalla(Maze maze)
        {
            Sdl_Manager.DeleteHiddenScreen();

            for (int i = 0; i < maze.maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.maze.GetLength(1); j++)
                {
                    if (walls[i, j] != null)
                    {
                        walls[i, j].Draw();
                    }
                }
            }

            Sdl_Manager.WriteHiddenTxt(
    "Humanity P1: " + Player1.Puntos,
    40, 10,
    255, 0, 0,
    type);
            Sdl_Manager.WriteHiddenTxt(
    "Humanity P2: " + Player2.Puntos,
    1000, 10,
    0, 31, 63,
    type);


            for (int i = 0; i < cantItems; i++)
            {
                items[i].Draw();
            }
            for(int i = 0; i < Count; i++)
            {
                Player1.tokens[i].Move(Player1.tokens[i].PosX, Player1.tokens[i].PosY);
                Player2.tokens[i].Move(Player2.tokens[i].PosX, Player2.tokens[i].PosY);
            }
            Sdl_Manager.DisplayHidden();
        }

        private static  void ComprobarEntradaUsuario()
        {
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key1))
            {
                if(Turno == 1)
                {
                    TokenSelected = Player1.tokens[0];
                }
                else
                {
                    TokenSelected = Player2.tokens[0];
                }
            }
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key2) && Count >= 2)
            {
                if (Turno == 1)
                {
                    TokenSelected = Player1.tokens[1];
                }
                else
                {
                    TokenSelected = Player2.tokens[1];
                }
            }
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key3) && Count >= 3)
            {
                if (Turno == 1)
                {
                    TokenSelected = Player1.tokens[2];
                }
                else
                {
                    TokenSelected = Player2.tokens[2];
                }
            }
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key4) && Count >= 4)
            {
                if (Turno == 1)
                {
                    TokenSelected = Player1.tokens[3];
                }
                else
                {
                    TokenSelected = Player2.tokens[3];
                }
            }
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key5) && Count >= 5)
            {
                if (Turno == 1)
                {
                    TokenSelected = Player1.tokens[4];
                }
                else
                {
                    TokenSelected = Player2.tokens[4];
                }
            }
            if (Sdl_Manager.KeyPressed(Sdl_Manager.key6) && Count >= 6)
            {
                if (Turno == 1)
                {
                    TokenSelected = Player1.tokens[5];
                }
                else
                {
                    TokenSelected = Player2.tokens[5];
                }
            }

            if (TokenSelected != null)
            {
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyLf))
                    && PossibleToMove(TokenSelected.PosX - 30, TokenSelected.PosY, TokenSelected.PosX, TokenSelected.PosY + 30))
                {
                    TokenSelected.PosX -= 30;
                    TokenSelected = null;
                    Turno = Turno == 1 ? 2 : 1;
                }
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyRg))
                    && PossibleToMove(TokenSelected.PosX + 30, TokenSelected.PosY, TokenSelected.PosX + 60, TokenSelected.PosY + 30))
                {
                    TokenSelected.PosX += 30;
                    TokenSelected = null;
                    Turno = Turno == 1 ? 2 : 1;
                }
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyUp))
                    && PossibleToMove(TokenSelected.PosX, TokenSelected.PosY - 30, TokenSelected.PosX + 30, TokenSelected.PosY))
                {
                    TokenSelected.PosY -= 30;
                    TokenSelected = null;
                    Turno = Turno == 1 ? 2 : 1;
                }
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyDown))
                    && PossibleToMove(TokenSelected.PosX, TokenSelected.PosY + 30, TokenSelected.PosX + 30, TokenSelected.PosY + 60))
                {
                    TokenSelected.PosY += 30;
                    TokenSelected = null;
                    Turno = Turno == 1 ? 2 : 1;
                }
            }
              
            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyEsc))
                gameOver = true;

            
        }

        private static void ComprobarEstadoDelJuego()
        {
            for (int i = 0; i < Player1.tokens.Length; i++)
            {
                if (Colisiona(items, Player1.tokens[i].sprite))
                {
                    Player1.Puntos += 10;
                    ConsumedItems += 1;
                    sound.Play();
                }
                if (Colisiona(items, Player2.tokens[i].sprite))
                {
                    Player2.Puntos += 10;
                    ConsumedItems += 1;
                    sound.Play();
                }
                
            }


        }
        public static bool Colisiona(Sprites[] sp, Sprites token)
        {

            for (int i = 0; i < sp.Length; i++)
            {
                if (token.Collides(sp[i]))
                {
                    sp[i].ActiveSet(false);
                    return true;
                }
            }

            return false;
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
