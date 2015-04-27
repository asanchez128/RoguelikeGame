using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
   static bool AudioBegin = false;
   static bool gamePaused = false;
   private GameObject go;
   private AudioSource audioSource;
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
      if (Input.GetKeyDown(KeyCode.P))
      {
         if (!audioSource.isPlaying)
         {
            audioSource.Play();
         }
         else
         {
            audioSource.Pause();
         }
      }
	}

   void Awake () {
      if (!AudioBegin)
      {
         go = GameObject.Find("GameMusic");
         audioSource = go.GetComponent<AudioSource>();
         //Finds the game object called Game Music, if it goes by a different name, change this. 
         audioSource.Play(); //Plays the audio.
         DontDestroyOnLoad(gameObject);
         AudioBegin = true;
      }
       
   }
}
