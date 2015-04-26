using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerController : MovingObject
{
    public float restartLevelDelay = 1f;

    public int stamina;                           
    public int health;
    public int strength;

    protected override void Start()
    {
        stamina = GameManager.instance.playerCurrentStamina;
        health = GameManager.instance.playerCurrentHealth;
        strength = GameManager.instance.playerStrength;
        
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerCurrentStamina = stamina;
        GameManager.instance.playerCurrentHealth = health;
    }


    private void Update()
    {
        if (!GameManager.instance.playersTurn) 
            return;

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.


        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<EnemyController>(horizontal, vertical);
            GameManager.instance.playersTurn = false;
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        LoseStamina(1);
        if (Move(xDir, yDir, out hit))
        {
            //?
        }

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        if (typeof(T) == typeof(EnemyController))
        {
            EnemyController hitEnemy = component as EnemyController;

            int attack = strength + Random.Range(-2, 3);
            if (attack < 0)
                attack = 0;
            Debug.Log("Hit enemy for " + attack);
            hitEnemy.LoseHealth(attack);
            CheckIfLeveledUp();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            PlayerPrefs.SetInt("Player Health", health);
            enabled = false;
        }
        else if (other.tag == "Food1")
        {
            GainStamina(10);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food2")
        {
            GainStamina(20);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food3")
        {
            GainStamina(30);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food4")
        {
            GainStamina(40);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food5")
        {
            GainStamina(50);
            Destroy(other.gameObject);
        }
        
    }
    private void Restart()
    {
        GameManager.instance.NextLevel();
    }
    public void LoseHealth(int loss)
    {
        health -= loss;
        GameManager.instance.playerCurrentHealth -= loss;
        CheckIfGameOver();
    }

    public void GainHealth(int gain)
    {
        health += gain;
        GameManager.instance.playerCurrentHealth += gain;
        if (health > GameManager.instance.playerMaxHealth)
        {
            health = GameManager.instance.playerMaxHealth;
        }
    }

    public void LoseStamina(int loss)
    {
        stamina -= loss;
        GameManager.instance.playerCurrentStamina -= loss;
        
        CheckIfGameOver();
    }

    public void GainStamina(int gain)
    {
        stamina += gain;
        GameManager.instance.playerCurrentStamina+= gain;
        if (stamina > GameManager.instance.playerMaxStamina)
        {
            stamina = GameManager.instance.playerMaxStamina;
        }
    }

    private void CheckIfLeveledUp()
    {
        if (GameManager.instance.enemiesKilled >= Math.Pow(2,GameManager.instance.playerLevel))
        {
            GameManager.instance.enemiesKilled = 0;
            GameManager.instance.playerLevel++;
            Debug.Log("Player leveled up to level " + GameManager.instance.playerLevel + "!");

            int levelStats = Random.Range(1, 4);
            if (levelStats != 1)
            {
                GainStamina(50);
                Debug.Log("Max Stamina rose to " + GameManager.instance.playerMaxStamina);
            }
            if (levelStats != 2)
            {
                GameManager.instance.playerMaxHealth += 10;
                GainHealth(10);
                Debug.Log("Max Health rose to " + GameManager.instance.playerMaxHealth);
            }
            if (levelStats != 3)
            {
                GameManager.instance.playerStrength += 2;
                strength += 2;
                Debug.Log("Strength rose to " + GameManager.instance.playerStrength);
            }
        }
    }

    private void CheckIfGameOver()
    {
        if (stamina <= 0 || health <= 0)
        {
            if (stamina <= 0)
                Debug.Log("Starved to death!");
            else if (health <= 0)
                Debug.Log("Player was killed!");
            GameManager.instance.GameOver();
        }
    }
}
