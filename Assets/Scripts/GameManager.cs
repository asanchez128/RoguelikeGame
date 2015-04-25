using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;
    public static int playerFoodPoints = 1000;
    public static int playerHealth = 100;
    public bool playersTurn = true;
    public GameObject PlayerObject;
    public float turnDelay = 0.1f; 
    public int debugCounter = 0;

    public List<EnemyController> enemies;
    private bool enemiesMoving;                             
    

    public static int level = 1;
    public static int levelCap = 25;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        PlayerObject = GameObject.FindWithTag("Player");
        enemies = new List<EnemyController>();
        InitGame();
        
    }

    void InitGame()
    {
<<<<<<< HEAD
        actors.Clear();
        if (instance != null)
        instance.enemies.Clear();
        else
        {
           enemies.Clear();
        }
        boardScript.SetupScene(level);
        PlayerObject.transform.position = BoardManager.entrance;
=======
        boardScript.SetupScene(level);
        PlayerObject.transform.position = BoardManager.entrance;
        enemies.Clear();

>>>>>>> d18c2c76ee0d63d6e75ffe344def4ba784560755
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
            StartCoroutine(MoveEnemies());
        }
    }

    //Call this to add the passed in Enemy to the List of Enemy objects.
    public void AddEnemyToList(EnemyController script)
    {
       //Add Enemy to List enemies.
       enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
       //While enemiesMoving is true player is unable to move.
       enemiesMoving = true;

       //Wait for turnDelay seconds, defaults to .1 (100 ms).
       yield return new WaitForSeconds(turnDelay);

       //If there are no enemies spawned (IE in first level):
       if (enemies.Count == 0)
       {
          //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
          yield return new WaitForSeconds(turnDelay);
       }

       //Loop through List of Enemy objects.
       for (int i = 0; i < enemies.Count; i++)
       {
          //Call the MoveEnemy function of Enemy at index i in the enemies List.
          enemies[i].MoveEnemy();

          //Wait for Enemy's moveTime before moving next Enemy, 
          yield return new WaitForSeconds(enemies[i].moveTime);
       }
       //Once Enemies are done moving, set playersTurn to true so player can move.
       playersTurn = true;

       //Enemies are done moving, set enemiesMoving to false.
       enemiesMoving = false;
    }
}