using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

   private int food;
   private Vector2 pos;
   private bool moving = false;
  
	// Use this for initialization
	void Start () {
      food = GameManager.instance.playerFoodPoints;
      pos = transform.position;
      Camera.main.orthographicSize = Screen.height / 2 * 1 / 32;

      
	}

   private void OnDisable()
   {
      //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
      GameManager.instance.playerFoodPoints = food;
   }
   void Update()
   {

      CheckInput();

      if (moving)
      {
         // pos is changed when there's input from the player
         transform.position = pos;
         moving = false;
      }
   }
   private void CheckInput()
   {

      // WASD control
      // We add the direction to our position,
      if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
      // this moves the character 1 unit (32 pixels)
      {
         pos += Vector2.right;
         moving = true;
         //ExecuteNPCAI();
      }

         // For left, we have to subtract the direction
      else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
      {
         pos -= Vector2.right;
         moving = true;
         // ExecuteNPCAI();
      }
      else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
      {
         pos += Vector2.up;
         moving = true;
        // ExecuteNPCAI();
      }

         // Same as for the left, subtraction for down
      else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
      {
         pos -= Vector2.up;
         moving = true;
         // ExecuteNPCAI();
      }
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
