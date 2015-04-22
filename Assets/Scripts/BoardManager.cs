using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

//With inspiration from http://unity3d.com/learn/tutorials/projects/2d-roguelike/boardmanager

public class BoardManager : MonoBehaviour
{

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] stairTiles;
    public GameObject[] itemTiles;

    public static List<Level> levels = new List<Level>();

    public static List<Vector3> floors = new List<Vector3>();
    public static List<Vector3> walls = new List<Vector3>();
    public static Vector3 exit = new Vector3();
    public static Vector3 entrance = new Vector3();

    public static List<Border> borders = new List<Border>();
    public static List<Border> connections = new List<Border>();

    public static List<Vector3> allPositions = new List<Vector3>();

    public static List<Room> rooms = new List<Room>();

    private Transform boardHolder;

    public class Level
    {
        public int levelNumber;
        public List<Vector3> levelFloors;
        public List<Vector3> levelWalls;
        public Vector3 levelExitStair;
        public Vector3 levelEntranceStair;

        public Level(int levelNum)
        {
            levelNumber = levelNum;
            levelFloors = floors;
            levelWalls = walls;
            levelExitStair = exit;
            levelEntranceStair = entrance;
        }
    }

    public class Border
    {
        public Vector3 position;
        public int directionToAdd;

        public Border(Vector3 pos, int dir)
        {
            position = pos;
            directionToAdd = dir;
        }
    }

    public class Room
    {
        public List<Vector3> floorSpaces;
        public List<Vector3> wallSpaces;
        public Room(Vector3 pos, int width, int height)
        {
            floorSpaces = new List<Vector3>();
            wallSpaces = new List<Vector3>();
            
            for (int y = (int)pos.y; y < pos.y + height; y++)
            {
                for (int x = (int)pos.x; x < pos.x + width; x++)
                {
                    if ((x == pos.x || x == pos.x + width - 1) || (y == pos.y || y == pos.y + height - 1))
                    {
                        wallSpaces.Add(new Vector3(x, y, 0f));
                        walls.Add(new Vector3(x, y, 0f));
                        allPositions.Add(new Vector3(x, y, 0f));

                        if (y > pos.y && y < pos.y + height - 1)
                        {
                            if (x == pos.x)//add to left
                            {
                                borders.Add(new Border(new Vector3(x - 2, y, 0f), 4));
                            }
                            else if (x == pos.x + width - 1)//add to right
                            {
                                borders.Add(new Border(new Vector3(x + 2, y, 0f), 2));
                            }
                        }
                        else if (x > pos.x && x < pos.x + width - 1)
                        {
                            if (y == pos.y)//add to bottom
                            {
                                borders.Add(new Border(new Vector3(x, y - 2, 0f), 3));
                            }
                            else if (y == pos.y + height - 1)//add to top
                            {
                                borders.Add(new Border(new Vector3(x, y + 2, 0f), 1));
                            }
                        }
                    }
                    else
                    {
                        floorSpaces.Add(new Vector3(x, y, 0f));
                        floors.Add(new Vector3(x, y, 0f));
                        allPositions.Add(new Vector3(x, y, 0f));
                    }
                }
            }
        }
    }

    bool checkSpace(Vector3 bottomLeft, int width, int height)
    {
        bool result = true;

        for (int y = (int)bottomLeft.y - 1; y < bottomLeft.y + height + 1; y++)
        {
            for (int x = (int)bottomLeft.x - 1; x < bottomLeft.x + width + 1; x++)
            {
                if (allPositions.Contains(new Vector3(x,y,0f)))
                {
                    result = false;
                }
            }
        }

            return result;
    }

    void BuildConnection(Border connection)
    {
        if (connection.directionToAdd == 1 || connection.directionToAdd == 3)
        {
            for (int y = (int)connection.position.y - 2; y < connection.position.y + 3; y++)
            {
                for (int x = (int)connection.position.x - 1; x < connection.position.x + 2; x++)
                {
                    Vector3 hallway = new Vector3(x, y, 0f);
                    if (walls.Contains(hallway))
                        walls.Remove(hallway);
                    if (x == connection.position.x)
                        floors.Add(hallway);
                    else
                        walls.Add(hallway);
                    if (!allPositions.Contains(hallway))
                    {
                        allPositions.Add(hallway);
                    }
                }
            }
        }
        else if (connection.directionToAdd == 2 || connection.directionToAdd == 4)
        {
            for (int y = (int)connection.position.y - 1; y < connection.position.y + 2; y++)
            {
                for (int x = (int)connection.position.x - 2; x < connection.position.x + 3; x++)
                {
                    Vector3 hallway = new Vector3(x, y, 0f);
                    if (walls.Contains(hallway))
                        walls.Remove(hallway);
                    if (y == connection.position.y)
                        floors.Add(hallway);
                    else
                        walls.Add(hallway);
                }
            }
        }
    }

    void BuildRooms(int level)
    {
        int roomGoal = Random.Range(level, level * 2 + 1);


        rooms.Add(new Room(new Vector3(0, 0, 0), Random.Range(5, 14), Random.Range(5, 14)));
        int roomAttemptCounter = 500;
        while (roomAttemptCounter > 0 && rooms.Count < roomGoal)
        {
            Border connection = borders[Random.Range(0, borders.Count)];

            if (connection.directionToAdd == 1)//add room above
            {
                int left = Random.Range(1, 12);

                Vector3 bottomLeft = new Vector3(connection.position.x - left, connection.position.y + 2, 0f);

                int height = Random.Range(5, 14);
                int width;
                if (left + 2 < 5)
                    width = Random.Range(5, 14);
                else
                    width = Random.Range(left + 2, 14);

                if (checkSpace(bottomLeft, width, height))
                {
                    rooms.Add(new Room(bottomLeft, width, height));
                    connections.Add(connection);
                    BuildConnection(connection);
                    borders.Remove(connection);
                }
                else
                {
                    roomAttemptCounter--;
                }

            }
            else if (connection.directionToAdd == 2)//add room right
            {
                int down = Random.Range(1, 12);

                Vector3 bottomLeft = new Vector3(connection.position.x + 2, connection.position.y - down, 0f);

                int width = Random.Range(5, 14);
                int height;
                if (down + 2 < 5)
                    height = Random.Range(5, 14);
                else
                    height = Random.Range(down + 2, 14);

                if (checkSpace(bottomLeft, width, height))
                {
                    rooms.Add(new Room(bottomLeft, width, height));
                    connections.Add(connection);
                    BuildConnection(connection);
                    borders.Remove(connection);
                }
                else
                {
                    roomAttemptCounter--;
                }
            }
            else if (connection.directionToAdd == 3)//add room below
            {
                int left = Random.Range(1, 12);
                int down = Random.Range(6, 15);

                Vector3 bottomLeft = new Vector3(connection.position.x - left, connection.position.y - down, 0f);

                int height = down - 1;

                int width;
                if (left + 2 < 5)
                    width = Random.Range(5, 14);
                else
                    width = Random.Range(left + 2, 14);

                if (checkSpace(bottomLeft, width, height))
                {
                    rooms.Add(new Room(bottomLeft, width, height));
                    connections.Add(connection);
                    BuildConnection(connection);
                    borders.Remove(connection);
                }
                else
                {
                    roomAttemptCounter--;
                }
            }
            else if (connection.directionToAdd == 4)//add room left
            {
                int left = Random.Range(6, 15);
                int down = Random.Range(1, 12);

                Vector3 bottomLeft = new Vector3(connection.position.x - left, connection.position.y - down, 0f);

                int width = left - 1;
                int height;
                if (down + 2 < 5)
                    height = Random.Range(5, 14);
                else
                    height = Random.Range(down + 2, 14);

                if (checkSpace(bottomLeft, width, height))
                {
                    rooms.Add(new Room(bottomLeft, width, height));
                    connections.Add(connection);
                    BuildConnection(connection);
                    borders.Remove(connection);
                }
                else
                {
                    roomAttemptCounter--;
                }
            }
        }
    }

    void AddStairs()
    {
        int roomIndex = Random.Range(0, rooms.Count);
        int floorIndex = Random.Range(0, rooms[roomIndex].floorSpaces.Count);
        exit = rooms[roomIndex].floorSpaces[floorIndex];

        int newRoomIndex = Random.Range(0, rooms.Count);

        if (rooms.Count > 1)
        {
            while(newRoomIndex==roomIndex)
            {
                newRoomIndex = Random.Range(0, rooms.Count);
            }
        }
        
        floorIndex = Random.Range(0, rooms[newRoomIndex].floorSpaces.Count);
        entrance = rooms[newRoomIndex].floorSpaces[floorIndex];
    }

    void DisplayScene()
    {
        foreach(Vector3 wall in walls)
        {
            GameObject toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
            toInstantiate = Instantiate(toInstantiate, wall, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(boardHolder);
        }
        foreach(Vector3 floor in floors)
        {
            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            toInstantiate = Instantiate(toInstantiate, floor, Quaternion.identity) as GameObject;
            toInstantiate.transform.SetParent(boardHolder);
        }
        GameObject exitStair = stairTiles[0];
        exitStair = Instantiate(exitStair, exit, Quaternion.identity) as GameObject;
        exitStair.transform.SetParent(boardHolder);

        GameObject entranceStair = stairTiles[1];
        entranceStair = Instantiate(entranceStair, entrance, Quaternion.identity) as GameObject;
        entranceStair.transform.SetParent(boardHolder);
    }

    void AddItems()
    {
        int itemNumber = Random.Range(0,floors.Count / 100);
        if((floors.Count / 100) < 1)
        {
            itemNumber = Random.Range(0, 2);
        }

        List<Vector3> itemSpots = new List<Vector3>();
        itemSpots.Add(exit);
        itemSpots.Add(entrance);

        while (itemNumber > 0)
        {
            Vector3 pos = floors[Random.Range(0, floors.Count)];

            if (!itemSpots.Contains(pos))
            {
                GameObject item = itemTiles[Random.Range(0, itemTiles.Length)];
                item = Instantiate(item, pos, Quaternion.identity) as GameObject;
                itemSpots.Add(pos);
                
            }
            itemNumber--;
        }
    }

    public void SetupScene(int levelNumber)
    {
        floors.Clear();
        walls.Clear();

        borders.Clear();
        connections.Clear();
        
        rooms.Clear();
        
        allPositions.Clear();

        

        BuildRooms(levelNumber);
        AddStairs();
        AddItems();


        levels.Add(new Level(levelNumber));



        DisplayScene();
        

    }
}