using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightController : MonoBehaviour {

    GameObject liveTetro;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            Vector2 touchPosition = device.GetAxis();
            if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1) {
                if (touchPosition.y > 0) {
                    Debug.Log("Press UP");
                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
                    liveTetro.GetComponent<Tetromino>().MoveZPos();
                } else {
                    Debug.Log("Press DOWN");
                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
                    liveTetro.GetComponent<Tetromino>().MoveZNeg();
                }

            } else {

                if (touchPosition.x > 0) {
                    Debug.Log("Press RIGHT");
                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
                    liveTetro.GetComponent<Tetromino>().MoveXPos();
                } else {
                    Debug.Log("Press LEFT");

                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().KeyUpHorizontal();
                    liveTetro.GetComponent<Tetromino>().MoveXNeg();
                }
            }
        }
    }
}