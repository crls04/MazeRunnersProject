using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazecom
{
    internal class Font
    {
        // Atributos

        IntPtr internalPointer;

        // Operaciones

        /// Constructor a partir de un nombre de fichero y un tamaño
        public Font(string fileName, short size)
        {
            Load(fileName, size);
        }

        public void Load(string fileName, short size)
        {
            internalPointer = Sdl_Manager .LoadFont(fileName, size);
            if (internalPointer == IntPtr.Zero)
                Sdl_Manager.Error("Fuente inexistente: " + fileName);
        }

        public IntPtr ReadPointer()
        {
            return internalPointer;
        }
    }
}
