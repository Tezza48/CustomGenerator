using UnityEngine;

internal class Line
{
    //public enum LineStyle
    //{
    //    VertFirst, HorizFirst

    //}

    private Vector2 origin1, origin2;

    private bool isHoriz;

    public Vector2 O1 { get {  return origin1; } }
    public Vector2 O2 { get { return origin2; } }

    public Vector3 Origin1v3 { get { return new Vector3(origin1.x, 0, origin1.y); } }
    public Vector3 Origin2v3 { get { return new Vector3(origin2.x, 0, origin2.y); } }

    public bool IsHoriz { get { return isHoriz; } }

    public Line(Vector2 origin1, Vector2 origin2, bool _isHoriz)
    {
        this.origin1 = origin1;
        this.origin2 = origin2;
        isHoriz = _isHoriz;
    }

    //public bool PointOnLine(int _x, int _y)
    //{
    //    // return whether this line intersects the point
    //    // make line segments for edges of the cell

    //    // see if it intersects any of the segments
    //    return false;
    //}

    //public LineStyle getLineStyle ()
    //{
    //    //return LineStyle.VertFirst;
    //    int dy = (int)(O2.y - O1.y);
    //    int dx = (int)(O2.x - O1.x);

    //    float gradient = (dx != 0) ? dy / dx : 0;
    //    if (gradient > 1)
    //    {
    //        return LineStyle.VertFirst;
    //    }
    //    else if (gradient < 1)
    //    {
    //        return LineStyle.HorizFirst;
    //    }
    //    else
    //    {
    //        return (LineStyle)Random.value;
    //    }
    //}
}