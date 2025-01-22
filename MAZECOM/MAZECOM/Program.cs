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
        static bool finished;
        static Sprites character;
        static Sprites[] items;
        static int x, y;
        static int cantItems;
        static int points;
        static Font type;
        static Sound sound;

        static void Main(string[] args)
        {
            InicializarJuego();

            Maze mz = new Maze(25, 25);//crea el laberinto cuadrado de 25x25
            mz.SpawnPoints();

            
            
            while (!finished)
            {
                DrawPantalla();
                ComprobarEntradaUsuario();
                ComprobarEstadoDelJuego();
                PausaHastaFinDeFotograma();
            }
        }
        private static void InicializarJuego()
        {
            Random generador = new Random();
            Sdl_Manager.Initialize(1280, 720, 24);
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
            
            type = new Font("Datos\\Joystix.ttf", 18);
            finished = false;
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
                finished = true;
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
