using UnityEngine;
using System.Collections;

public class EnemyController : MovingObject {

   private int enemyStrength;
   private int enemyHealth;

   private Animator animator;  
   private Transform target;  
   private bool skipMove;    

   protected override void Start()
   {

      GameManager.instance.AddEnemyToList(this);

      target = GameObject.FindGameObjectWithTag("Player").transform;

      if (GameManager.level <= 5)
          enemyStrength = Random.Range(1, GameManager.level);
      else
          enemyStrength = Random.Range(GameManager.level - 5, GameManager.level + 6);

      if (GameManager.level <= 5)
          enemyHealth = Random.Range(5, GameManager.level+6);
      else
          enemyHealth = Random.Range(GameManager.level, GameManager.level + 6);
      

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
          hitPlayer.LoseHealth(attack);
       }
   }
}