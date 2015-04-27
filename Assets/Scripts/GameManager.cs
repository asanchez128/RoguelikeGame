using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
    
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;

    public static PlayerLog playerLog;

    private Text FloorNumberText;
    private Text PlayerLevelText;
    private Text HealthText;
    private Text StaminaText;
    private Text ScoreText;

    public GameObject[] droppableLoot;

    public Dictionary<int,int> foundPotions;
    
    public int? playerCurrentHealth;  
    public int? playerCurrentStamina;
    public int? playerMaxHealth;
    public int? playerMaxStamina;
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

    public bool waitingForInput = false;
    public bool dead = false;
    private GameObject healthObject;
    private GameObject staminaObject;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        playerLog = GetComponent<PlayerLog>();
        DontDestroyOnLoad(playerLog);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        PlayerObject = GameObject.FindWithTag("Player");

        enemies = new List<EnemyController>();
        foundPotions = new Dictionary<int,int>();

        instance.healthObject = new GameObject();
        instance.staminaObject = new GameObject();
        
        instance.healthObject.AddComponent<GUIText>();
        instance.healthObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.1f, 0.0f);
        instance.staminaObject.AddComponent<GUIText>();
        instance.staminaObject.GetComponent<Transform>().position = new Vector3(0.1f, 0.9f, 0.0f);
        if (!instance.playerCurrentHealth.HasValue)
        instance.playerCurrentHealth = 100;
        
        if (!instance.playerCurrentStamina.HasValue)
        instance.playerCurrentStamina = 200;

        if (!instance.playerMaxHealth.HasValue)
           instance.playerMaxHealth = 100;
        
       if (!instance.playerMaxStamina.HasValue)
       instance.playerMaxStamina = 200;

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

        FloorNumberText = GameObject.Find("FloorNumberText").GetComponent<Text>();
        FloorNumberText.text = "Floor " + level;

        PlayerLevelText = GameObject.Find("PlayerLevelText").GetComponent<Text>();
        PlayerLevelText.text = "Level " + playerLevel;

        //HealthText = GameObject.Find("HealthText").GetComponent<Text>();
        //HealthText.text = "Health:  " + playerCurrentHealth;
        instance.healthObject.GetComponent<GUIText>().text = "Health:  " + playerCurrentHealth;
        //StaminaText = GameObject.Find("StaminaText").GetComponent<Text>();
        //StaminaText.text = "Stamina:  " + playerCurrentStamina;
        instance.staminaObject.GetComponent<GUIText>().text = "Stamina:  " + playerCurrentStamina;

        
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = "Score:  " + playerPoints;

        boardScript.SetupScene(level);
        PlayerObject = GameObject.FindWithTag("Player");
        PlayerObject.transform.position = BoardManager.entrance;

        DontDestroyOnLoad(FloorNumberText);
        DontDestroyOnLoad(PlayerLevelText);
        //DontDestroyOnLoad(HealthText);
        //DontDestroyOnLoad(StaminaText);
        DontDestroyOnLoad(ScoreText);

        occupiedSpots.Clear();     
    }

    public void GameOver()
    {
        dead = true;
        if (PlayerObject != null)
        {
            PlayerObject.SetActive(false);
        }
        UpdateHealth();
        UpdateStamina();

        waitingForInput = true;
        playersTurn = true;
        playerLog.NewMessage("Restart the game?  (y/n)");
        StartCoroutine(WaitForKeyPress());
    }

    void Update()
    {
        if (!playerLog)
        {
            playerLog = GetComponent<PlayerLog>();
        }
       if (!dead && Input.GetKeyDown(KeyCode.R))
       {
           waitingForInput = true;
           playersTurn = true;
           playerLog.NewMessage("Restart the game?  (y/n)");
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
            dead = false;
            Restart();
        }
        else
        {
            Application.Quit();
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
        UpdateHealth();
        UpdateStamina();
        UpdateFloorNumber();
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

    public void UpdateHealth()
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

    public void UpdateStamina()
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
    public void UpdatePlayerLevel()
   {
        if (!PlayerLevelText)
            PlayerLevelText = GameObject.Find("PlayerLevelText").GetComponent<Text>();
        PlayerLevelText.text = "Level " + playerLevel;
   }

    public void UpdatePlayerScore()
    {
        if (!ScoreText)
            ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = "Score:  " + playerPoints;
    }

    public void UpdateFloorNumber()
    {
        if (!FloorNumberText)
            FloorNumberText = GameObject.Find("FloorNumberText").GetComponent<Text>();
        FloorNumberText.text = "Floor " + level;
    }
}
