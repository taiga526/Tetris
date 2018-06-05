﻿using UnityEngine;
using System.Collections;

// this script is attached to each of the active falling tetrominoes
// which you can find in the prefabs folder.
//
// completely object oriented, it organizes itself and checks
// for collisions in a bit-array (sorry unity, no collider)
//
// unity is only used for visual representation

public class Block : MonoBehaviour {

	public string[] BlockStructure;
	public GameObject cubus;
	public Material blockMaterial;
	public AudioClip swoosh;
	
	private GameObject block;
	private bool [,] blockMatrix;
	private int size;
	private int halfSize;
	private float halfSizeFloat;
	
	private int yPosition;
	private int xPosition;

	public float fallingInterval = 1f;
	float movingInterval = 0.1f;
	float elapsedTimeFalling = 0f;
	float elapsedTimeMoving = 0f;

	private bool isFrozen;
	private bool dropped;


	// here, Awake instead of Start has to be  used
	// after instantiation from the Gamemanager class,
	// method freeze may be called
	// freeze uses parameters which must have been 
	// initialized so far 
	// used for the "next block" functionality

	// Use this for initialization
	void Awake () {
		// get size of Block
		size = BlockStructure.Length;

		// to do: 
		// a lot of error checking

		// needed for correct positioning
		halfSize = size / 2;
		halfSizeFloat = size * 0.5f;

		// generate bitField for Collisions
		// this time, unity is used only for visual representation
		blockMatrix = new bool[size, size];

		// Instantiate Block from the Blockstructure
		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size; x++) {
				if (BlockStructure[y][x].ToString() == "1")
				{
					blockMatrix[x,y] = true;
					// if Block Bit is set, Instantiate Block 
					block  = Instantiate(cubus,new Vector3(x-halfSizeFloat,halfSizeFloat-y,0), Quaternion.identity) as GameObject;
					// give color to the block
					block.transform.GetComponentInChildren<MeshRenderer>().GetComponent<Renderer>().material = blockMaterial;
					// bind block to parent for rotation and movement 
					block.transform.parent = this.transform;
				}
			}
		}
		Vector3 position = transform.position;
		position.x = Gamemanager.thisOne.getFieldWidth () / 2 + (size % 2 == 0 ? 0.0f : 0.5f);
		xPosition = (int)(position.x - halfSizeFloat);
		yPosition = Gamemanager.thisOne.getFieldHeight () - 1;
		position.y = yPosition - halfSizeFloat; 
		transform.position = position;

		// don't freeze block
		isFrozen = false;
		// block hasn't been dropped
		dropped = false;

		// we just spawned a new Tetromino
		// have we reached the top? Is the game already over?
		if (Gamemanager.thisOne.checkBlock (blockMatrix, size, xPosition, yPosition)) {
			// this game is over
			Gamemanager.thisOne.gameOver();
			// and out
			return;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isFrozen){
			elapsedTimeFalling += Time.deltaTime;
			elapsedTimeMoving += Time.deltaTime;
			if (elapsedTimeFalling >= fallingInterval) {
				elapsedTimeFalling -= fallingInterval;
				fall ();
			}
			if (elapsedTimeMoving >= movingInterval) {
				checkInput ();
			}
		}
	}

	public void freeze(){
		isFrozen = true;
		Vector3 position = transform.position;
		position.x = -8.5f;
		position.y = 15.25f;
		transform.position = position;
	}

	// let the block fall
	void fall() {
			// let block fall (virtually)
			yPosition--;
			// is there a collision at the new position?
			if (Gamemanager.thisOne.checkBlock(blockMatrix,size, xPosition, yPosition)) {
				// Then set the block at actual position...
				Gamemanager.thisOne.setBlock(blockMatrix,size, xPosition, yPosition+1,dropped);
				// and destroy the gameObject
				Destroy(gameObject);
				// I almost forgot: leave the loop
				return;
			}

			// move the block physically
			Vector3 position = transform.position;
			position.y -= 1;
			transform.position = position;
	}


	// steer the block
	void checkInput() {
		// move block left
		if (Input.GetKeyUp(KeyCode.LeftArrow)) {
			moveHorizontal(-1);
		}
		// move block right
		else if (Input.GetKeyUp(KeyCode.RightArrow)) {
			moveHorizontal(1);
		}

		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			rotateBlockRight();
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			rotateBlockLeft();
		}

		// drop the block
		if (Input.GetButtonDown("Drop")) {
			fallingInterval = 0f;
			dropped = true;
		}
	}


	public void leftButton() {
		if (!isFrozen)
			moveHorizontal (-1);
	}

	public void rightButton() {
		if (!isFrozen)
			moveHorizontal (1);
	}

	public void rolButton() {
		if (!isFrozen)
			rotateBlockLeft ();
	}

	public void rorButton() {
		if (!isFrozen)
			rotateBlockRight ();
	}

	public void dropButton() {
		if (!isFrozen) {
			fallingInterval = 0f;
			dropped = true;
		}
	}


	// move the block physically
	void moveHorizontal(int direction) {
		if (!Gamemanager.thisOne.checkBlock(blockMatrix, size,xPosition+direction, yPosition)) {
			GetComponent<AudioSource>().PlayOneShot(swoosh);
			xPosition += direction;
			Vector3 position = transform.position;
			position.x += direction;
			transform.position = position;
		}
	}

	// rotate the block right, 90°
	void rotateBlockRight() {
		// generate a temporary matrix to store the rotated block
		bool[,] tempMatrix = new bool[size, size];
		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size;x++) {
				// copy the values and rotate
				tempMatrix[y,x] = blockMatrix[x,(size-1)-y];
			}
		}
		
		// check if rotated block overlaps something
		if (!Gamemanager.thisOne.checkBlock (tempMatrix, size, xPosition, yPosition)) {
			GetComponent<AudioSource>().PlayOneShot(swoosh);
			// if not, copy the temp matrix to the original blockmatrix
			System.Array.Copy(tempMatrix, blockMatrix, size*size);
			// and don't forget: rotate the block on the screen
			transform.Rotate(Vector3.forward*-90.0f);
		}
	}

	// rotate the block left, 90°
	void rotateBlockLeft() {
		// generate a temporary matrix to store the rotated block
		bool[,] tempMatrix = new bool[size, size];
		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size;x++) {
				// copy the values and rotate
				tempMatrix[(size-1)-y,(size-1)-x] = blockMatrix[x,(size-1)-y];
			}
		}
		
		// check if rotated block overlaps something
		if (!Gamemanager.thisOne.checkBlock (tempMatrix, size, xPosition, yPosition)) {
			GetComponent<AudioSource>().PlayOneShot(swoosh);
			// if not, copy the temp matrix to the original blockmatrix
			System.Array.Copy(tempMatrix, blockMatrix, size*size);
			// and don't forget: rotate the block on the screen
			transform.Rotate(Vector3.forward*+90.0f);
		}
	}

	public void setFallingInterval(float interval){
		fallingInterval = interval;
	}

	public bool isDropped(){
		return dropped;
	}
}
