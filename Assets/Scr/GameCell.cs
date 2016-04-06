using UnityEngine;
using System.Collections;

public class GameCell : MonoBehaviour {
    public MazeTiles cell;
    public int exits;
    public void Initialize(MazeTiles _cell, int _exits)
    {
        cell = _cell;
        exits = _exits;

    }
}