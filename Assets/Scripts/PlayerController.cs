using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MovingObject {

   private int food;     
	// Use this for initialization
	void Start () {
      food = GameManager.instance.playerFoodPoints;
      base.Start();
	}

   private void OnDisable()
   {
      //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
      GameManager.instance.playerFoodPoints = food;
   }
	
	// Update is called once per frame
	void Update () {
      if (!GameManager.instance.playersTurn) return;

      int horizontal = 0;     //Used to store the horizontal move direction.
      int vertical = 0;       //Used to store the vertical move direction.


      //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
      horizontal = (int)(Input.GetAxisRaw("Horizontal"));

      //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
      vertical = (int)(Input.GetAxisRaw("Vertical"));

      //Check if moving horizontally, if so set vertical to zero.
      if (horizontal != 0)
      {
         vertical = 0;
      }

      //Check if we have a non-zero value for horizontal or vertical
      if (horizontal != 0 || vertical != 0)
      {
         //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
         //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
         AttemptMove<Wall>(horizontal, vertical);
      }
	}

   protected override void AttemptMove<T>(int xDir, int yDir)
   {
      //Every time player moves, subtract from food points total.
      food--;

      //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
      base.AttemptMove<T>(xDir, yDir);

      //Hit allows us to reference the result of the Linecast done in Move.
      RaycastHit2D hit;

      //If Move returns true, meaning Player was able to move into an empty space.
      if (Move(xDir, yDir, out hit))
      {
         //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
      }

      //Since the player has moved and lost food points, check if the game has ended.
      CheckIfGameOver();

      //Set the playersTurn boolean of GameManager to false now that players turn is over.
      GameManager.instance.playersTurn = false;
   }

   protected override void OnCantMove<T>(T component)
   {
      //Set hitWall to equal the component passed in as a parameter.
      Wall hitWall = component as Wall;

   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      //Check if the tag of the trigger collided with is Exit.
      if (other.tag == "Exit")
      {
         //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
         //Invoke("Restart", restartLevelDelay);

         //Disable the player object since level is over.
         enabled = false;
      }

      //Check if the tag of the trigger collided with is Food.
      else if (other.tag == "Food")
      {
         //Add pointsPerFood to the players current food total.
         //food += pointsPerFood;

         //Disable the food object the player collided with.
         other.gameObject.SetActive(false);
      }

      //Check if the tag of the trigger collided with is Soda.
      else if (other.tag == "Soda")
      {
         //Add pointsPerSoda to players food points total
         //food += pointsPerSoda;


         //Disable the soda object the player collided with.
         other.gameObject.SetActive(false);
      }
   }


   //Restart reloads the scene when called.
   private void Restart()
   {
      //Load the last scene loaded, in this case Main, the only scene in the game.
      Application.LoadLevel(Application.loadedLevel);
   }


   //LoseFood is called when an enemy attacks the player.
   //It takes a parameter loss which specifies how many points to lose.
   public void LoseFood(int loss)
   {
    
      //Subtract lost food points from the players total.
      food -= loss;

      //Check to see if game has ended.
      CheckIfGameOver();
   }


   //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
   private void CheckIfGameOver()
   {
      //Check if food point total is less than or equal to zero.
      if (food <= 0)
      {

         //Call the GameOver function of GameManager.
         GameManager.instance.GameOver();
      }
   }
}
