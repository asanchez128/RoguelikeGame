using UnityEngine;
using System.Collections;

public class PlayerController : MovingObject
{
    public float restartLevelDelay = 1f;

    private int food;                           
    private int health;

    protected override void Start()
    {
        food = GameManager.instance.playerFoodPoints;
        health = GameManager.instance.playerHealth;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;

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
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {

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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Food2")
        {
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Food3")
        {
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Food4")
        {
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Food5")
        {
            other.gameObject.SetActive(false);
        }
    }
    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void LoseFood(int loss)
    {
        food -= loss;

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
