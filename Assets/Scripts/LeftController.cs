using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftController : MonoBehaviour {

    GameObject liveTetro;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update(){ 
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("Press DOWN && triger down");

            liveTetro = FindObjectOfType<Game>().liveTetromino;
            liveTetro.GetComponent<Tetromino>().SlamDown();
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchPosition = device.GetAxis();
            if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x< -1)
            {
                if (touchPosition.y > 0){
                    Debug.Log("Press UP");
                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().RotateY();
                }else{
                    Debug.Log("Press DOWN");
                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                    liveTetro.GetComponent<Tetromino>().MoveDown();
                }

            }else{

                if (touchPosition.x > 0)
                {                    
                    Debug.Log("Press RIGHT");

                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().RotateZ();
                } else{                    
                    Debug.Log("Press LEFT");

                    liveTetro = FindObjectOfType<Game>().liveTetromino;

                    liveTetro.GetComponent<Tetromino>().RotateX();
                }
            }
        }
    }
}
