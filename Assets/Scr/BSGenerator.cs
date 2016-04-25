using System.Collections.Generic;
using UnityEngine;

// work on it and design it on paper

public enum Tile
{
    Big_Filler,
    Big_Floor,
    Med_Filler,
    Med_Floor,
    Med_Wall,
    Small_Cap,
    Small_Corner,
    Small_Filler,
    Small_Floor,
    Small_Pillar,
    Small_TJunc,
    Small_Wall
}

/*
just set the big tiles a state, set the medium and small tiles around the big ones
*/

public class BSGenerator : MonoBehaviour
{

    [Header("Helper")]
    public Tile availableTiles;

    [Header("Tile Prefabs")]
    public List<GameObject> tilePrefab = new List<GameObject>();

    [Header("Spawning Settings")]
    [Tooltip("Recommended greater or equal to 10 by 10.")]
    public int width = 10;
    [Tooltip("Recommended greater or equal to 10 by 10.")]
    public int height = 10;
    [Range(1, 20)]
    public int maxRooms = 10;
    [Range(3, 10)]
    public int minRoomSize = 3;
    [Range(3, 20)]
    public int maxRoomSize = 5;
    [Tooltip("Number of times the generator will try\nto place a new room. the higher the number,\nthe more likely the max number of rooms will spawn.")]
    public int maxRoomTries = 100;

    [Header("Tile Settings")]
    public const int TileUnits = 10;
    public const int BIG_TILE_WIDTH = 40;
    public const int SMALL_TILE_WIDTH = 10;

    #region Private_Variables
    private int spawnInterval;

    private List<Rect> rooms = new List<Rect>();
    private List<Line> coridors = new List<Line>();
    private List<GameObject> spawnedTiles = new List<GameObject>();
    private int[,] cells;//bit flag for cardinal the wall is on NESW 8421

    #endregion

    /*
    button to press that makes a big room.
    delete button
    */

    void Start()
    {
        spawnInterval = BIG_TILE_WIDTH / 2 + SMALL_TILE_WIDTH / 2;

        //MakeEmptyRoom();
        //InnitCells(out cells);
        //AddRooms(ref cells);
    }

    #region Spawn_Buttons
    public void DeleteGrid()
    {
        foreach (GameObject currentTile in spawnedTiles)
        {
            Destroy(currentTile);
        }
        spawnedTiles = new List<GameObject>();
        rooms = new List<Rect>();
        coridors = new List<Line>();
    }

    public void MakeEmptyRoom()
    {
        DeleteGrid();
        //initialize cells
        InnitCells(ref cells);
        //make one big room
        //rooms coordinates just point to big cells
        Rect room = new Rect(0, 0, width, height);//rectangle that's one less than the size of the array leaving a big tile and a small tile on the border
        rooms.Add(room);

        #region Manual_Room_Setting
        //make the cells on the borders into walls
        foreach (Rect currentRoom in rooms)
        {

            //make the cells on the borders into walls
            for (int x = (int)currentRoom.x; x < currentRoom.xMax; x++)
            {
                for (int y = (int)currentRoom.y; y < currentRoom.yMax; y++)
                {
                    cells[x * 2 + 1, y * 2 + 1] = 0;
                    //cells[x * 2 + 1, y * 2] = 0;
                    //cells[x * 2, y * 2 + 1] = 0;
                    //cells[x * 2 + 1, y * 2 + 1] = 0;
                }
            }
            cells[((int)currentRoom.xMax - 1) * 2, ((int)currentRoom.yMax - 1) * 2] = 1;//last corner
        }
        #endregion

        InitialiseTiles(cells);
    }

    public void SpawnRooms()
    {
        DeleteGrid();

        InnitCells(ref cells);

        AddRooms(ref cells, true);

        InitialiseTiles(cells);
    }

    public void SpawnCoridors()
    {
        DeleteGrid();

        InnitCells(ref cells);

        AddRooms(ref cells, false);

        MakeCoridors(ref cells);

        InitialiseTiles(cells);
    }

    public void FullSpawn()
    {
        DeleteGrid();

        InnitCells(ref cells);

        AddRooms(ref cells);

        MakeCoridors(ref cells);

        InitialiseTiles(cells);
    }
    #endregion

    #region Spawning_Methods
    private void InnitCells(ref int[,] _cells)
    {
        //initialize cells
        _cells = new int[width * 2 + 1, height * 2 + 1];
        for (int y = 0; y < height * 2 + 1; y++)
        {
            for (int x = 0; x < width * 2 + 1; x++)
            {
                _cells[x, y] = 1;
            }
        }
    }

    private void AddRooms(ref int[,] _cells, bool writeToCells = true)
    {
        #region Generate_Rooms
        for (int i = 0; i < maxRooms; i++)
        {
            int tries = maxRoomTries;
            while (tries > 0)
            {
                int xPos = Random.Range(0, width - minRoomSize);
                int yPos = Random.Range(0, height - minRoomSize);
                int roomWidth = Random.Range(minRoomSize, maxRoomSize);
                int roomHeight = Random.Range(minRoomSize, maxRoomSize);

                Rect newRoom = new Rect(xPos, yPos, roomWidth, roomHeight);

                if (xPos + roomWidth > width || yPos + roomHeight > height)
                {
                    continue;
                }

                bool isValid = true;
                foreach (Rect currentRoom in rooms)
                {
                    if (newRoom.Overlaps(currentRoom))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    rooms.Add(newRoom);
                    break;
                }

                tries--;
            }
        }
        #endregion
        if (writeToCells)
        {
            #region Write_Rooms_To_Cells
            foreach (Rect currentRoom in rooms)
            {

                //make the cells on the borders into walls
                for (int x = (int)currentRoom.x; x < currentRoom.xMax; x++)
                {
                    for (int y = (int)currentRoom.y; y < currentRoom.yMax; y++)
                    {
                        cells[x * 2+1, y * 2+1] = 0;
                        if (x > currentRoom.xMin)
                        {
                            cells[x * 2, y * 2 + 1] = 0;
                        }
                        if (y > currentRoom.yMin)
                        {
                            cells[x * 2 + 1, y * 2] = 0;
                        }
                        if (x > currentRoom.xMin && y > currentRoom.yMin)
                        {

                            cells[x * 2, y * 2] = 0;
                        }
                    }
                }
            }
            #endregion
        }
    }

    private void MakeCoridors(ref int[,] _cells)
    {
        for (int i = 1; i < rooms.Count; i++)
        {

            //horiz first
            Vector2 start = new Vector2(Mathf.Floor(rooms[i - 1].center.x), Mathf.Floor(rooms[i - 1].center.y));
            Vector2 end = new Vector2(Mathf.Floor(rooms[i].center.x), Mathf.Floor(rooms[i].center.y));
            Vector2 corner = new Vector2(end.x, start.y);
            
            Debug.DrawLine(new Vector3(start.x*spawnInterval*2, 0, start.y * spawnInterval*2),
                new Vector3(end.x * spawnInterval*2, 0, end.y * spawnInterval*2),
                Color.black,
                5f,
                false);

            //make a horizontal coridor
            coridors.Add(new Line(start, corner, true));
            //then make the vertical component
            coridors.Add(new Line(corner, end, false));
        }
        //write to the cells.
        foreach (Line coridor in coridors)
        {
            //write here, here and here
            if (coridor.IsHoriz)
            {
                int y = (int)coridor.O1.y;

                bool isIncreasing = coridor.O1.x < coridor.O2.x;

                if (isIncreasing)
                {
                    for (int x = (int)coridor.O1.x; x < coridor.O2.x; x++)
                    {
                        cells[x * 2 + 1, y * 2 + 1] = 0;
                        cells[x * 2 + 2, y * 2 + 1] = 0;
                    }
                }
                else
                {
                    for (int x = (int)coridor.O1.x; x > coridor.O2.x; x--)
                    {
                        cells[x * 2 + 1, y * 2 + 1] = 0;
                        cells[x * 2, y * 2 + 1] = 0;
                    }
                }
            }
            else
            {
                int x = (int)coridor.O1.x;

                bool isIncreasing = coridor.O1.y < coridor.O2.y;
                if (isIncreasing)
                {
                    for (int y = (int)coridor.O1.y; y < coridor.O2.y; y++)
                    {
                        cells[x * 2 + 1, y * 2 + 1] = 0;
                        cells[x * 2 + 1, y * 2 + 2] = 0;
                    }
                }
                else
                {
                    for (int y = (int)coridor.O1.y; y > coridor.O2.y; y--)
                    {
                        cells[x * 2 + 1, y * 2 + 1] = 0;
                        cells[x * 2 + 1, y * 2] = 0;
                    }
                }
            }
        }
    }

    private void InitialiseTiles(int[,] _cells)
    {
        GameObject spawnedTile;
        for (int y = 0; y < height * 2 + 1; y++)
        {
            for (int x = 0; x < width * 2 + 1; x++)
            {
                //small cells
                if (y % 2 == 0 && x % 2 == 0)
                {
                    if (_cells[x, y] == 0)
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Small_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                    else
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Small_Pillar], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                }

                //med cells
                else if (x % 2 == 0)
                {
                    if (_cells[x, y] == 0)
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 90f, 0f));
                    }
                    else
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Med_Wall], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 90f, 0f));
                    }
                }
                else if (y % 2 == 0)
                {
                    if (_cells[x, y] == 0)
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Med_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Med_Wall], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.Euler(0f, 0f, 0f));
                    }
                }

                //big cells
                else
                {
                    if (_cells[x, y] == 0)
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Big_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                    else
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Big_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                }
                spawnedTiles.Add(spawnedTile);
                spawnedTile.name = "(" + x + ", " + y + ") Exits: " + _cells[x, y];
            }
        }
    }
    #endregion
}