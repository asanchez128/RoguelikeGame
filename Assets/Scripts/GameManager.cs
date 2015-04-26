using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;

    public GameObject[] droppableLoot;

    public Dictionary<int,int> foundPotions;
    
    public int playerCurrentHealth = 100;  
    public int playerCurrentStamina = 200;
    public int playerMaxHealth = 100;
    public int playerMaxStamina = 200;
    public int playerLevel = 1;
    public int enemiesKilled = 0;
    public int playerPoints = 0;
    public int playerStrength = 5;
    public int enemyBaseHealth = 10;
    public int enemyBaseStrength = 2;

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
        UpdateHealth(0);
        UpdateStamina(0);
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
        instance.playerCurrentHealth = 50;
        instance.playerCurrentStamina = 200;
        instance.playerMaxHealth = 50;
        instance.playerMaxStamina = 200;
        playerLevel = 1;
        enemiesKilled = 0;
        playerPoints = 0;
        playerStrength = 5;
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
        UpdateHealth(playerCurrentHealth);
        UpdateStamina(playerCurrentStamina);
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
       occupiedSpots.Clear();

       enemiesMoving = false;
    }

   public void UpdateHealth(int currentHealth)
   {
       if (healthObject != null)
           healthObject.GetComponent<GUIText>().text = "Health: " + currentHealth;
       else
       {
           healthObject = new GameObject();
           healthObject.AddComponent<GUIText>();
           healthObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.1f, 0.0f);
       }
   }

   public void UpdateStamina(int currentStamina)
   {
       if (staminaObject != null)
           staminaObject.GetComponent<GUIText>().text = "Stamina: " + currentStamina;
       else
       {
           staminaObject = new GameObject();
           staminaObject.AddComponent<GUIText>();
           staminaObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.9f, 0.0f);
       }
   }
}
