using SDL2;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Sdl;

namespace Mazecom
{
    internal class Traps
    {
        public string Name;
        public Sprites sprite;
        public bool activated = false;

        public Traps(string name, Sprites sprite)
        {
            Name = name;
            this.sprite = sprite;
        }

        protected void Message(string effecto)
        {
            bool active = true;
            Font type = new Font("Datos\\Joystix.ttf", 30);
            while (active)
            {
                Sdl_Manager.DeleteHiddenScreen();
                Sdl_Manager.WriteHiddenTxt(
                    "Has activado una trampa",
                    400, 150,
                    255, 0, 0,
                    type);
                Sdl_Manager.WriteHiddenTxt(effecto,
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
    }

    class FrostTrap : Traps
    {
        public FrostTrap(string name,Sprites sprite) : base(name,sprite)
        {

        }

        public void Active(Token token)
        {
            if (!base.activated)
            {
                token.congelada = 3;
                base.activated = true;
                base.Message("Esta Ficha no podra moverse en tres turnos");
            }
        }
    }
    class TeleportTrap : Traps
    {
        public TeleportTrap(string name, Sprites sprite) : base(name, sprite)
        {

        }

        public void Active(Token token, Maze maze)
        {
            if (!base.activated)
            {
                Random generador = new Random();
                base.activated = true;
                base.Message("Eres enviado a otra zona");
                bool tele = true;
                while(tele)
                {
                    int i = generador.Next(0, maze.maze.GetLength(0));
                    int j = generador.Next(0, maze.maze.GetLength(1));
                    if (maze.maze[i, j] == ' ')
                    {
                        token.Move(maze.xMap + j * maze.broadTile, maze.yMap + i * maze.highTile);
                        tele = false;
                    }
                }
            }
        }
    }
    class DamageTramp : Traps
    {
        public DamageTramp(string name, Sprites sprite) : base(name, sprite)
        {
        }
        public void Active(PlayerData player)
        {
            if (!base.activated)
            {
                player.Puntos -= 10;
                base.activated = true;
                base.Message("Pierdes parte de tu cordura. -10ptos");
            }
        }
    }

}