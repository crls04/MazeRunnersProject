using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDL2;

namespace Mazecom
{
    internal class Program
    {
        static bool gameOver;
        static bool sessionEnded;
        static Sprites[] items;
        static Sprites[,] walls;
        static int cantItems = 11;
        static int ConsumedItems = 0;
        static Font type;
        static Font type1;
        static Sound sound;
        static Maze maze;
        static PlayerData Player1,Player2;
        static string[] TokensR = new string[6];
        static string[] TokensB = new string[6];
        static int Count;
        static Token TokenSelected;
        static int Turno = 1;
        static FrostTrap[] TrapFrost;
        static DamageTramp[] TrapDamage;
        static TeleportTrap[] TeleTrap;
        static int TurnoAnterior = 1;
        static Sprites Obstacle;
        static int count = 0;

        static void Main(string[] args)
        {
            TokensR[0] = "Datos\\1r.png";
            TokensR[1] = "Datos\\2r.png";
            TokensR[2] = "Datos\\3r.png";
            TokensR[3] = "Datos\\4r.png";
            TokensR[4] = "Datos\\5r.png";
            TokensR[5] = "Datos\\6r.png";

            TokensB[0] = "Datos\\1a.png";
            TokensB[1] = "Datos\\2a.png";
            TokensB[2] = "Datos\\3a.png";
            TokensB[3] = "Datos\\4a.png";
            TokensB[4] = "Datos\\5a.png";
            TokensB[5] = "Datos\\6a.png";

            Obstacle = new Sprites("Datos\\eye.png");

            do
            {
                if (!gameOver)
                {
                    InitializeSession();
                    Random generador = new Random();
                    maze = new Maze();
                    maze.GenerarLaberinto(21);
                    InitialMenu();
                    InitialSelectedPlay();
                    Player1 = new PlayerData("Carlos", Count, TokensR);
                    Player2 = new PlayerData("Javier", Count, TokensB);
                    bool obstaclemove = true;

                    while (obstaclemove)
                    {
                        int i = generador.Next(0, maze.maze.GetLength(0));
                        int j = generador.Next(0, maze.maze.GetLength(1));
                        if (maze.maze[i, j] == ' ')
                        {
                            Obstacle.MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                            Obstacle.SetBroadHigh(maze.broadTile, maze.highTile);
                            obstaclemove = false;
                        }
                    }
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
                            if (Turno != TurnoAnterior) RestColdown();
                            IsMove();

                            if (ConsumedItems == cantItems)
                            {
                                gameOver = true;
                            }
                        }
                    }
                }
                else
                {
                    FinalGame();
                }
            }
            while (!sessionEnded);
        }

        private static void IsMove()
        {
            bool PosibleJugar = false;
            if (Turno == 1)
            {
                TurnoAnterior = 1;
                for (int i = 0; i < Player1.tokens.Length; i++)
                {
                    if (Player1.tokens[i].congelada == 0)
                    {
                        PosibleJugar = true;
                    }
                }
            }
            else
            {
                TurnoAnterior = 2;
                for (int i = 0; i < Player2.tokens.Length; i++)
                {
                    if (Player2.tokens[i].congelada == 0)
                    {
                        PosibleJugar = true;
                    }
                }
            }
            if (!PosibleJugar)
            {
                if (Turno == 1)
                {
                    Turno = 2;
                    Message("Jugador 1 no puede mover ninguna ficha");
                }
                else
                {
                    Turno = 1;
                    Message("Jugador 2 no puede mover ninguna ficha");
                }
            }
        }

        private static void Message(string Message)
        {
            bool active = true;
            Font type = new Font("Datos\\Joystix.ttf", 30);
            while (active)
            {
                Sdl_Manager.DeleteHiddenScreen();
                Sdl_Manager.WriteHiddenTxt(
                    "Advertencia",
                    500, 150,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(Message,
                    170, 250,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "Presiona la tecla Space para continuar",
                    200, 500,
                    255, 0, 0,
                    type);
                Sdl_Manager.DisplayHidden();
                if (Sdl_Manager.KeyPressed(Sdl_Manager.keySpa)) active = false;
            }
        }

        private static void RestColdown()
        {
            Random generador = new Random();
            count++;
            while (count == 5)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    Obstacle.MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    count = 0;
                }
            }
            if (Turno == 1)
            {
                TurnoAnterior = 2;
                for (int i = 0; i < Player1.tokens.Length; i++)
                {
                    if (Player1.tokens[i].congelada > 0) Player1.tokens[i].congelada--;
                }
            }
            else
            {
                TurnoAnterior = 1;
                for (int i = 0; i < Player2.tokens.Length; i++)
                {
                    if (Player2.tokens[i].congelada > 0) Player2.tokens[i].congelada--;
                }
            }
        }

        private static void FinalGame()
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
                gameOver = false;
                ConsumedItems = 0;
            }
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
                    items[item] = new Sprites("Datos\\soul.png");
                    items[item].MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    items[item].SetBroadHigh(10, 10);
                    item++;
                    for (int s = 0; s < cantItems;s++)
                    {
                        if(s != item-1 && items[s] != null)
                        {
                            if (items[item-1].Collides(items[s]))
                            {
                                item--;
                                break;
                            }
                        }
                    }

                }
            }
            int frost = 0;
            TrapFrost = new FrostTrap[5];
            while (frost < TrapFrost.Length)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    TrapFrost[frost] = new FrostTrap("Frost", new Sprites("Datos\\cadenas.png"));
                    TrapFrost[frost].sprite.MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    TrapFrost[frost].sprite.SetBroadHigh(10, 10);
                    frost++;
                    for (int s = 0; s < cantItems; s++)
                    {
                            if (items[s] != null && items[s].Collides(TrapFrost[frost-1].sprite))
                            {
                                frost--;
                                break;
                            }    
                    }
                    for (int s = 0; s < TrapFrost.Length; s++)
                    {
                        if (TrapFrost[s] != null && s != frost-1)
                        {
                            if (TrapFrost[frost-1].sprite.Collides(TrapFrost[s].sprite))
                            {
                                frost--;
                                break;
                            }
                        }
                    }
                }
            }

            int damage = 0;
            TrapDamage = new DamageTramp[5];
            while (damage < TrapDamage.Length)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    TrapDamage[damage] = new DamageTramp("Damage", new Sprites("Datos\\damage trap.png"));
                    TrapDamage[damage].sprite.MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    TrapDamage[damage].sprite.SetBroadHigh(10, 10);
                    damage++;
                    for (int s = 0; s < cantItems; s++)
                    {
                        if (items[s] != null && items[s].Collides(TrapDamage[damage-1].sprite))
                        {
                            damage--;
                            break;
                        }
                    }
                    for (int s = 0; s < TrapFrost.Length; s++)
                    {
                        if (TrapFrost[s] != null)
                        {
                            if (TrapDamage[damage-1].sprite.Collides(TrapFrost[s].sprite))
                            {
                                damage--;
                                break;
                            }
                        }
                    }
                    for (int s = 0; s < TrapDamage.Length; s++)
                    {
                        if (TrapDamage[s] != null && s != damage-1)
                        {
                            if (TrapDamage[damage - 1].sprite.Collides(TrapDamage[s].sprite))
                            {
                                damage--;
                                break;
                            }
                        }
                    }
                }
            }
            int tele = 0;
            TeleTrap = new TeleportTrap[5];
            while (tele < TeleTrap.Length)
            {
                int i = generador.Next(0, maze.maze.GetLength(0));
                int j = generador.Next(0, maze.maze.GetLength(1));
                if (maze.maze[i, j] == ' ')
                {
                    TeleTrap[tele] = new TeleportTrap("Teleport", new Sprites("Datos\\hole.png"));
                    TeleTrap[tele].sprite.MoverA(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                    TeleTrap[tele].sprite.SetBroadHigh(10, 10);
                    tele++;
                    for (int s = 0; s < cantItems; s++)
                    {
                        if (items[s] != null && items[s].Collides(TeleTrap[tele-1].sprite))
                        {
                            tele--;
                            break;
                        }
                    }
                    for (int s = 0; s < TrapFrost.Length; s++)
                    {
                        if (TrapFrost[s] != null)
                        {
                            if (TeleTrap[tele-1].sprite.Collides(TrapFrost[s].sprite))
                            {
                                tele--;
                                break;
                            }
                        }
                    }
                    for (int s = 0; s < TrapDamage.Length; s++)
                    {
                        if (TrapDamage[s] != null)
                        {
                            if (TeleTrap[tele-1].sprite.Collides(TrapDamage[s].sprite))
                            {
                                tele--;
                                break;
                            }
                        }
                    }
                    for (int s = 0; s < TeleTrap.Length; s++)
                    {
                        if (TeleTrap[s] != null && tele-1 != s)
                        {
                            if (TeleTrap[tele-1].sprite.Collides(TeleTrap[s].sprite))
                            {
                                tele--;
                                break;
                            }
                        }
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

            InitPoints();
            InitInstruction();
            Obstacle.Draw();

            for (int i = 0; i < cantItems; i++)
            {
                items[i].Draw();
            }
            for (int i = 0; i < TrapFrost.Length; i++)
            {
               if(!TrapFrost[i].activated) TrapFrost[i].sprite.Draw();
            }
            for (int i = 0; i < TrapDamage.Length; i++)
            {
                if (!TrapDamage[i].activated) TrapDamage[i].sprite.Draw();
            }
            for (int i = 0; i < TeleTrap.Length; i++)
            {
                if (!TeleTrap[i].activated) TeleTrap[i].sprite.Draw();
            }
            for (int i = 0; i < Count; i++)
            {
                Player1.tokens[i].Move(Player1.tokens[i].PosX, Player1.tokens[i].PosY);
                Player2.tokens[i].Move(Player2.tokens[i].PosX, Player2.tokens[i].PosY);
            }
            Sdl_Manager.DisplayHidden();
        }

        private static void InitPoints()
        {
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
        }

        private static void InitInstruction()
        {
            if (Turno == 1)
            {
                Sdl_Manager.WriteHiddenTxt(
                    "Es tu turno ",
                    70, 150,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "Presiona la tecla ",
                    45, 200,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "que indique la ficha ",
                    20, 250,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "a utilizar. Luego",
                    40, 300,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "utilice las flechas ",
                    25, 350,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "de direccion para ",
                    35, 400,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "mover la ficha ",
                    50, 450,
                    255, 0, 0,
                    type);


            }
            else
            {
                Sdl_Manager.WriteHiddenTxt(
                    "Es tu turno ",
                    1010, 150,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "Presiona la tecla ",
                    970, 200,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "que indique la ficha ",
                    940, 250,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "a utilizar. Luego",
                    960, 300,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "utilice las flechas ",
                    945, 350,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "de direccion para ",
                    955, 400,
                    0, 31, 63,
                    type);
                Sdl_Manager.WriteHiddenTxt(
                    "mover la ficha ",
                    970, 450,
                    0, 31, 63,
                    type);
            }
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

            if (TokenSelected != null && TokenSelected.congelada == 0)
            {
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyLf))
                    && PossibleToMove(TokenSelected.PosX - 30, TokenSelected.PosY, TokenSelected.PosX, TokenSelected.PosY + 30))
                {
                    TokenSelected.PosX -= 30;
                    TokenSelected = null;
                    Turno = Turno == 1 ? 2 : 1;
                }
                if ((Sdl_Manager.KeyPressed(Sdl_Manager.keyRg))
                    && PossibleToMove(TokenSelected.PosX + 30, TokenSelected.PosY, TokenSelected.PosX + 60, TokenSelected.PosY + 30)
                   )
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
                for(int s = 0; s < TrapFrost.Length;s++)
                {
                    if (TrapFrost[s].sprite.Collides(Player1.tokens[i].sprite)) TrapFrost[s].Active(Player1.tokens[i]);
                    if (TrapFrost[s].sprite.Collides(Player2.tokens[i].sprite)) TrapFrost[s].Active(Player2.tokens[i]);
                }
                for(int s = 0; s < TrapDamage.Length;s++)
                {
                    if (TrapDamage[s].sprite.Collides(Player1.tokens[i].sprite)) TrapDamage[s].Active(Player1);
                    if (TrapDamage[s].sprite.Collides(Player2.tokens[i].sprite)) TrapDamage[s].Active(Player2);
                }
                for (int s = 0; s < TeleTrap.Length; s++)
                {
                    if (TeleTrap[s].sprite.Collides(Player1.tokens[i].sprite))TeleTrap[s].Active(Player1.tokens[i],maze);
                    if (TeleTrap[s].sprite.Collides(Player2.tokens[i].sprite))TeleTrap[s].Active(Player2.tokens[i],maze);
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
                            xInit, yInit, xEnd, yEnd) || Obstacle.Collides(
                            xInit, yInit, xEnd, yEnd))
                            return false;
                    }
                }
            }
            return true;
        }
    }


}
