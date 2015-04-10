using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

//With inspiration from http://unity3d.com/learn/tutorials/projects/2d-roguelike/boardmanager

public class BoardManager : MonoBehaviour
{
    const int MAP_EDGE = 50;
    
    private Transform boardHolder;
    
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    //public GameObject[] itemTiles;
    //public GameObject[] enemyTiles;

    private List <Vector3> gridPositions = new List <Vector3> ();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 0; x < 13; x++) //test values for debug room
        {
            for (int y = 0; y < 13; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform
        boardHolder = new GameObject("Board").transform;


        for (int x = 0; x < 13; x++)
        {
            for (int y = 0; y < 13; y++)
            {

                GameObject toInstantiate = floorTiles[0]; //todo: tile selection process

                if (x == 0 || x == 12 || y == 0 || y == 12)
                    toInstantiate = wallTiles[0];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of the new instance to boardHolder
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public void SetupScene (int level)
    {
        BoardSetup ();
            
        InitialiseList ();
    }
}
