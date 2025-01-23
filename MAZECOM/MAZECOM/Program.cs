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
        static int x, y;
        static int cantItems;
        static int points;
        static Font type;
        static Font type1;
        static Sound sound;

        static void Main(string[] args)
        {
            InitializeSession();

            do
            {
                InitialMenu();

                if (!sessionEnded)
                {


                    InitializeGame();

                    while (!gameOver)
                    {
                        DrawPantalla();
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
            Sdl_Manager.Initialize(1280, 720, 24);
            sessionEnded = false;

            type = new Font("Datos\\Joystix.ttf", 18);
            type1 = new Font("Datos\\Joystix.ttf", 48);
        }
        private static void InitialMenu()
        {
            bool endMenu = false;
            Sdl_Manager.DeleteHiddenScreen();

            Sdl_Manager.WriteHiddenTxt("MAZE RUNER",
                400, 150, //coordenadas
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
        private static void InitializeGame()
        {
            Random generador = new Random();
            
            character = new Sprites("Datos\\llave.png");
            character.SetBroadHigh(48, 45);
            x = 600;
            y = 300;
            
            cantItems = 20;
            items = new Sprites[cantItems];
            for (int i = 0; i < cantItems; i++)
            {
                items[i] = new Sprites("Datos\\llave.png");
                items[i].MoverA(
                    generador.Next(100, 1100),
                    generador.Next(100, 600));
                items[i].SetBroadHigh(60, 18);
            }
            
            
            gameOver = false;
            points = 0;

            sound = new Sound("Datos\\sound.mp3");
        }

        private static void DrawPantalla()
        {
            Sdl_Manager.DeleteHiddenScreen();

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
            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyLf))
                x -= 3;
            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyRg))
                x += 3;
            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyUp))
                y -= 3;
            if (Sdl_Manager.KeyPressed(Sdl_Manager.keyDown))
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
    }
}
