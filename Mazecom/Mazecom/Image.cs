using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
    internal class Image
    {
        // Atributos
        private IntPtr internalPointer;

        // Operaciones    
        public Image()  // Constructor
        {
            internalPointer = IntPtr.Zero;  // En principio, no hay imagen
        }

        public Image(string fileName)  // Constructor
        {
            Load(fileName);
        }

        /// Carga una imagen a partir de un nombre de fichero
        public void Load(string fileName)
        {
            internalPointer = Sdl_Manager.LoadImage(fileName);
            if (internalPointer == IntPtr.Zero)
                Sdl_Manager.Error("Imagen inexistente: " + fileName);
        }

        /// Dibuja una imagen en unas coordenadas (se apoya en Sdl Manager)
        public void Draw(int x, int y)
        {
            Sdl_Manager.DrawHiddenImage(internalPointer, x, y);
        }

        /// Devuelve el puntero de SDL (no deberia ser necesario usarla nunca)
        public IntPtr GetPointer()
        {
            return internalPointer;
        }
    }
}
