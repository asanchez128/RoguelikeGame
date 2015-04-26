using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;

    public GameObject[] droppableLoot;

    public Dictionary<int,int> foundPotions;
    
    public int playerLevel = 10;
    public int enemiesKilled = 0;

    public int playerPoints = 0;

    public int playerMaxStamina = 200;
    public int playerCurrentStamina = 200;

    public int playerMaxHealth = 50;
    public int playerCurrentHealth = 50;

    public int playerStrength = 10;

    public int enemyBaseHealth = 30;

    public int enemyBaseStrength = 2;

    [HideInInspector] public bool playersTurn = true;

    public GameObject PlayerObject;
    public float turnDelay = 0.05f; 
    public int debugCounter = 0;

    public List<EnemyController> enemies;
    private bool enemiesMoving;

    public static List<Vector2> occupiedSpots = new List<Vector2>();

    public static int level = 1;
    public static int levelCap = 25;

    //public GUIText healthText;
    //public GUIText staminaText;

   private float staminaTextPositionX = 0.01f;
   private float healthTextPositionX = 0.01f;
   private float healthTextPositionY = 0.85f;
   private float staminaTextPositionY = 0.95f;
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
        foundPotions = new Dictionary<int,int>();
        InitGame();        
    }

    void InitGame()
    {
        if (instance != null)
            instance.enemies.Clear();
        else
        {
           enemies.Clear();
        }

        boardScript.SetupScene(level);
        PlayerObject = GameObject.FindWithTag("Player");
        PlayerObject.transform.position = BoardManager.entrance;
        occupiedSpots.Clear();
        
    }

    public void GameOver()
    {
        if (PlayerObject != null)
        {
            PlayerObject.SetActive(false);
            Destroy(PlayerObject.gameObject);
        }
        enabled = false;
    }

    void Update()
    {
       if (playersTurn || enemiesMoving)
       {
          UpdateHealth();
          UpdateStamina();
          return; 
       }
        else
        {
            StartCoroutine(MoveEnemies());
        }
    }

    public void NextLevel()
    {
        level++;

        enemyBaseHealth += 5;
        enemyBaseStrength += 2;
        
        Application.LoadLevel(Application.loadedLevel);
    }

    public void AddEnemyToList(EnemyController script)
    {
        
        if (Random.Range(1, 4) == 1)
        {
            script.itemDrop = droppableLoot[Random.Range(0, droppableLoot.Length)];
        }
       enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
       enemiesMoving = true;

       yield return new WaitForSeconds(turnDelay);

       for (int i = 0; i < enemies.Count; i++)
       {
          if (enemies[i] != null)
          {
             enemies[i].MoveEnemy();
          }
          else
          {
             enemies.Remove(enemies[i]);
          }
       }
       yield return new WaitForSeconds(turnDelay);
       
       playersTurn = true;

       enemiesMoving = false;
    }

   private void UpdateHealth()
   {
      //healthText.text = "Health: " + instance.playerMaxHealth;
   }

   private void UpdateStamina()
   {
      //staminaText.text = "Stamina: " + instance.playerCurrentStamina;
   }
}