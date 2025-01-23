using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Tao.Sdl;

namespace Mazecom
{
    internal class Sdl_Manager
    {
        // Atributos

        static IntPtr hiddenScreen;
        static int broad, high;

        // Operaciones

        /// Inicializa el modo grafico a un cierto ancho, alto y profundidad de color, p.ej. 640, 480, 24 bits
        public static void Initialize(int br, int hig, int colors)
        {
            
            broad = br;
            high = hig;

            int flags = (Sdl.SDL_HWSURFACE | Sdl.SDL_DOUBLEBUF | Sdl.SDL_ANYFORMAT);
            Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
            hiddenScreen = Sdl.SDL_SetVideoMode(
                broad,
                high,
                colors,
                flags);

            Sdl.SDL_Rect rect2 = new Sdl.SDL_Rect(0, 0, (short)broad, (short)high);
            Sdl.SDL_SetClipRect(hiddenScreen, ref rect2);
            SdlTtf.TTF_Init();

            if (SdlMixer.Mix_OpenAudio(22050, unchecked(Sdl.AUDIO_S16LSB), 2, 1024) == -1)
                Error("No se ha podido inicializar el sonido");
        }

        /// Dibuja una image en pantalla oculta, en ciertas coordenadas
        public static void DeleteHiddenScreen()
        {
            Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, (short)broad, (short)high);
            Sdl.SDL_FillRect(hiddenScreen, ref origin, 0);
        }

        /// Dibuja una image en pantalla oculta, en ciertas coordenadas
        public static void DrawHiddenImage(IntPtr image, int x, int y)
        {
            Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, (short)broad, (short)high);
            Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, (short)broad, (short)high);
            Sdl.SDL_BlitSurface(image, ref origin, hiddenScreen, ref dest);
        }

        /// Dibuja una image en pantalla oculta, en ciertas coordenadas
        public static void DrawHiddenImage(Image image, int x, int y)
        {
            DrawHiddenImage(image.GetPointer(), x, y);
        }

        /// Visualiza la pantalla oculta
        public static void DisplayHidden()
        {
            Sdl.SDL_Flip(hiddenScreen);
        }


        public static IntPtr LoadImage(string file)
        {
            IntPtr image = IntPtr.Zero;
            image = SdlImage.IMG_Load(file);
            if (image == IntPtr.Zero)
            {
                System.Console.WriteLine("image inexistente: {0}", file);
                Environment.Exit(4);
            }
            return image;
        }

        public static void WriteHiddenTxt(string text,
            int x, int y, byte r, byte g, byte b, Font f)
        {
            WriteHiddenTxt(text, x, y, r, g, b, f.ReadPointer());
        }

        public static void WriteHiddenTxt(string text,
            int x, int y, byte r, byte g, byte b, IntPtr font)
        {
            Sdl.SDL_Color color = new Sdl.SDL_Color(r, g, b);
            IntPtr textAsImage = SdlTtf.TTF_RenderText_Solid(
                font, text, color);
            if (textAsImage == IntPtr.Zero)
                Environment.Exit(5);

            Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, (short)broad, (short)high);
            Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y, (short)broad, (short)high);

            Sdl.SDL_BlitSurface(textAsImage, ref origin,
                hiddenScreen, ref dest);
            Sdl.SDL_FreeSurface(textAsImage);
        }

        public static IntPtr LoadFont(string file, int size)
        {
            IntPtr font = SdlTtf.TTF_OpenFont(file, size);
            if (font == IntPtr.Zero)
            {
                System.Console.WriteLine("font inexistente: {0}", file);
                Environment.Exit(6);
            }
            return font;
        }

        public static bool KeyPressed(int c)
        {
            bool pulsada = false;

            Sdl.SDL_PumpEvents();
            Sdl.SDL_Event suceso;
            Sdl.SDL_PollEvent(out suceso);

            int numkeys;
            byte[] teclas = Tao.Sdl.Sdl.SDL_GetKeyState(out numkeys);

            if (teclas[c] == 1)
                pulsada = true;
            return pulsada;
        }

        public static void Pause(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        /// Devuelve la anchura de la pantalla, en pixeles
        public static int GetBroad()
        {
            return broad;
        }

        /// Devuelve la altura de la pantalla, en pixeles
        public static int GetHigh()
        {
            return high;
        }

        /// Abandona el programa, mostrando un cierto mensaje de error
        public static void Error(string text)
        {
            System.Console.WriteLine(text);
            Environment.Exit(1);
        }


        // Definiciones de teclas
        public static int key20 = Sdl.SDLK_KP_ENTER;
        public static int keyEsc = Sdl.SDLK_ESCAPE;
        public static int keySpa = Sdl.SDLK_SPACE;
        public static int keyA = Sdl.SDLK_a;
        public static int keyB = Sdl.SDLK_b;
        public static int keyC = Sdl.SDLK_c;
        public static int keyD = Sdl.SDLK_d;
        public static int keyE = Sdl.SDLK_e;
        public static int keyF = Sdl.SDLK_f;
        public static int keyG = Sdl.SDLK_g;
        public static int keyH = Sdl.SDLK_h;
        public static int keyI = Sdl.SDLK_i;
        public static int keyJ = Sdl.SDLK_j;
        public static int keyK = Sdl.SDLK_k;
        public static int keyL = Sdl.SDLK_l;
        public static int keyM = Sdl.SDLK_m;
        public static int keyN = Sdl.SDLK_n;
        public static int keyO = Sdl.SDLK_o;
        public static int keyP = Sdl.SDLK_p;
        public static int keyQ = Sdl.SDLK_q;
        public static int keyR = Sdl.SDLK_r;
        public static int keyS = Sdl.SDLK_s;
        public static int keyT = Sdl.SDLK_t;
        public static int keyU = Sdl.SDLK_u;
        public static int keyV = Sdl.SDLK_v;
        public static int keyW = Sdl.SDLK_w;
        public static int keyX = Sdl.SDLK_x;
        public static int keyY = Sdl.SDLK_y;
        public static int keyZ = Sdl.SDLK_z;
        public static int key1 = Sdl.SDLK_1;
        public static int key2 = Sdl.SDLK_2;
        public static int key3 = Sdl.SDLK_3;
        public static int key4 = Sdl.SDLK_4;
        public static int key5 = Sdl.SDLK_5;
        public static int key6 = Sdl.SDLK_6;
        public static int key7 = Sdl.SDLK_7;
        public static int key8 = Sdl.SDLK_8;
        public static int key9 = Sdl.SDLK_9;
        public static int key0 = Sdl.SDLK_0;
        public static int keyUp = Sdl.SDLK_UP;
        public static int keyDown = Sdl.SDLK_DOWN;
        public static int keyRg = Sdl.SDLK_RIGHT;
        public static int keyLf = Sdl.SDLK_LEFT;
    }
}
