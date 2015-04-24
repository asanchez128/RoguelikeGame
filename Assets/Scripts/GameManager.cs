using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 200;
    public int playerHealth = 100;
    public bool playersTurn = true;
    public GameObject PlayerObject;

    public int debugCounter = 0;

    public List<MovingObject> actors;

    public static int level = 1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        PlayerObject = GameObject.FindWithTag("Player");
        actors = new List<MovingObject>();
        InitGame();
        
    }

    void InitGame()
    {
        actors.Clear();
        boardScript.SetupScene(level);
        PlayerObject.transform.position = BoardManager.entrance;
        
    }

    public void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
        if (playersTurn)
            return;
        else
        {
            debugCounter++;
            if (debugCounter > 10)
            {
                debugCounter = 0;
                playersTurn = true;
            }
        }
    }
      
}