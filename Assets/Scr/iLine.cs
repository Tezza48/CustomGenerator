using UnityEngine;

class iLine
{
    private int[] start, end;

    public int[] Start { get { return start; } }
    public int[] End { get { return end; } }

    //public Vector2 vStart { get { return new Vector2(start[0], start[1]); } }
    //public Vector2 vEnd { get { return new Vector2(end[0], end[1]); } }

    public iLine()
    {
        start = new int[0];
        end = new int[0];
    }

    public iLine (int[] _start, int[] _end)
    {
        start = _start;
        end = _end;
    }

    public bool isHorizFirst()
    {
        float dy = end[1] - start[1];
        float dx = end[0] - start[0];

        float gradient = (dx != 0) ? dy / dx : 0;

        if (gradient > 1)
            return false;
        else if (gradient < 1)
            return true;
        else
            return Random.value > 0.5;
    }

    public bool hasPositiveDelta()
    {
        if (isHorizFirst())
            return (end[0] > start[0]);
        else
            return (end[1] > start[1]);
    }
}
