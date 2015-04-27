using UnityEngine;
using System.Collections;

public class GameMusic : MonoBehaviour {

   private static GameMusic instance = null;

   public AudioClip NewMusic;
   public static GameMusic Instance
   {
      get { return instance; }
   }
   void Awake() {
     if (instance != null && instance != this) {
         Destroy(this.gameObject);
         return;
     } else {
         instance = this;
     }
     DontDestroyOnLoad(this.gameObject);
   }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   
}
