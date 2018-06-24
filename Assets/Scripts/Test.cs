using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    GameObject liveTetro;
    public float fall = 0;
    public float fallSpeed = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.Backspace) ) {
            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpVertical();
            liveTetro.GetComponent<Tetromino>().MoveDown();
            Debug.Log("Zキーを押した");
        }
    }
}
