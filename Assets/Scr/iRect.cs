using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class iRect
{
    private int x, y, w, h;



    public int[] Origin
    { get { return new int[] { x, y }; } }

    public Vector2 vOrigin { get { return new Vector2(x, y); } }
    public Vector2 vSize { get { return new Vector2(w, h); } }

    public int X { get { return x; } }
    public int Y { get { return y; } }
    public int W { get { return w; } }
    public int H { get { return h; } }

    public int xMax { get { return x + w+1; } }
    public int yMax { get { return y + h+1; } }

    public int[] Centre { get { return new int[] { w / 2, h / 2 }; } }

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

    public static bool Intersects(iRect _a, iRect _b)
    {
        Rect aRect = new Rect(_a.vOrigin, _a.vSize);
        Rect bRect = new Rect(_b.vOrigin, _b.vSize);

        return aRect.Overlaps(bRect);
    }
}