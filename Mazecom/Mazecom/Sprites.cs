using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mazecom
{
     class Sprites
    {
        // Atributos
        protected int x;
        protected int y;
        protected int velocX;
        protected int velocY;
        protected int high;
        protected int broad;
        protected bool asset;

        protected int xOriginal;  // Para llevar a su posicion inicial
        protected int yOriginal;

        // La imagen que se mostrarß en pantalla, si es estatica
        protected Image myImage;

        // La secuencia de imagenes, si es animada
        protected Image[][] sequence;
        protected byte currentFrame;
        protected byte direction;
        public const byte down = 0;
        public const byte up = 1;
        public const byte rg = 2;
        public const byte lf = 3;

        bool countImage;        // Si no contiene imagen, no se podra dibujar
        bool countSequence;     // La alternativa: imagenes multiples

        // ----- Operaciones -----

        /// Constructor: Carga la imagen que representara a este elemento grafico
        public Sprites(string name)
        {
            LoadImage(name);
            direction = down;
            asset = true;
            currentFrame = 0;
            countImage = true;
            countSequence = false;
            sequence = new Image[12][];
            asset = true;
            // Valores por defecto para broad y high
            broad = 32;
            high = 32;
        }

        /// Mueve el elemento grafico a otra posicion
        public void MoverA(int newX, int newY)
        {
            x = newX;
            y = newY;
            if ((xOriginal == 0) && (yOriginal == 0))
            {
                xOriginal = newX;
                yOriginal = newY;
            }
        }

        /// Cambia la velocidad (incrX e incrY) de un elemento
        public void SpeedSet(int vX, int vY)
        {
            velocX = vX;
            velocY = vY;
        }


        /// Carga la imagen que representara a este elemento grafico
        public void LoadImage(string name)
        {
            myImage = new Image();
            myImage.Load(name);
            countImage = true;
            countSequence = false;
        }


        /// Carga una secuencia de imagenes para un elemento animado
        public void LoadSequence(byte direction, string[] names)
        {
            countImage = true;
            countSequence = true;
            byte tamanyo = (byte)names.Length;
            sequence[direction] = new Image[tamanyo];
            for (byte i = 0; i < names.Length; i++)
            {
                sequence[direction][i] = new Image(names[i]);
            }
            // Valores por defecto para ancho y largo
            broad = 32;
            high = 32;
        }

        /// Mueve el elemento grafico a otra posicion
        public void ChangeDirection(byte newDir)
        {
            if (direction != newDir)
            {
                direction = newDir;
                currentFrame = 0;
            }
        }

        /// Devuelve el personaje a su posicion inicial
        public void Reboot()
        {
            x = xOriginal;
            y = yOriginal;
        }


        /// Dibuja el elemento grafico en su posicion actual
        public void Draw()
        {
            if (asset == false) return;
            if (countSequence)
                sequence[direction][currentFrame].Draw(x, y);
            else if (countImage)
                myImage.Draw(x, y);
            else
                Sdl_Manager.Error("Se ha intentado dibujar una imagen no cargada!");
        }

        /// Dibuja el elemento grafico en una posicion cualquiera
        public void Draw(int nuevaX, int nuevaY)
        {
            MoverA(nuevaX, nuevaY);
            Draw();
        }

        /// Comprueba si ha chocado con otro elemento grßfico
        public bool Collides(Sprites otherElem)
        {
            // No se debe chocar con un elemento oculto      
            if ((asset == false) || (otherElem.asset == false))
                return false;
            // Ahora ya compruebo coordenadas
            if ((otherElem.x + otherElem.broad > x)
                && (otherElem.y + otherElem.high > y)
                && (x + broad > otherElem.x)
                && (y + high > otherElem.y))
                return true;
            else
                return false;
        }
        public bool Collides(int xInit, int yInit, int xEnd, int yEnd)
        {
            // No se debe chocar con un elemento oculto      
            if (asset == false)
                return false;
            // Ahora ya compruebo coordenadas
            if ((x<xEnd) && (x+broad>xInit) && (y<yEnd) && (y + high > yInit))
                return true;
            else
                return false;
        }

        /// Prepara el siguiente fotograma, para animar el movimiento de
        /// un personaje
        public void NextFrame()
        {
            if (currentFrame < sequence[direction].Length - 1)
                currentFrame++;
            else
                currentFrame = 0;
        }


        /// Devuelve el valor de x
        public int GetX()
        {
            return x;
        }

        /// Devuelve el valor de y
        public int GetY()
        {
            return y;
        }

        /// Cambia el ancho y el alto
        public void SetBroadHigh(int an, int al)
        {
            high = al;
            broad = an;
        }

        /// Devuelve si esta activo
        public bool GetActive()
        {
            return asset;
        }

        /// Cambia si esta activo (visible) o no (muerto / no visible)
        public void ActiveSet(bool a)
        {
            asset = a;
        }
    }
}
