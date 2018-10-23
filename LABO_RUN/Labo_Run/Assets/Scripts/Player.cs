using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int PlayerSpeed = 0;
    public int AddSpeed = 1;
    public int MaxSpeed = 50

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerSpeed += AddSpeed;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerSpeed += AddSpeed;
        }

        if (PlayerSpeed > MaxSpeed)
        {
            PlayerSpeed = MaxSpeed;
        }

        transform.position += new Vector3(0f, 0f, PlayerSpeed * Time.deltaTime);
    }
}
