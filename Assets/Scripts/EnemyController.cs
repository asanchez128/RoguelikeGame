using UnityEngine;
using System.Collections;

public class EnemyController : MovingObject {

   public int playerDamage;                            //The amount of food points to subtract from the player when attacking.

   private Animator animator;                          //Variable of type Animator to store a reference to the enemy's Animator component.
   private Transform target;                           //Transform to attempt to move toward each turn.
   private bool skipMove;                              //Boolean to determine whether or not enemy should skip a turn or move this turn.
   //Start overrides the virtual Start function of the base class.
   protected override void Start()
   {
      //Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
      //This allows the GameManager to issue movement commands.
      GameManager.instance.AddEnemyToList(this);

      //Get and store a reference to the attached Animator component.
      animator = GetComponent<Animator>();

      //Find the Player GameObject using it's tag and store a reference to its transform component.
      target = GameObject.FindGameObjectWithTag("Player").transform;

      //Call the start function of our base class MovingObject.
      base.Start();
   }

   public void MoveEnemy()
   {
      int xDir = Random.Range(-1,2);
      int yDir = Random.Range(-1, 2);
      while(xDir != 0 && yDir != 0)
      {
          xDir = Random.Range(-1, 2);
          yDir = Random.Range(-1, 2);
      }

      AttemptMove<PlayerController>(xDir, yDir);
   }

   protected override void OnCantMove<T>(T component)
   {
       if (typeof (T) == typeof (PlayerController))
       {
          PlayerController hitPlayer = component as PlayerController;

          //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
          hitPlayer.LoseHealth(playerDamage);
       }
   }
}