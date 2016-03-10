enum Exit { NONE = 0, Up = 1, Right = 2, Down = 4, Left = 8 }
enum InverseExit { Up = 4, Right = 8, Down = 1, Left = 2 }

class Cell
{
    private int exits;

    public Cell()
    {
        exits = 0;
    }

    public int Exits
    {
        get
        {
            return exits;
        }

        set
        {
            exits = value;
        }
    }

    public int getNumExits()
    {
        int _exitCount = 0;

        _exitCount += ((exits & (int)Exit.Up) == (int)Exit.Up) ? 1 : 0;
        _exitCount += ((exits & (int)Exit.Right) == (int)Exit.Right) ? 1 : 0;
        _exitCount += ((exits & (int)Exit.Down) == (int)Exit.Down) ? 1 : 0;
        _exitCount += ((exits & (int)Exit.Left) == (int)Exit.Left) ? 1 : 0;

        return _exitCount;
    }

    public int getOrientation(ref int _orientation)
    {
        for (int i = 0; i < 4; i++)
        {
            if (((exits >> i) & 1) == 1)
            {
                return i;
            }
        }
        return 0;
    }
}