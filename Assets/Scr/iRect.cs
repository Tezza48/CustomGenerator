using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class iRect
{
    private int x, y, w, h;



    public int[] Origin
    { get { return new int[] { x, y }; } }

    public int X { get { return x; } }
    public int Y { get { return y; } }
    public int W { get { return w; } }
    public int H { get { return h; } }

    public int xMax { get { return x + w; } }
    public int yMax { get { return y + h; } }

    public iRect ()
    {
        x = 0;
        y = 0;
        w = 0;
        h = 0;
    }

    public iRect(int _x, int _y, int _w, int _h)
    {
        x = _x;
        y = _y;
        w = _w;
        h = _h;
    }
    //Overlaps()
}