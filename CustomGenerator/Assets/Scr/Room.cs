using UnityEngine;

class Room
{
    private int x, y, width, height;

    private Vector2 origin;

    #region Properties
    public int Height { get { return height; } }

    public int Width { get { return width; } }

    public int X { get { return x; } }

    public int Y { get { return y; } }

    public Rect RoomRect { get { return new Rect(x-1, y-1, width+2, height+2);  } }

    public Vector2 Origin { get { return origin; } }
    #endregion

    public Room(int _x, int _y, int _width, int _height)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        origin = new Vector2( x + (width / 2), y + (height/2) );
    }

    public bool RoomContains(int _x, int _y)
    {
        if (_x >= x && _x <= x + width)
        {
            if (_y >= y && _y <= y + height)
            {
                return true;
            }
        }
        return false;
    }

    public RoomTiles CheckPosition(ref int orientation, int _x, int _y)
    {
        RoomTiles tile = RoomTiles.Centre;
        // if corner
        if (_x == x && _y == y)
        {
            orientation = 3;
            return RoomTiles.Corner;
        }
        else if (_x == x + width && _y == y)
        {
            orientation = 2;
            return RoomTiles.Corner;
        }
        else if (_x == x && _y == y + height)
        {
            orientation = 0;
            return RoomTiles.Corner;
        }
        else if (_x == x + width && _y == y + height)
        {
            orientation = 1;
            return RoomTiles.Corner;
        }

        // if edge
        else if (_x == x)
        {
            orientation = 3;
            return RoomTiles.Edge;
        }
        else if (_y == y)
        {
            orientation = 2;
            return RoomTiles.Edge;
        }
        else if (_x == x + width)
        {
            orientation = 1;
            return RoomTiles.Edge;
        }
        else if (_y == y + height)
        {
            orientation = 0;
            return RoomTiles.Edge;
        }
        //else centre
        orientation = UnityEngine.Random.Range(0, 4);
        return tile;
    }

    /* public bool DoesOverlap(Room _checkRoom)
    {

        bool bottomLeft = RoomContains(_checkRoom.X, _checkRoom.Y);
        bool bottomRight = RoomContains(_checkRoom.Width, _checkRoom.Y);
        bool topLeft = RoomContains(_checkRoom.X, _checkRoom.Height);
        bool topRight = RoomContains(_checkRoom.Width, _checkRoom.Height);

        if (bottomLeft || bottomRight || topLeft || topRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
}