using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Basic_Control : MonoBehaviour {



    public int Rotation_Speed = 5;
    public int Down_Speed = 3;
    public int Move_Speed = 5;
    public int Position_Move = 5; /*HOW BLOCK CAN MOVE*/

    public int Position_Down = 10; /*BLOCK FALL*/

    [HideInInspector] public bool Check_R = false; /*Check not multiple Press*/
    [HideInInspector] public bool Check_P = false; /*Check not multiple Press*/


    Vector3 originalPos; /*reset position*/
    // Use this for initialization
    void Start () {
        originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z); ///FOR RESET POSITION///
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) /*reset position*/
        {
            this.transform.position = originalPos;
        }

        if (!Check_R)/*Check not multiple Press*/
        {
            ///////////////////////ROTATION_START//////////////////////////////////
            if (Input.GetKeyDown(KeyCode.W))
            {
                Check_R = true;/*Check for not multiple press*/
                StartCoroutine(Rotate(Vector3.up, 90, Rotation_Speed * Time.deltaTime));
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Check_R = true;/*Check for not multiple press*/
                StartCoroutine(Rotate(Vector3.down, 90, Rotation_Speed * Time.deltaTime));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Check_R = true;/*Check for not multiple press*/
                StartCoroutine(Rotate(Vector3.left, 90, Rotation_Speed * Time.deltaTime));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Check_R = true;/*Check for not multiple press*/
                StartCoroutine(Rotate(Vector3.right, 90, Rotation_Speed * Time.deltaTime));
            }
            ///////////////////////ROTATION_END////////////////////////////////////
        }


        ///////////////////////MOVING_START//////////////////////////////////

        if (!Check_P)/*Check for not multiple press*/
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Check_P = true;/*Check for not multiple press*/
                MoveObjectTo(this.transform, new Vector3(this.transform.position.x - Position_Move, this.transform.position.y, this.transform.position.z), Move_Speed);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Check_P = true;/*Check for not multiple press*/
                MoveObjectTo(this.transform, new Vector3(this.transform.position.x + Position_Move, this.transform.position.y, this.transform.position.z), Move_Speed);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Check_P = true;/*Check for not multiple press*/
                MoveObjectTo(this.transform, new Vector3(this.transform.position.x, this.transform.position.y - Position_Down, this.transform.position.z), Move_Speed);
            }
        }


        ///////////////////////MOVING_END////////////////////////////////////
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)  ///////ROTATE_FUNCTION////////////
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
        Check_R = false; /*Check for not multiple press*/
    }

    private void MoveObjectTo(Transform objectToMove, Vector3 targetPosition, float moveSpeed) //////MOVE_FUCNTION/////
    {
        StopCoroutine(MoveObject(objectToMove, targetPosition, moveSpeed));
        StartCoroutine(MoveObject(objectToMove, targetPosition, moveSpeed));
    }

    IEnumerator MoveObject(Transform objectToMove, Vector3 targetPosition, float moveSpeed) //////MOVE_FUCNTION/////
    {
        float currentProgress = 0;
        Vector3 cashedObjectPosition = objectToMove.transform.position;

        while (currentProgress <= 1)
        {
            currentProgress += moveSpeed * Time.deltaTime;

            objectToMove.position = Vector3.Lerp(cashedObjectPosition, targetPosition, currentProgress);

            yield return null;
        }
        Check_P = false;/*Check for not multiple press*/
    }
}
