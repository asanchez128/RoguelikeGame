using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
   public GameObject target;  // The player
   private Vector3 offset;

	// Use this for initialization
	void Start () {
      offset = new Vector3(0f, 0f, -10f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   void LateUpdate()
   {
      // Change camera's position to the same as the player (with the z-value of -10)
      transform.position = target.transform.position + offset;
   }
}
