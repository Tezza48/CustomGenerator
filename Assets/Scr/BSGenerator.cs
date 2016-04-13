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

    private List<iRect> rooms = new List<iRect>();
    private List<iLine> coridors = new List<iLine>();
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
        rooms = new List<iRect>();
        coridors = new List<iLine>();
    }

    public void MakeEmptyRoom()
    {
        DeleteGrid();
        //initialize cells
        InnitCells(ref cells);
        //make one big room
        //rooms coordinates just point to big cells
        iRect room = new iRect(0, 0, width, height);//rectangle that's one less than the size of the array leaving a big tile and a small tile on the border
        rooms.Add(room);

        #region Manual_Room_Setting
        //make the cells on the borders into walls
        for (int x = room.X; x < room.xMax - 1; x++)
        {
            cells[x * 2, room.Y * 2] |= 8;//small
            cells[x * 2 + 1, room.Y * 2] |= 8;//med

            cells[x * 2, (room.yMax - 1) * 2] |= 2;
            cells[x * 2 + 1, (room.yMax - 1) * 2] |= 2;
        }
        for (int y = room.Y; y < room.yMax - 1; y++)
        {
            cells[room.X * 2, y * 2] |= 4;
            cells[room.X * 2, y * 2 + 1] |= 4;

            cells[(room.xMax - 1) * 2, y * 2] |= 1;
            cells[(room.xMax - 1) * 2, y * 2 + 1] |= 1;
        }
        cells[(room.xMax - 1) * 2, (room.xMax - 1) * 2] |= 3;//last corner
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
                _cells[x, y] = 0;
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

                iRect newRoom = new iRect(xPos, yPos, roomWidth, roomHeight);

                if (xPos + roomWidth > width || yPos + roomHeight > height)
                {
                    continue;
                }

                bool isValid = true;
                foreach (iRect currentRoom in rooms)
                {
                    if (iRect.Intersects(newRoom, currentRoom))
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
            foreach (iRect currentRoom in rooms)
            {

                //make the cells on the borders into walls
                for (int x = currentRoom.X; x < currentRoom.xMax - 1; x++)
                {
                    cells[x * 2, currentRoom.Y * 2] |= 8;//small
                    cells[x * 2 + 1, currentRoom.Y * 2] |= 8;//med

                    cells[x * 2, (currentRoom.yMax - 1) * 2] |= 2;
                    cells[x * 2 + 1, (currentRoom.yMax - 1) * 2] |= 2;
                }
                for (int y = currentRoom.Y; y < currentRoom.yMax - 1; y++)
                {
                    cells[currentRoom.X * 2, y * 2] |= 4;
                    cells[currentRoom.X * 2, y * 2 + 1] |= 4;

                    cells[(currentRoom.xMax - 1) * 2, y * 2] |= 1;
                    cells[(currentRoom.xMax - 1) * 2, y * 2 + 1] |= 1;
                }
                cells[(currentRoom.xMax - 1) * 2, (currentRoom.yMax - 1) * 2] |= 3;//last corner
            }
            #endregion
        }
    }

    private void MakeCoridors(ref int[,] _cells)
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            coridors.Add(new iLine(rooms[i-1].Centre, rooms[i].Centre));
        }
        foreach (iLine currentCoridor in coridors)
        {
            if (currentCoridor.isHorizFirst())
            {
                if (currentCoridor.hasPositiveDelta())
                {
                    for (int x = currentCoridor.Start[0]; x < currentCoridor.End[0]; x++)
                    {
                        int realX = x * 2;
                        //12_
                        //3__
                        //45_
                        cells[realX - 1, currentCoridor.Start[1] * 2 - 1] |= 2;
                        cells[realX, currentCoridor.Start[1] * 2 - 1] |= 2;
                        //cells[realX + 1, currentCoridor.Start[1] * 2 - 1] |= 2;

                        cells[realX - 1, currentCoridor.Start[1] * 2] = 0;

                        cells[realX - 1, currentCoridor.Start[1] * 2 + 1] |= 8;
                        cells[realX, currentCoridor.Start[1] * 2 + 1] |= 8;
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
                    if (_cells[x, y] == 15)
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Big_Filler], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                    else
                    {
                        spawnedTile = (GameObject)Instantiate(tilePrefab[(int)Tile.Big_Floor], new Vector3(x * spawnInterval, 0, y * spawnInterval), Quaternion.identity);
                    }
                }
                spawnedTiles.Add(spawnedTile);
                spawnedTile.name = "(" + x + ", " + y + ") Exits: " + _cells[x, y];
            }
        }
    }
    #endregion
}