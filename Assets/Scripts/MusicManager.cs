using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
   static bool AudioBegin = false;
   public GameObject go;
   private AudioSource audioSource;
	void Start () {
      
	}
	
	void Update () {
      if (!go)
      {
          go = GameObject.Find("GameMusic");
      }
      audioSource = go.GetComponent<AudioSource>();
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
         audioSource.Play(); //Plays the audio.
         DontDestroyOnLoad(gameObject);
         AudioBegin = true;
      }
       
   }
}
