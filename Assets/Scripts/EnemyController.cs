using UnityEngine;
using System.Collections;

public class EnemyController : MovingObject {

   private int enemyStrength;
   private int enemyHealth; 

   protected override void Start()
   {

       GameManager.instance.AddEnemyToList(this);

       enemyHealth = GameManager.instance.enemyBaseHealth + Random.Range(-5, 6);
       enemyStrength = GameManager.instance.enemyBaseStrength + Random.Range(-2, 3);

       base.Start();
   }

   public void MoveEnemy()
   {
       int xDir = Random.Range(-1, 2);
       int yDir = Random.Range(-1, 2);
       while (xDir != 0 && yDir != 0)
       {
           xDir = Random.Range(-1, 2);
           yDir = Random.Range(-1, 2);
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
           Debug.Log("Enemy died!");
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
          Debug.Log("Player was hit for " + attack);
          hitPlayer.LoseHealth(attack);
          
       }
   }
}