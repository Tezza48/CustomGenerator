using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// NSEW
namespace BSGenerator
{
    class Program
    {
        public static int Width = 3, Height = 3;
        static void Main(string[] args)
        {
            int[,] cells = new int[Width * 2 + 1, Height * 2 + 1];
            for (int x = 1; x < Width * 2 + 1; x += 2) 
            {
                cells[x-1, 0]=
            }
        }

        private static void WriteCharArr(char[,] map)
        {
            for (int y = map.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }
        /*
big square
201, 205, 187
186, [space], 186
200, 205, 188
*/

    }
}
