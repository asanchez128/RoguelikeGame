using UnityEngine;
using System.Collections;

public class EnemyController : MovingObject {

   private int enemyStrength;
   private int enemyHealth;

   private LayerMask walls;

   private int enemyPoints;

   public GameObject itemDrop;

   protected override void Start()
   {

       GameManager.instance.AddEnemyToList(this);

       int randHealth = Random.Range(-5, 6);
       int randStrength = Random.Range(-2, 3);

       enemyHealth = GameManager.instance.enemyBaseHealth + randHealth;
       enemyStrength = GameManager.instance.enemyBaseStrength + randStrength;

       enemyPoints = enemyHealth + enemyStrength;

       base.Start();
   }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        Vector2 pos = gameObject.transform.position;
        Vector2 target = GameObject.FindWithTag("Player").transform.position;

        RaycastHit2D hit = Physics2D.Linecast(pos, target, walls);

        if (hit.transform == null && Random.Range(1,3) == 1)
        {
            //can see player
            if (pos.x < target.x)
                xDir = 1;
            else if (pos.x > target.x)
                xDir = -1;

            if (pos.y < target.y)
                yDir = 1;
            else if (pos.y > target.y)
                yDir = -1;

            if (xDir != 0 && yDir != 0)
            {
                if (Random.Range(1, 3) == 1)
                    xDir = 0;
                else
                    yDir = 0;
            }
        }
        else
        {//wander aimlessly
            xDir = Random.Range(-1, 2);
            yDir = Random.Range(-1, 2);
            while (xDir != 0 && yDir != 0)
            {
                xDir = Random.Range(-1, 2);
                yDir = Random.Range(-1, 2);
            }
        }
       base.AttemptMove<PlayerController>(xDir, yDir);
   }

   public void LoseHealth(int loss)
   {
       enemyHealth -= loss;

       CheckIfDead();
   }

   void CheckIfDead()
   {
       if (enemyHealth <= 0)
       {
           GameManager.instance.enemiesKilled++;
           GameManager.instance.playerPoints += enemyPoints;
           Debug.Log("The "+ gameObject.tag + " has been slain.");

           if (itemDrop != null)
           {
               Vector3 pos = new Vector3(gameObject.transform.position.x + Random.Range(-1, 2),
                                         gameObject.transform.position.y + Random.Range(-1, 2), 0f);

               Instantiate(itemDrop, pos, Quaternion.identity);
           }
           if (GameManager.occupiedSpots.Contains(gameObject.transform.position))
               GameManager.occupiedSpots.Remove(gameObject.transform.position);
           Destroy(gameObject);
       }
   }

   protected override void OnCantMove<T>(T component)
   {
       if (typeof (T) == typeof (PlayerController))
       {
          PlayerController hitPlayer = component as PlayerController;
          int attack = enemyStrength + Random.Range(-2, 3);
          if (attack < 0)
              attack = 0;
          Debug.Log("The "+ gameObject.tag +" attacks you!  (-" + attack + " hp)");
          hitPlayer.LoseHealth(attack);     
       }
   }
}