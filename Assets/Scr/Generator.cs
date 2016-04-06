using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum MazeTiles
{
    Filler,
    Deadend,
    Corner,
    Straight,
    Junction,
    Cross,
    UNDEFINED,
    Room
}

public enum RoomTiles
{
    Centre,
    Edge,
    Corner,
    DoorEdge,
    CornerOpenLeft,
    CornerOpenDown,
    CornerOpenBoth,
    UNDEFINED
}

public class Generator : MonoBehaviour {
    [Header("Generator Fields")]
    [SerializeField][Range(5, 50)] private int WIDTH = 40;
    [SerializeField][Range(5, 50)] private int HEIGHT = 40;
    private const int TILE_SIZE = 40;

    [Header("Room Fields")]
    [SerializeField][Range(5, 20)] private int roomsToSpawn = 13;
    private int maxRoomTries = 100;
    private int minRoomSize = 3;
    [SerializeField][Range(8, 20)] private int maxRoomSize = 8;

    [Header("Tile Fields")]
    public GameObject[] CoridoorTilePrefabs;
    public GameObject[] RoomTilePrefabs;

    private Cell[,] cells;
    private List<Room> Rooms = new List<Room>();
    private List<Line> hallways;

    // Use this for initialization
    void Start () {
        Generate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Generate()
    {
        cells = new Cell[WIDTH, HEIGHT];
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                cells[x, y] = new Cell();
            }
        }
        //Rooms.Add(new Room(0, 0, 6, 6));
        //Rooms.Add(new Room(0, 10, 6, 6));

        GenerateRooms();
        GenerateCoridoors(ref Rooms, out hallways);
        SetCoridoorCells();

        /*
        GenerateMaze();
        MakeDoors();
        */

        //foreach (Room thisRoom in Rooms)
        //{
        //    Debug.Log(thisRoom.Origin.ToString());
        //}
        //foreach (Line hallway in hallways)
        //{
        //    Debug.DrawLine(hallway.Origin1v3 * TILE_SIZE, hallway.Origin2v3 * TILE_SIZE, Color.cyan, 1000f, false);
        //}

        MakeTiles();
    }

    private void SetCoridoorCells()
    {
        foreach (Line hallway in hallways)
        {
            int[] startPos = new int[] { (int)hallway.O1.x, (int)hallway.O1.y };
            int[] endPos = new int[] { (int)hallway.O2.x, (int)hallway.O2.y };
            Vector3 origin = hallway.Origin1v3 * TILE_SIZE;
            Vector3 corner = Vector3.zero;
            Vector3 end = hallway.Origin2v3 * TILE_SIZE;

            int dx = endPos[0] - startPos[0];
            int dy = endPos[1] - startPos[1];

            //int isPosX = dx > 0 ? 1 : -1;
            //int isPosY = dy > 0 ? 1 : -1;
            
            //which way do i go first?

            switch (hallway.getLineStyle())
            {
                case Line.LineStyle.VertFirst:
                    corner = new Vector3(origin.x, 0, end.z);
                    // vertical
                    if (dy > 0) // if y up
                    {
                        cells[startPos[0], startPos[1]].Exits |= (int)Exit.Up;
                        for (int y = startPos[1] + 1; y <= endPos[1] - 1; y++)
                        {
                            cells[startPos[0], y].Exits |= (int)Exit.Down + (int)Exit.Up;
                        }
                        cells[startPos[0], endPos[1]].Exits |= (int)InverseExit.Up; // corner
                    }
                    else // if y down
                    {
                        cells[startPos[0], startPos[1]].Exits |= (int)Exit.Down;
                        for (int y = startPos[1] - 1; y >= endPos[1] + 1; y--)
                        {
                            cells[startPos[0], y].Exits |= (int)Exit.Down + (int)Exit.Up;
                        }
                        cells[startPos[0], endPos[1]].Exits |= (int)InverseExit.Down; // corner
                    }
                    //horizontal
                    if (dx > 0) // if x right
                    {
                        cells[startPos[0], endPos[1]].Exits |= (int)Exit.Right; // corner
                        for (int x = startPos[0] + 1; x <= endPos[0] - 1; x++)
                        {
                            cells[x, endPos[1]].Exits |= (int)Exit.Left + (int)Exit.Right;
                        }
                        cells[endPos[0], endPos[1]].Exits |= (int)InverseExit.Right; // end
                    }
                    else // if x left
                    {
                        cells[startPos[0], endPos[1]].Exits |= (int)Exit.Left; // corner
                        for (int x = startPos[0] - 1; x >= endPos[0] + 1; x--)
                        {
                            cells[x, endPos[1]].Exits |= (int)Exit.Left + (int)Exit.Right;
                        }
                        cells[endPos[0], endPos[1]].Exits |= (int)InverseExit.Left; // end
                    }
                    break;
                case Line.LineStyle.HorizFirst:
                    corner = new Vector3(end.x, 0, origin.z);
                    if (dx > 0) // x right
                    {
                        cells[startPos[0], startPos[1]].Exits |= (int)Exit.Right; // start
                        for (int x = startPos[0] + 1; x <= endPos[0] - 1; x++)
                        {
                            cells[x, startPos[1]].Exits |= (int)Exit.Left + (int)Exit.Right;
                        }
                        cells[endPos[0], startPos[1]].Exits |= (int)InverseExit.Right; // corner
                    }
                    else // x left
                    {
                        cells[startPos[0], startPos[1]].Exits |= (int)Exit.Left; // start
                        for (int x = startPos[0] - 1; x >= endPos[0] + 1; x--)
                        {
                            cells[x, startPos[1]].Exits |= (int)Exit.Left + (int)Exit.Right;
                        }
                        cells[endPos[0], startPos[1]].Exits |= (int)InverseExit.Left; // corner
                    }
                    if (dy > 0) // y up
                    {
                        cells[endPos[0], startPos[1]].Exits |= (int)Exit.Up; // corner
                        for (int y = startPos[1] + 1; y <= endPos[1] - 1; y++)
                        {
                            cells[endPos[0], y].Exits |= (int)Exit.Up + (int)Exit.Down;
                        }
                        cells[endPos[0], endPos[1]].Exits |= (int)InverseExit.Up; // end
                    }
                    else // y down
                    {
                        cells[endPos[0], startPos[1]].Exits |= (int)Exit.Up; // corner
                        for (int y = startPos[1] - 1; y >= endPos[1] + 1; y--)
                        {
                            cells[endPos[0], y].Exits |= (int)Exit.Up + (int)Exit.Down;
                        }
                        cells[endPos[0], endPos[1]].Exits |= (int)InverseExit.Down; // end
                    }
                    break;
                default:
                    Debug.LogError("Line didn't determine a correct orientation.");
                    break;
            }
            Debug.DrawLine(origin, corner, Color.cyan, 1000f, false);
            Debug.DrawLine(corner + Vector3.up, end + Vector3.up, Color.red, 1000f, false);
        }
    }

    private void MakeTiles()
    {
        Vector3 spawnPos;
        int spawnOrientation = 0;
        GameObject newTile = CoridoorTilePrefabs[0];
        Cell currentCell;
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                spawnPos = new Vector3(x * TILE_SIZE, 0, y * TILE_SIZE);
                spawnOrientation = 0;
                newTile = null;
                currentCell = cells[x, y];
                MazeTiles mTile = MazeTiles.UNDEFINED;
                RoomTiles tile = RoomTiles.UNDEFINED;
                Room currentRoom = null;
                foreach (Room checkRoom in Rooms)
                {
                    if (checkRoom.RoomContains(x, y))
                    {
                        currentRoom = checkRoom;
                        break;
                    }
                }
                currentRoom = null;
                if (currentRoom != null)
                {
                    mTile = MazeTiles.Room;
                    tile = currentRoom.CheckPosition(ref spawnOrientation, x, y);
                    if (tile == RoomTiles.Edge)
                    {
                        // the orientation is the bit we need to check -1
                        //if (spawnOrientation == 3)
                        //{
                        //    spawnOrientation = -1;
                        //}
                        if ((currentCell.Exits & (spawnOrientation)) == 1)
                        {
                            tile = RoomTiles.DoorEdge;
                        }
                    }
                    else if (tile == RoomTiles.Corner)
                    {
                        switch (spawnOrientation)
                        {
                            case 0:
                                switch (currentCell.Exits)
                                {
                                    case (int)Exit.Left:
                                        tile = RoomTiles.CornerOpenLeft;
                                        break;
                                    case (int)Exit.Down:
                                        tile = RoomTiles.CornerOpenDown;
                                        break;
                                    case 12:
                                        tile = RoomTiles.CornerOpenBoth;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 1:
                                switch (currentCell.Exits)
                                {
                                    case (int)Exit.Up:
                                        tile = RoomTiles.CornerOpenLeft;
                                        break;
                                    case (int)Exit.Left:
                                        tile = RoomTiles.CornerOpenDown;
                                        break;
                                    case 9:
                                        tile = RoomTiles.CornerOpenBoth;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 2:
                                switch (currentCell.Exits)
                                {
                                    case (int)Exit.Right:
                                        tile = RoomTiles.CornerOpenLeft;
                                        break;
                                    case (int)Exit.Up:
                                        tile = RoomTiles.CornerOpenDown;
                                        break;
                                    case 3:
                                        tile = RoomTiles.CornerOpenBoth;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 3:
                                switch (currentCell.Exits)
                                {
                                    case (int)Exit.Down:
                                        tile = RoomTiles.CornerOpenLeft;
                                        break;
                                    case (int)Exit.Right:
                                        tile = RoomTiles.CornerOpenDown;
                                        break;
                                    case 6:
                                        tile = RoomTiles.CornerOpenBoth;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    newTile = RoomTilePrefabs[(int)tile];
                }
                else
                {
                    int exits = currentCell.getNumExits();
                    switch (exits)
                    {
                        case 0:
                            //newTile = CoridoorTilePrefabs[(int)MazeTiles.Filler];
                            // mTile = MazeTiles.Filler;
                            break;
                        case 1:
                            mTile = MazeTiles.Deadend;
                            currentCell.getFirstExit(ref spawnOrientation);
                            break;
                        case 2:
                            if (currentCell.Exits == 10 || currentCell.Exits == 5)
                            {
                                mTile = MazeTiles.Straight;
                                currentCell.getFirstExit(ref spawnOrientation);
                            }
                            else
                            {
                                mTile = MazeTiles.Corner;
                                if (currentCell.Exits == 3)
                                {
                                    spawnOrientation = 0;
                                }
                                else if (currentCell.Exits == 6)
                                {
                                    spawnOrientation = 1;
                                }
                                else if (currentCell.Exits == 12)
                                {
                                    spawnOrientation = 2;
                                }
                                else if (currentCell.Exits == 9)
                                {
                                    spawnOrientation = 3;
                                }
                            }
                            break;
                        case 3:
                            mTile = MazeTiles.Junction;
                            if (currentCell.Exits == 7)
                            {
                                spawnOrientation = 0;
                            }
                            else if (currentCell.Exits == 14)
                            {
                                spawnOrientation = 1;
                            }
                            else if (currentCell.Exits == 13)
                            {
                                spawnOrientation = 2;
                            }
                            else if (currentCell.Exits == 11)
                            {
                                spawnOrientation = 3;
                            }
                            break;
                        case 4:
                            mTile = MazeTiles.Cross;
                            break;
                        default:
                            newTile = new GameObject("Error Tile");
                            break;
                    }
                    if (mTile != MazeTiles.UNDEFINED)
                    {
                        newTile = CoridoorTilePrefabs[(int)mTile];
                        spawnOrientation = currentCell.getFirstExit(ref spawnOrientation);
                    }
                }
                if (mTile != MazeTiles.UNDEFINED)
                {
                    GameObject spawnedTile = (GameObject) Instantiate(newTile, spawnPos, Quaternion.Euler(0f, 90*spawnOrientation ,0f));
                    spawnedTile.name = "(" + x + ", " + y + ") " + newTile.name;
                    GameCell cellinfo = spawnedTile.AddComponent<GameCell>();
                    cellinfo.Initialize(mTile, currentCell.Exits);
                }
            }
        }
    }

    private void MakeDoors()
    {
        throw new NotImplementedException();
    }

    private void GenerateCoridoors(ref List<Room> _Rooms, out List<Line> _hallways)
    {
        _hallways = new List<Line>();
        // first room is starting point so dont do anything
        for (int i = 1; i < _Rooms.Count; i++)
        {
            // as this is the second room, we assume it's the closest room yet
            int closestRoom = i;
            float closestDistance = Vector2.Distance(_Rooms[i-1].Origin, _Rooms[i].Origin);
            // check distance to all other rooms
            for (int j = i + 1; j < _Rooms.Count; j++)
            {
                float currentDistance = Vector2.Distance(_Rooms[j].Origin, _Rooms[i].Origin);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestRoom = j;
                }
            }
            // make the closest room the second in the list
            if(closestRoom != i)
            {
                SwapRooms(ref _Rooms, i, closestRoom);
            }
            // repeat for all of the rooms
        }
        // the rooms are now sorted in an order
        // now make hallways between the rooms
        for (int i = 1; i < _Rooms.Count; i++)
        {
            _hallways.Add(new Line(_Rooms[i - 1].Origin, _Rooms[i].Origin));
        }
    }

    private void SwapRooms(ref List<Room> _Rooms, int i, int closestRoom)
    {
        Room swapRoom = _Rooms[i];
        _Rooms[i] = _Rooms[closestRoom];
        _Rooms[closestRoom] = swapRoom;
    }

    private void GenerateRooms()
    {
        for (int i = 0; i < roomsToSpawn; i++)
        {
            int tryCounter = maxRoomTries;
            while (tryCounter > 0)
            {
                int xPos = UnityEngine.Random.Range(0, WIDTH - minRoomSize);
                int yPos = UnityEngine.Random.Range(0, HEIGHT - minRoomSize);
                int roomWidth = UnityEngine.Random.Range(minRoomSize, maxRoomSize);
                int roomHeight = UnityEngine.Random.Range(minRoomSize, maxRoomSize);

                Room newRoom = new Room(xPos, yPos, roomWidth, roomHeight);

                if (xPos + roomWidth + 1 > WIDTH || yPos + roomHeight + 1 > HEIGHT)
                {
                    continue;
                }

                bool isValid = true;
                foreach (Room currentRoom in Rooms)
                {
                    if (newRoom.RoomRect.Overlaps(currentRoom.RoomRect))
                    {
                        isValid = false;
                        break;
                    }
                }

                // Debug.Log(isValid.ToString() + newRoom.X + ", "  + newRoom.Y + ", " + (newRoom.Width + newRoom.X).ToString() + ", " + (newRoom.Height + newRoom.Y).ToString());

                if (isValid)
                {
                    Rooms.Add(newRoom);
                    break;
                }

                tryCounter--;
            }
        }
    }
}
