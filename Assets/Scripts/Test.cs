using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    GameObject liveTetro;
    //float fall = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float fallSpeed = FindObjectOfType<Game>().fallSpeed;

        if (Input.GetKeyUp(KeyCode.V)) {
            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
            liveTetro.GetComponent<Tetromino>().MoveZPos();
            Debug.Log("V");
        }

        if (Input.GetKeyUp(KeyCode.B)) {
            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
            liveTetro.GetComponent<Tetromino>().MoveZNeg();
            Debug.Log("B");
        }
        /*
        if (Input.GetKeyUp(KeyCode.V)) {
            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
            liveTetro.GetComponent<Tetromino>().MoveXNeg();
            Debug.Log("V");
        }

        if (Input.GetKeyUp(KeyCode.V)) {
            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
            liveTetro.GetComponent<Tetromino>().MoveXNeg();
            Debug.Log("V");
        }*/
    }
    
}
