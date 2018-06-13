using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public float fallSpeed = 1.0f;

<<<<<<< HEAD:Assets/6-12/Game.cs
    public static int gridWidth =9;
    public static int gridHeight = 15;
=======
    public static int gridWidth = 5;
    public static int gridHeight = 30;
>>>>>>> da22dd9603b09d4c9446704b25be160651518dec:Assets/Scripts/Game.cs

    public static Transform[,,] grid = new Transform[gridWidth, gridHeight, gridWidth];

    private bool gameStarted = false;
    private int numberOfRowsThisTurn = 0;

    private GameObject nextTetromino;
    private GameObject previewTetromino;
    private string previewTetrominoName;


    private Vector3 initTetrominoPosition = new Vector3(2.0f, 15.0f, 2.0f);
    private Vector3 previewTetrominoPosition = new Vector3(-4.0f, 5.0f, 5.0f);

    public GameObject liveTetromino;

    // Use this for initialization
    void Start () {
        SpawnNextTetromino();
    }

    public void DeleteRow()
    {

        for (int y = 0; y < gridHeight; y++)
        {

            if (IsFullRowAt(y))
            {

                DeleteMinoAt(y);

                MoveAllRowDown(y + 1);

                y--;
            }
        }
    }

    public bool IsFullRowAt(int y)
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {

                if (grid[x, y, z] == null)
                {
                    return false;
                }
            }
        }

        // found one row full
        numberOfRowsThisTurn++;
        return true;
    }

    public void DeleteMinoAt(int y)
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {

                Destroy(grid[x, y, z].gameObject);

                grid[x, y, z] = null;
            }
        }
    }

    public void MoveRowDown(int y)
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {

                if (grid[x, y, z] != null)
                {

                    grid[x, y - 1, z] = grid[x, y, z];

                    grid[x, y, z] = null;

                    grid[x, y - 1, z].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    public void MoveAllRowDown(int y)
    {

        for (int i = y; i < gridHeight; i++)
        {

            MoveRowDown(i);
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos)
    {

        if (pos.y > gridHeight - 1)
        {

            return null;

        }
        else
        {

            return grid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public bool CheckIsAboveGrid(Tetromino tetromino)
    {

        foreach (Transform mino in tetromino.transform)
        {

            Vector3 pos = Round(mino.position);

            if (pos.y > gridHeight - 1)
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateGrid(Tetromino tetromino)
    {

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridWidth; z++)
                {

                    if (grid[x, y, z] != null)
                    {

                        if (grid[x, y, z].parent == tetromino.transform)
                        {

                            grid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        foreach (Transform mino in tetromino.transform)
        {

            Vector3 pos = Round(mino.position);

            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y, (int)pos.z] = mino;
            }
        }

    }


    public void SpawnNextTetromino()
    {
        if (!gameStarted)
        {

            gameStarted = true;

            string nextTetroName = GetRandomTetromino();
            string nextTetroPath = "Prefab/Model/Tmino_" + nextTetroName;
            string nextTetroShadowPath = "Prefab/Model/Smino_" + nextTetroName;

            nextTetromino = (GameObject)Instantiate(Resources.Load(nextTetroPath, typeof(GameObject)), initTetrominoPosition, Quaternion.identity);
            GameObject nextTetroShadow = (GameObject)Instantiate(Resources.Load(nextTetroShadowPath, typeof(GameObject)), initTetrominoPosition, Quaternion.identity);

            //- preview tetromino
            previewTetrominoName = GetRandomTetromino();
            string previewTetroPath = "Prefab/Model/Tmino_" + previewTetrominoName;

            previewTetromino = (GameObject)Instantiate(Resources.Load(previewTetroPath, typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled = false;

        }
        else
        {
            previewTetromino.transform.localPosition = initTetrominoPosition;
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetromino>().enabled = true;

            string nextTetroShadowPath = "Prefab/Model/Smino_" + previewTetrominoName;
            GameObject nextTetroShadow = (GameObject)Instantiate(Resources.Load(nextTetroShadowPath, typeof(GameObject)), initTetrominoPosition, Quaternion.identity);


            //- preview tetromino
            previewTetrominoName = GetRandomTetromino();
            string previewTetroPath = "Prefab/Model/Tmino_" + previewTetrominoName;

            previewTetromino = (GameObject)Instantiate(Resources.Load(previewTetroPath, typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled = false;

        }

        liveTetromino = nextTetromino;
    }

    public bool CheckIsInsideGrid(Vector3 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.z >= 0 && (int)pos.z < gridWidth && (int)pos.y >= 0;
    }

    public Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
    string GetRandomTetromino()
    {
        int randomTetromino = Random.Range(1, 5); ///////////////////FOR SET HOW MUCH TETRIS FORM WE HAVE///////////////////////////

        string randomTetrominoName = "";

        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "T";
                break;
            case 2:
                randomTetrominoName = "I";
                break;
            case 3:
                randomTetrominoName = "L";
                break;
            case 4:
                randomTetrominoName = "Square";
                break;
           /* case 5:
                randomTetrominoName = "Prefab/Model/Tmino_J";
                break;
                 case 6:
                      randomTetrominoName = "Prefab/Model/Tmino_S";
                      break;
                  case 7:
                      randomTetrominoName = "Prefab/Model/Tmino_Z";
                      break;*/


        }
        return randomTetrominoName;

<<<<<<< HEAD:Assets/6-12/Game.cs
    }

    public void GameOver()
    {


=======
    public void GameOver()
    {


>>>>>>> da22dd9603b09d4c9446704b25be160651518dec:Assets/Scripts/Game.cs
      //  Application.LoadLevel("GameOver");
    }
}
