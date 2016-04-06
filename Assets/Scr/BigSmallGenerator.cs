using UnityEngine;
using System.Collections.Generic;
using System;

// work on it and design it on paper

public enum Tile
{
    Big_Floor,
    Big_Filler,
    Med_Floor,
    Med_Wall,
    Med_Filler,
    Small_Floor,
    Small_Pillar,
    Small_Cap,
    Small_Corner,
    Small_TJunc,
    Small_Wall,
    Small_Filler
}
internal class BSCell
{
    int state;
    int size;

    public int State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }

    public BSCell()
    {
        State = 1;// floor, wall (filler is a wall)
    }
}

/*
just set the big tiles a state, set the medium and small tiles around the big ones
*/

public class BSGenerator : MonoBehaviour {

    public Tile availableTiles;

    public List<GameObject> tilePrefab = new List<GameObject>();

    private int width = 20, height = 20;
    private int workingWidth, workingHeight;

    private List<Rect> rooms = new List<Rect>();
    private BSCell[,] bsCells;

    private const int TileUnits = 10;
	// Use this for initialization
	void Start () {
        //the working widths are the actuall width and height once we have smaller tiles
        workingWidth = width * 2 + 1;
        workingHeight = height * 2 + 1;

        InitCells();
        MakeRooms();
        //MakeCoridors();
        SetCells();
        DoLayout();

        //MakeFloor();
    }

    private void SetCells()
    {

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                foreach (Rect room in rooms)
                {
                    if (room.Contains(new Vector2(x, y)))
                    {
                        bsCells[x, y].State = 0;
                    }
                }
            }
        }
    }

    private void MakeCoridors()
    {
        throw new NotImplementedException();
    }

    private void DoLayout()
    {
        float spaX;
        float spaY;
        int newTile;// will hold the enum for the tile to spawn
        int neighbours;

        for (int y = 0; y < workingHeight; y++)
        {
            for (int x = 0; x < workingWidth; x++)
            {
                spaX = TileUnits * 2.5f * x;// world spawn locations
                spaY = TileUnits * 2.5f * y;

                if( (y % 2 == 0) && (x % 2 == 0) )// small cell
                {
                    neighbours = CheckNeighbours();
                }
            }
        }
    }

    private int CheckNeighbours()
    {
        throw new NotImplementedException();
    }

    private void InitCells()
    {
        bsCells = new BSCell[width, height];
    }

    private void MakeRooms()
    {
        rooms.Add(new Rect(new Vector2(1f, 1f), new Vector2(3, 3)));
        rooms.Add(new Rect(new Vector2(10f, 10f), new Vector2(3, 3)));
        foreach (Rect room in rooms)
        {

        }
    }

    private void MakeFloor()
    {
        float spaX;
        float spaY;

        for (int y = 0; y < workingHeight; y++)
        {
            for (int x = 0; x < workingWidth; x++)
            {
                spaX = TileUnits * 2.5f * x;
                spaY = TileUnits * 2.5f * y;
                if (y % 2 == 0)
                {
                    if (x % 2 == 0)
                    {
                        Instantiate(tilePrefab[(int)Tile.Small_Floor], new Vector3(spaX, 0, spaY), Quaternion.identity);
                        continue;
                    }
                    Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(spaX, 0, spaY), Quaternion.Euler(0, 0, 0));
                    continue;
                }
                if (x % 2 == 0)
                {
                    Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(spaX, 0, spaY), Quaternion.Euler(0, 90, 0));
                    continue;
                }
                Instantiate(tilePrefab[(int)Tile.Big_Floor], new Vector3(spaX, 0, spaY), Quaternion.identity);
                continue;
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    private int WorkingToBase(int working)
    {
        if (working % 2 != 0)
        {
            return (working - 1) / 2;
        }
        Debug.LogError("tried to convert even number to a working position, big tiles have to be odd positions");
        return 0;
    }
    //make a 11*11 floor out of big small tiles
}
