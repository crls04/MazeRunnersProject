using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
     class PlayerData
    {
        public string Name;
        public Token[] tokens;
        public int Puntos;


        public PlayerData(string name,int n,string[] img)
        {
            Name = name;
            tokens = new Token[n];
            for(int i = 0; i < n; i++)
            {
                tokens[i] = new Token(img[i], 100, 100);
            }
            Puntos = 0;
        }
    }


    class Token
    {
        public  Sprites sprite;
        public int PosX, PosY;
        public int congelada = 0;

        public Token(string img,int x, int y)
        {
            sprite  = new Sprites(img);
            sprite.SetBroadHigh(5, 5);
            PosX = x;
            PosY = y;
        }
        public void Move(int x, int y)
        {
            PosX = x;
            PosY = y;
            sprite.MoverA(PosX, PosY);
            sprite.Draw();
        }
    }
}
