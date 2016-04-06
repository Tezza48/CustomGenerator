using System;
using System.Collections.Generic;
using UnityEngine;

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

/*
just set the big tiles a state, set the medium and small tiles around the big ones
*/

public class BSGenerator : MonoBehaviour {

    public Tile availableTiles;

    public List<GameObject> tilePrefab = new List<GameObject>();

    private int width = 10, height = 10;
    private int workingWidth, workingHeight;
    private int spawnInterval;

    private List<iRect> rooms = new List<iRect>();
    private int[,] cells;//0 = floor, 1 = ceiling

    private const int TileUnits = 10;
    private const int BIG_TILE_WIDTH = 40;
    private const int SMALL_TILE_WIDTH = 10;

    /*
    button to press that makes a big room.
    delete button
    */

	void Start () {
        spawnInterval = BIG_TILE_WIDTH / 2 + SMALL_TILE_WIDTH / 2;
        MakeEmptyRoom();
        //the working widths are the actuall width and height once we have smaller tiles
        //workingWidth = width * 2 + 1;
        //workingHeight = height * 2 + 1;
        //InitCells();
        //MakeRooms();
        //MakeCoridors();
        //SetCells();
        //DoLayout();
        //MakeFloor();
    }

    void MakeEmptyRoom()
    {
        //initialize cells
        cells = new int[width*2+1, height*2+1];
        for (int y = 0; y < height*2+1; y++)
        {
            for (int x = 0; x < width*2+1; x++)
            {
                cells[x, y] = 0;
            }
        }
        //make one big room
        //rooms coordinates just point to big cells
        iRect room = new iRect();//rectangle that's one less than the size of the array leaving a big tile and a small tile on the border
        rooms.Add(room);
        //make the cells on the borders into walls
        cells[MakeBig(room.X), MakeBig(room.Y)] = 1;
        cells[MakeBig(room.xMax), MakeBig(room.yMax)] = 1;

        #region Instatiation
        //make those cells onto gameobjects
        for (int y = 0; y < height*2+1; y++)
        {
            for (int x = 0; x < width * 2 + 1; x++)
            {
                //small cells
                if (y % 2 == 0 && x % 2 == 0)
                {
                    if (cells[x, y] == 0)
                    {
                        Instantiate(tilePrefab[(int)Tile.Small_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(tilePrefab[(int)Tile.Small_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                }

                //med cells
                else if (x % 2 == 0)
                {
                    if (cells[x, y] == 0)
                    {
                        Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 90f, 0f));
                    }
                    else
                    {
                        Instantiate(tilePrefab[(int)Tile.Med_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 90f, 0f));
                    }
                }
                else if (y % 2 == 0)
                {
                    if (cells[x, y] == 0)
                    {
                        Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(tilePrefab[(int)Tile.Med_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 0f, 0f));
                    }
                }

                //big cells
                else
                {
                    if (cells[x, y] == 0)
                    {
                        Instantiate(tilePrefab[(int)Tile.Big_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(tilePrefab[(int)Tile.Big_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                }
            }
        }
        #endregion
    }

    private int MakeBig(int small)
    {
        // convert coordinate of a big cell to a full coordinate
        return small * 2 + 1;
    }

    //private void SetCells()
    //{

    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            foreach (Rect room in rooms)
    //            {
    //                if (room.Contains(new Vector2(x, y)))
    //                {
    //                    bsCells[x, y].State = 0;
    //                }
    //            }
    //        }
    //    }
    //}

    //private void MakeCoridors()
    //{
    //    throw new NotImplementedException();
    //}

    //private void DoLayout()
    //{
    //    float spaX;
    //    float spaY;
    //    int newTile;// will hold the enum for the tile to spawn
    //    int neighbours;

    //    for (int y = 0; y < workingHeight; y++)
    //    {
    //        for (int x = 0; x < workingWidth; x++)
    //        {
    //            spaX = TileUnits * 2.5f * x;// world spawn locations
    //            spaY = TileUnits * 2.5f * y;

    //            if( (y % 2 == 0) && (x % 2 == 0) )// small cell
    //            {
    //                neighbours = CheckNeighbours();
    //            }
    //        }
    //    }
    //}

    //private int CheckNeighbours()
    //{
    //    throw new NotImplementedException();
    //}

    //private void InitCells()
    //{
    //    bsCells = new BSCell[width, height];
    //}

    //private void MakeRooms()
    //{
    //    rooms.Add(new Rect(new Vector2(1f, 1f), new Vector2(3, 3)));
    //    rooms.Add(new Rect(new Vector2(10f, 10f), new Vector2(3, 3)));
    //    foreach (Rect room in rooms)
    //    {
    //        continue;
    //    }
    //}

    //private void MakeFloor()
    //{
    //    float spaX;
    //    float spaY;

    //    for (int y = 0; y < workingHeight; y++)
    //    {
    //        for (int x = 0; x < workingWidth; x++)
    //        {
    //            spaX = TileUnits * 2.5f * x;
    //            spaY = TileUnits * 2.5f * y;
    //            if (y % 2 == 0)
    //            {
    //                if (x % 2 == 0)
    //                {
    //                    Instantiate(tilePrefab[(int)Tile.Small_Floor], new Vector3(spaX, 0, spaY), Quaternion.identity);
    //                    continue;
    //                }
    //                Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(spaX, 0, spaY), Quaternion.Euler(0, 0, 0));
    //                continue;
    //            }
    //            if (x % 2 == 0)
    //            {
    //                Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(spaX, 0, spaY), Quaternion.Euler(0, 90, 0));
    //                continue;
    //            }
    //            Instantiate(tilePrefab[(int)Tile.Big_Floor], new Vector3(spaX, 0, spaY), Quaternion.identity);
    //            continue;
    //        }
    //    }
    //}

    //private int WorkingToBase(int working)
    //{
    //    if (working % 2 != 0)
    //    {
    //        return (working - 1) / 2;
    //    }
    //    Debug.LogError("tried to convert even number to a working position, big tiles have to be odd positions");
    //    return 0;
    //}
    //make a 11*11 floor out of big small tiles
}
