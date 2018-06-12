﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// this script is attached to the empty gameObject in the scene "game2"

public class Gamemanager : MonoBehaviour {

	// Inspector
	// create some slots to draw the blocks in
	public GameObject[] blocks;
	public GameObject cube;
	// reference to the audio effects
	public AudioClip drop;
	public AudioClip shatter;
	// Reference to the game font
	public Font myFont;

	static public Gamemanager thisOne;
	// Grid boundaries
	public int _fieldHeight = 13;
	public int _fieldWidth = 10;
	// maximal Size of a block
	// --> I-Block --> 4
	public int maxBlockSize = 4;
	private bool[,] field;
	//virtual grid with thick walls
	private int fieldWidth;
	private int fieldHeight;

	// sneak preview to tetrominoe falling next
	private int nextBlock;
	private GameObject goNextBlock;

	// some statistics
	private int level;
	private int score;
	private int linesCleared;

	public Canvas mobileUI;

	public int getFieldHeight() {
		return fieldHeight;
	}

	public int getFieldWidth() {
		return fieldWidth;
	}

	// I tried to make the GUI resolution independent
	// so, here the native resolution is set to 1024x768
	// OnGUI resizes the GUI-elements
	// These values have to be float,
	// else division in method OnGUI will return null (rx, ry)
	private float nativeWidth = 1024;
	private float nativeHeight = 768;

    // finite state machine for game states
    enum GameStates
    {
        game,
        gameOver,
        gamePaused,
    };

    GameStates gameState;

    // Use this for initialization
    void Start () {
		// create a singleton of this instance
		if (!thisOne) {
			thisOne = this;
		}
		else {
			Debug.Log("Only one Instance of this script is allowed");
			return;
		}

		if (Application.platform != RuntimePlatform.Android)
			mobileUI.gameObject.SetActive (false);
		else
			mobileUI.gameObject.SetActive (true);

		// set the gameState to "game"
		gameState = GameStates.game;

		// starting level is zero
		level = 0;
		// starting score is zero
		score = 0;
		// linesCleared is used to compute level
		linesCleared = 0;

		// next block to be spawned
		// first time: -1
		nextBlock = -1;

		// generate Bitfield for Collisions
		// walls are 4 bricks thick to avoid blocks penetrate the wall
		fieldWidth = _fieldWidth + maxBlockSize * 2;
		fieldHeight = _fieldHeight + maxBlockSize;
		field = new bool[fieldWidth, fieldHeight];

		// thick walls...
		for (int y = 0; y < fieldHeight; y++) {
			for (int x = 0; x < maxBlockSize;x++) {
				// ... on the left...
				field[x,y] = true;
				// ... and on the right
				field[fieldWidth - 1 - x,y] = true;
			}
		}

		// ground control :-)
		for (int x = 0; x < fieldWidth; x++) {
			field[x,0] = true;
		}

		//let's get to it: 
		// create the first block in the game
		spawnBlock ();
	}


	// create a new random block at the top of the grid
	void spawnBlock() {
		GameObject block;
		// is this the first block ever?
		if (nextBlock == -1) {
			// instantiate a new block...
			block = (GameObject)Instantiate (blocks [Random.Range (0, blocks.Length - 1)]);
		}
		else {
			// instantiate the "nextBlock"-Block
			block = (GameObject)Instantiate (blocks [nextBlock]);
		}
		// ok so far. we have a current block and the next block (sneak preview)
		// ...and set the falling speed according to the level
		block.GetComponent<Block>().setFallingInterval(1f-0.025f*level);
		// generate the next block
		nextBlock = Random.Range (0, blocks.Length - 1);
		if (goNextBlock != null)
			Destroy(goNextBlock);
		goNextBlock = (GameObject)Instantiate (blocks [nextBlock]);
		goNextBlock.GetComponent<Block>().freeze();
	}
	

	// Update is called once per frame
	void Update () {
		// check the game states the game may be in
		switch (gameState) {
		case GameStates.game:
			// if we are playing and "P" is pressed,
			// we change to state pause
			if (Input.GetKeyUp(KeyCode.P)){
				gameState = GameStates.gamePaused;
				Time.timeScale = 0f;
			}
			break;
		case GameStates.gamePaused:
			// if the game is paused and "P" is pressed,
			// we change back to state game
			if (Input.GetKeyUp(KeyCode.P)){
				gameState = GameStates.game;
				Time.timeScale = 1f;
			}
			break;
		case GameStates.gameOver:
			// if the game is over,
			// go back to the menu
			if (Input.GetKeyUp(KeyCode.Space)){
				Application.LoadLevel(" ");
			}
				break;
		}
	}

	// why the same function?
	// we destroy the parent block and his script. This way, we give control to a function in this script
	// before destroying the block. So this routine can run even if the block is destroyed
	public void _setBlock(bool[,] blockMatrix, int size, int xPos, int yPos,bool dropped) {
		// play the drop sound
		GetComponent<AudioSource>().PlayOneShot (drop);
		//check the blockMatrix
		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size; x++) {
				// do we have a brick set?
				if (blockMatrix[x,y]) {
					// Instantiate a new cube at this position
					Instantiate(cube, new Vector3(xPos +x, yPos-y,0), Quaternion.identity);
					field[xPos+x,yPos-y] = true;
				}
			}
		}
		// dropping a block scores
		scoreBlock(dropped);

		// complete rows can olny be in the range of the dropped block
		checkRows (yPos - size, size);
		// go ahead, make my day... uhhh... game
		if (gameState == GameStates.game)
			spawnBlock();
	}



	// The Container-Block must be destroyed when reaching the ground or touching another
	// brick from the top. Why not just use the current cubes for the game?
	//  We have to test for x/y coordinates which may have be corrupted by block rotation :-/
	public void setBlock(bool[,] blockMatrix, int size, int xPos, int yPos,bool dropped) {
		_setBlock (blockMatrix, size, xPos, yPos, dropped);
	}


	// check the collision matrix at a certain position
	public bool checkBlock(bool[,] blockMatrix, int size, int xPos, int yPos) {
		for (int y = size-1; y>=0; y--) {
			for (int x = 0; x < size; x++) {
				if (blockMatrix[x,y] && field[xPos+x,yPos-y]) {
					return true;
				}
			}
		}
		return false;
	}



	void checkRows(int yStart, int size) {
		int y,x;
		int rowsCollapsed = 0;
		// make sure to start above the grid (floor)
		if (yStart < 1)
			yStart = 1;
		for (y = yStart; y < yStart+size; y++) {
			// take the thick walls into account
			for (x = maxBlockSize; x < fieldWidth-maxBlockSize;x++) {
				// empty spaces? then leave the current row alone
				if (!field[x,y])
					break;
			}
			// complete row is filled ?
			if (x== fieldWidth - maxBlockSize) {
				//remove the row
				if (collapseRow(y))
					rowsCollapsed++;
				// check the row again after collapse
				// maybe there was more than one row filled
				y--;
			}
		}
		// could we clean one or more rows?
		if (rowsCollapsed > 0) {
			iTween.ShakePosition (Camera.main.gameObject, new Vector3 (0.1f * rowsCollapsed, 0.1f * rowsCollapsed, 0), 0.25f*rowsCollapsed);
			GetComponent<AudioSource>().PlayOneShot (shatter);
			scoreLine(rowsCollapsed);
			checkLevel(rowsCollapsed);
		}
	}

	void checkLevel(int rowsCollapsed) {
		linesCleared += rowsCollapsed;
		if (linesCleared >= (level+1) * 5)
		{
			level++;
		}
	}

	void scoreBlock(bool dropped){
		if (dropped) {
			// each cube scores twice when hard dropped
			score += 2*4;
		}
		else {
			// each cube scores once when soft dropped
			score += 1*4;
		}
	}

	void scoreLine(int rowsCollapsed) {
		// tetris scoring system:
		// level n
		// one row:		40*(n+1)
		// two rows:	100*(n+1)
		// three rows:	300*(n+1)
		// four rows:	1200(n+1)
		switch (rowsCollapsed){
		case 1:
			score += 40 * (level+1);
			break;
		case 2:
			score += 100 * (level+1);
			break;
		case 3:
			score += 300 * (level+1);
			break;
		case 4:
			score += 1200 *(level+1);
			break;
		default:
			break;
		}
	}



	bool collapseRow(int row) {
		// Move rows down one in the array
		for (int y = row; y < fieldHeight -1; y++) {
			// take the thick walls into account
			for (int x = maxBlockSize; x < fieldWidth - maxBlockSize;x++) {
				field[x,y] = field[x,y+1];
			}
		}
		// make sure top line is cleared 
		for (int x = maxBlockSize; x < fieldWidth - maxBlockSize;x++) {
			field[x,fieldHeight-1] = false;
		}
		// now for the gameObjects: destroy cubes in the deleted row
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube");
		Transform[] cubeReferences = new Transform[cubes.Length];
		int[] cubePositions = new int[cubes.Length];
		int cubesToMove = 0;
		foreach (GameObject cube in cubes) {
			// cubes above have to be moved
			if (cube.transform.position.y > row) {
				cubePositions[cubesToMove] = (int)cube.transform.position.y;
				cubeReferences[cubesToMove] = cube.transform;
				cubesToMove++;
			}
			// cubes in the line have to be destroyed
			else if (cube.transform.position.y == row) {
				// may cause problems with array index! Keep track!
				Destroy(cube);
			}
		}
		// now move the cubes
		for (int c = 0; c < cubesToMove; c++) {
			Vector3 position = cubeReferences[c].position;
			position.y -= 1;
			cubeReferences[c].position = position;
		}
		return true;
	}
	
	public void gameOver() {
		gameState = GameStates.gameOver;
	}


	public void leftButton() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject go in blocks) {
			go.GetComponent<Block>().leftButton();
		}
	}
	
	public void rightButton() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject go in blocks) {
			go.GetComponent<Block>().rightButton();
		}
	}
	
	public void rolButton() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject go in blocks) {
			go.GetComponent<Block>().rolButton();
		}
	}
	
	public void rorButton() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject go in blocks) {
			go.GetComponent<Block>().rorButton();
		}
	}
	
	public void dropButton() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject go in blocks) {
			go.GetComponent<Block>().dropButton();
		}
	}



 	void OnGUI() {
		// care for resolution independence
		float rx = Screen.width / nativeWidth;
		float ry = Screen.height / nativeHeight;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0,0,0), Quaternion.identity, new Vector3 (rx, ry, 1));

		// set the game statistics
		GUIStyle myTitleStyle = new GUIStyle(GUI.skin.label);
		myTitleStyle.fontSize = 32;
		GUI.Label(new Rect(10,10,600,100), "Score: " + score.ToString("D7"), myTitleStyle);
		GUI.Label(new Rect(10,50,600,100), "Lines: " + linesCleared.ToString("D7"), myTitleStyle);
		GUI.Label(new Rect(10,90,600,100), "Level: " + level.ToString("D3"), myTitleStyle);
		GUI.Label(new Rect(10,130,600,100), "Next: ", myTitleStyle);

		// dependent on the current game state,
		// draw a game over screen
		if (gameState == GameStates.gameOver){
			GUIStyle myOverStyle = new GUIStyle(GUI.skin.label);
			myOverStyle.fontSize = 100;
			myOverStyle.font = myFont;
			GUI.Label(new Rect(60,250,1024,300), "   nice！！！", myOverStyle);
		}

		// draw a pause screen
		if (gameState == GameStates.gamePaused){
			GUIStyle myPauseStyle = new GUIStyle(GUI.skin.label);
			myPauseStyle.fontSize = 160;
			myPauseStyle.font = myFont;
			GUI.Label(new Rect(60,250,1024,300), "Game Paused", myPauseStyle);
			GUIStyle myPause2Style = new GUIStyle(GUI.skin.label);
			myPause2Style.fontSize = 90;
			myPause2Style.font = myFont;
			GUI.Label(new Rect(60,420,1024,300), "Press P To Continue", myPause2Style);
		}
	}
}
