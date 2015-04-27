using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
   static bool AudioBegin = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   void Awake () {
      if (!AudioBegin)
      {
         GameObject go = GameObject.Find("GameMusic");
         //Finds the game object called Game Music, if it goes by a different name, change this. 

         go.GetComponent<AudioSource>().Play(); //Plays the audio.
         DontDestroyOnLoad(gameObject);
         AudioBegin = true;
      }
       
   }
}
