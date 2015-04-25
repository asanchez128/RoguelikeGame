using UnityEngine;
using System.Collections;

public class PlayerController : MovingObject
{
    public float restartLevelDelay = 1f;

    private int food;                           
    private int health;
    private int strength;
    private Animation animation;


    protected override void Start()
    {
        food = GameManager.playerFoodPoints;
        health = GameManager.playerHealth;
        strength = GameManager.playerStrength;
        
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.playerFoodPoints = food;
        GameManager.playerHealth = health;
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

        if (Move(xDir, yDir, out hit))
        {
            food--;
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        if (typeof(T) == typeof(EnemyController))
        {
            EnemyController hitEnemy = component as EnemyController;
            hitEnemy.GetComponent<Animation>().Play();
            int attack = strength + Random.Range(-2, 3);
            if (attack < 0)
                attack = 0;
            hitEnemy.LoseHealth(attack);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food1")
        {
            food += 10;
            if (food > 200)
                food = 200;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food2")
        {
            food += 20;
            if (food > 200)
                food = 200;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food3")
        {
            food += 30;
            if (food > 200)
                food = 200;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food4")
        {
            food += 40;
            if (food > 200)
                food = 200;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Food5")
        {
            food += 50;
            if (food > 200)
                food = 200;
            Destroy(other.gameObject);
        }
        
    }
    private void Restart()
    {
        GameManager.level++;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void LoseHealth(int loss)
    {
        health -= loss;

        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0 || health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
