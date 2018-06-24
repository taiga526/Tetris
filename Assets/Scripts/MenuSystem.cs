using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour {

    public string SceneName;

    public Text GameOverScore;
    public Text GameOverScore2;
    public Text GameOverScore3;
    public Text GameOverScore4;

    public void PlayAgain() {
        Application.LoadLevel("Game");
    }

    private void Start()
    {
        GameOverScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        GameOverScore2.text = PlayerPrefs.GetInt("HighScore2").ToString();
        GameOverScore3.text = PlayerPrefs.GetInt("HighScore3").ToString();
        GameOverScore4.text = PlayerPrefs.GetInt("CurrentScore").ToString();

    }
}