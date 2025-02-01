## Tabla de Contenidos
- Introduccion(Como pudiesen comenzar los nuevos usuarios con el proyecto(****Tutorial inicial****))
- Descubriendo la verdad
- Jugabilidad
- Funcionalidad del Proyecto(fragmentos de codigo en C#)

### Introduccion
 Hola ***{Name}***, si eres nuevo por aca este documento te servira a modo de tutorial y entender la jugabilidad de este proyecto universitario con tematica de laberintos.
 ### Descubriendo la verdad
 - En un mundo donde la realidad y la locura se entrelazan, te encuentras en el borde de un abismo que parece engullir todo sentido de claridad. Este abismo, conocido como “El Abismo de la Locura”, es un laberinto en constante cambio, donde los ecos de las mentes perdidas resuenan y susurros de antiguas almas atormentadas flotan en el aire. Los personajes que se aventuran aquí son conocidos como los Buscadores.
La historia comienza cuando un grupo de Buscadores recibe un antiguo manuscrito que detalla la ubicación del abismo. Se dice que en su interior se encuentran fragmentos de cordura y objetos que pertenecieron a aquellos que sucumbieron a la locura.
A medida que los Buscadores descienden en el laberinto, se dan cuenta de que cada pasaje está impregnado de la locura de quienes han caminado antes que ellos. Un giro a la izquierda podría convertir una puerta en un muro
Mientras buscan fragmentos de cordura, los jugadores también se encuentran con objetos de un pasado sombrío. Cada objeto provoca visiones que pueden llevar a la desesperación. Algunas visiones muestran el dolor de aquellos que fueron arrastrados al abismo, mientras que otras revelan vislumbres del pasado de los propios Buscadores, desenterrando traumas que creían enterrados.
A medida que avanzan, la locura comienza a tomar forma, manifestándose en sombras que parecen cobrar vida. Los jugadores deben enfrentarse a sus propios demonios internos.
 En "El Abismo de la Locura", la línea entre la cordura y la locura es una delgada y aterradora realidad que cada jugador debe navegar.
 ### Jugabilidad
- Al iniciarse el juego podras elegir entre comenzar la partida o salir del juego. Si se comienza la partida automaticamente se podra elegir con cuantas fichas desea jugar cada player(ambos la misma cantidad), pudiendo elegir presionando el numero de la cantidad de fichas que desee. Al iniciar el juego se mostrara el laberinto generado aleatoriamente con trampas y obstaculos esparcidos en el mapa. Tambien dentro del juego se mostraran instrucciones para el movimiento de las fichas y el lanzamiento de la habilidad. Cada ficha cuenta con una habilidad, velocidad y tiempo de enfriamiento de la misma. El juego se basa en recolectar objetos y el primer jugador que obtenga la mayor cantidad de puntos despues de recogidos todos los objetos pues ganara la partida notificandolo con un mensaje en pantalla. Cada ficha consta de un numero y el movimiento de estas se realizara de tal forma que, despues de presionado el numero de la ficha que quiera mover, con las flechas del teclado usted seleccionara la direccion a la cual dirigirse mientras sea valida.
 ## Funcionalidad del proyecto
- Aqui les dejo a algunos desarrolladores interesados, fragmentos de codigo para que entiendan y tengan nocion sobre el backend de este proyecto.
 ### Clase que maneja los datos de los jugadores y las fichas
 ```
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
        public int velocity = 1;
        public int ActiveTime = 0;
        public int coldown = 0;
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

        public void Hability()
        {
            if(coldown == 0)
            {
                velocity = 2;
                ActiveTime = 3;
                coldown = 3;
            }
        }
    }
}
```
### Metodo que crea el menu inicial con el trabajo de SDLS
```
private static void InitialMenu()
        {
            bool endMenu = false;
            Sdl_Manager.DeleteHiddenScreen();

            Sdl_Manager.WriteHiddenTxt("The Abyss of Madness",
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
            Sdl_Manager.WriteHiddenTxt("Pulsa L para descubrir la verdad",
                500, 450, //coordenadas
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
                if (Sdl_Manager.KeyPressed(Sdl_Manager.keyL))
                {
                    InitialLore();
                }

                if (Sdl_Manager.KeyPressed(Sdl_Manager.keyEsc))
                {
                    endMenu = true;
                    sessionEnded = true;
                }
            }
            while (!endMenu);
        }
```
### Fragmento de la clase Sdl Manager donde se crearon las bases del trabajo con SDLS
```
internal class Sdl_Manager
    {
        // Atributos

        static public IntPtr hiddenScreen;
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
```
