using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;

    public GameObject[] droppableLoot;

    public Dictionary<int,int> foundPotions;
    
    public int playerLevel;
    public int enemiesKilled;

    public int playerPoints;

    public int playerMaxStamina;
    public int? playerCurrentStamina;

    public int playerMaxHealth;
    public int? playerCurrentHealth;

    public int playerStrength;

    public int enemyBaseHealth;

    public int enemyBaseStrength;

    [HideInInspector] public bool playersTurn = true;

    public GameObject PlayerObject;
    public float turnDelay = 0.05f; 

    public List<EnemyController> enemies;
    private bool enemiesMoving;

    public static List<Vector2> occupiedSpots = new List<Vector2>();

    public static int level = 1;
    public static int levelCap = 25;

    public GameObject healthObject;
    public GameObject staminaObject;

   private float staminaTextPositionX = 0.01f;
   private float healthTextPositionX = 0.01f;
   private float healthTextPositionY = 0.85f;
   private float staminaTextPositionY = 0.95f;

   public bool waitingForInput = false;

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
        healthObject = new GameObject();
        staminaObject = new GameObject();
        healthObject.AddComponent<GUIText>();
        healthObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.1f, 0.0f);
        staminaObject.AddComponent<GUIText>();
        staminaObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.9f, 0.0f);

        instance.playerCurrentHealth = 100;  
        instance.playerCurrentStamina = 200;
        instance.playerMaxHealth = 100;
        instance.playerMaxStamina = 200;
        playerLevel = 1;
        enemiesKilled = 0;
        playerPoints = 0;
        playerStrength = 10;
        enemyBaseHealth = 30;
        enemyBaseStrength = 2;

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
        }
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.R))
       {
           waitingForInput = true;
           playersTurn = false;
           Debug.Log("Restart the game?  (y/n)");
           StartCoroutine(WaitForKeyPress());
       }
        if (!waitingForInput)
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
    }

    public IEnumerator WaitForKeyPress()
    {
        while (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
        {
            yield return null;
        }
        waitingForInput = false;
        if (Input.inputString[0] == 'y' || Input.inputString[0] == 'Y')
        {
            Restart();
        }

    }

    public void Restart()
    {
        instance.playerCurrentHealth = 100;
        instance.playerCurrentStamina = 200;
        instance.playerMaxHealth = 100;
        instance.playerMaxStamina = 200;
        playerLevel = 10;
        enemiesKilled = 0;
        playerPoints = 0;
        playerStrength = 10;
        enemyBaseHealth = 30;
        enemyBaseStrength = 2;
        PlayerObject = GameObject.FindWithTag("Player");
        enemies.Clear();
        foundPotions.Clear();
        level = 1;

        Application.LoadLevel(Application.loadedLevel);
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
      if (healthObject != null)
         healthObject.GetComponent<GUIText>().text = "Health: " + instance.playerCurrentHealth;
      else
      {
         healthObject = new GameObject();
        healthObject.AddComponent<GUIText>();
        healthObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.1f, 0.0f);
      }
   }

   private void UpdateStamina()
   {
      if (staminaObject != null)
         staminaObject.GetComponent<GUIText>().text = "Stamina: " + instance.playerCurrentStamina;
      else
      {
         staminaObject = new GameObject();
        staminaObject.AddComponent<GUIText>();
        staminaObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.9f, 0.0f);
      }
   }
}
