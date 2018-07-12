using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftController : MonoBehaviour
{

    GameObject liveTetro;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        Quaternion direction = InputTracking.GetLocalRotation(XRNode.Head);


        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("triger down");
            device.TriggerHapticPulse(2000);

            liveTetro = FindObjectOfType<Game>().liveTetromino;
            liveTetro.GetComponent<Tetromino>().SlamDown();
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log("grip down");
            device.TriggerHapticPulse(2000);

            liveTetro = FindObjectOfType<Game>().liveTetromino;

            liveTetro.GetComponent<Tetromino>().KeyUpVertical();
            liveTetro.GetComponent<Tetromino>().MoveDown();
        }

        // 前向き
        if (-0.25f <= direction[1] && direction[1] < 0.25f)
        {

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {

                device.TriggerHapticPulse(2000);

                Vector2 touchPosition = device.GetAxis();
                if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                {
                    if (touchPosition.y > 0)
                    {
                        Debug.Log("Press UP");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateXPos();
                    }
                    else
                    {
                        Debug.Log("Press DOWN");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                        liveTetro.GetComponent<Tetromino>().RotateXNeg();
                    }

                }
                else
                {

                    if (touchPosition.x > 0)
                    {
                        Debug.Log("Press RIGHT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYPos();
                    }
                    else
                    {
                        Debug.Log("Press LEFT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYNeg();
                    }
                }
            }
            // 後ろ向き
        }
        else if (direction[1] <= -0.75f || 0.75f <= direction[1])
        {

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {

                device.TriggerHapticPulse(2000);

                Vector2 touchPosition = device.GetAxis();
                if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                {
                    if (touchPosition.y > 0)
                    {
                        Debug.Log("Press UP");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateXNeg();
                    }
                    else
                    {
                        Debug.Log("Press DOWN");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                        liveTetro.GetComponent<Tetromino>().RotateXPos();
                    }

                }
                else
                {

                    if (touchPosition.x > 0)
                    {
                        Debug.Log("Press RIGHT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYNeg();
                    }
                    else
                    {
                        Debug.Log("Press LEFT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYPos();
                    }
                }
            }
            // 左向き
        }
        else if (0.25f <= direction[1] && direction[1] < 0.75f)
        {
            if (direction[3] < 0)
            {

                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {

                    device.TriggerHapticPulse(2000);

                    Vector2 touchPosition = device.GetAxis();
                    if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                    {
                        if (touchPosition.y > 0)
                        {
                            Debug.Log("Press UP");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateZNeg();
                        }
                        else
                        {
                            Debug.Log("Press DOWN");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                            liveTetro.GetComponent<Tetromino>().RotateZPos();
                        }

                    }
                    else
                    {

                        if (touchPosition.x > 0)
                        {
                            Debug.Log("Press RIGHT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYPos();
                        }
                        else
                        {
                            Debug.Log("Press LEFT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYNeg();
                        }
                    }
                }
            }
            else
            {
                Debug.Log("right");

                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {

                    device.TriggerHapticPulse(2000);

                    Vector2 touchPosition = device.GetAxis();
                    if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                    {
                        if (touchPosition.y > 0)
                        {
                            Debug.Log("Press UP");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateZPos();
                        }
                        else
                        {
                            Debug.Log("Press DOWN");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                            liveTetro.GetComponent<Tetromino>().RotateZNeg();
                        }

                    }
                    else
                    {

                        if (touchPosition.x > 0)
                        {
                            Debug.Log("Press RIGHT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYPos();
                        }
                        else
                        {
                            Debug.Log("Press LEFT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYNeg();
                        }
                    }
                }
            }
            // 右向き
        }
        else if (-0.75f <= direction[1] && direction[1] < -0.25f)
        {
            if (direction[3] < 0)
            {
                Debug.Log("right");

                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {

                    device.TriggerHapticPulse(2000);

                    Vector2 touchPosition = device.GetAxis();
                    if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                    {
                        if (touchPosition.y > 0)
                        {
                            Debug.Log("Press UP");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateZPos();
                        }
                        else
                        {
                            Debug.Log("Press DOWN");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                            liveTetro.GetComponent<Tetromino>().RotateZNeg();
                        }

                    }
                    else
                    {

                        if (touchPosition.x > 0)
                        {
                            Debug.Log("Press RIGHT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYPos();
                        }
                        else
                        {
                            Debug.Log("Press LEFT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYNeg();
                        }
                    }
                }
            }
            else
            {
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {

                    device.TriggerHapticPulse(2000);

                    Vector2 touchPosition = device.GetAxis();
                    if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
                    {
                        if (touchPosition.y > 0)
                        {
                            Debug.Log("Press UP");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateZNeg();
                        }
                        else
                        {
                            Debug.Log("Press DOWN");
                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                            liveTetro.GetComponent<Tetromino>().RotateZPos();
                        }
                    }
                    else
                    {

                        if (touchPosition.x > 0)
                        {
                            Debug.Log("Press RIGHT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYPos();
                        }
                        else
                        {
                            Debug.Log("Press LEFT");

                            liveTetro = FindObjectOfType<Game>().liveTetromino;

                            liveTetro.GetComponent<Tetromino>().RotateYNeg();
                        }
                    }
                }
            }

            /*
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {

                device.TriggerHapticPulse(2000);

                Vector2 touchPosition = device.GetAxis();
                if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x< -1)
                {
                    if (touchPosition.y > 0){
                        Debug.Log("Press UP");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateXPos();
                    }
                    else
                    {
                        Debug.Log("Press DOWN");
                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        //liveTetro.GetComponent<Tetromino>().KeyUpVertical();
                        liveTetro.GetComponent<Tetromino>().RotateXNeg();
                    }

                }else{

                    if (touchPosition.x > 0)
                    {                    
                        Debug.Log("Press RIGHT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYPos();
                    }
                    else
                    {                    
                        Debug.Log("Press LEFT");

                        liveTetro = FindObjectOfType<Game>().liveTetromino;

                        liveTetro.GetComponent<Tetromino>().RotateYNeg();
                    }
                }
            }*/
        }
    }
}
