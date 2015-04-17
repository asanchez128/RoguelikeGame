using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   Vector3 movement;                   // The vector to store the direction of the player's movement.
   Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
  

   private float speed = 1.0f;

   void Update()
   {
      var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
      transform.position += move*speed*Time.deltaTime;
   }
}
        