﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 300;
    public int playerHealth = 100;
    public bool playersTurn = true; 
    [HideInInspector]

    private int level = 15;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
       //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
       if (playersTurn)

          //If any of these are true, return and do not start MoveEnemies.
          return;

    }
      
}