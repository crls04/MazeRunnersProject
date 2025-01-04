using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Sdl;

namespace Mazecom
{
    internal class Sound
    {
        
        IntPtr internalPointer;

       
        public Sound(string fileName)
        {
            internalPointer = SdlMixer.Mix_LoadMUS(fileName);
        }

       
        public void Play()
        {
            SdlMixer.Mix_PlayMusic(internalPointer, 1);
        }

        
        public void PlayBackground()
        {
            SdlMixer.Mix_PlayMusic(internalPointer, -1);
        }

        
        public void Interrupt()
        {
            SdlMixer.Mix_HaltMusic();
        }
    }
}
