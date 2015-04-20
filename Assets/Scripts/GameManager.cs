using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

    public BoardManager boardScript;

<<<<<<< HEAD
    private int level = 15;
=======
    private int level = 1;
>>>>>>> parent of 26cd5e3... Map generation code

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
