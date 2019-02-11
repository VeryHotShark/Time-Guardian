using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool tutorial = false;
    public bool gameIsOver = false;
    public bool playerWon = false;
    public bool gameStarted = false;

    public GameObject endScreen;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;

    MainMenu mainMenuSettings;

	// Use this for initialization
	void Awake ()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
	}

    private void Start()
    {
        if (!tutorial)
            FadeScreen.instance.StartFade(0f);

        mainMenuSettings = MainMenu.instance;

        if(!tutorial)
        {
            if(mainMenuSettings != null)
            {
                if (mainMenuSettings.scoreText == null)
                    mainMenuSettings.scoreText = scoreText.gameObject;

                if (mainMenuSettings.endScreen == null)
                    mainMenuSettings.endScreen = endScreen.gameObject;
            }
        }

        if(!tutorial)
        {
            gameIsOver = false;
            playerWon = false;
            scoreText.gameObject.SetActive(false);
            endScreen.SetActive(false);
            Player.OnPlayerDeath += ShowUI;
            TimeManager.instance.OnTimeEnd += ShowUI;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameIsOver && playerWon)
        {
            gameIsOver = true;
            ShowUI();
        }

        if(gameIsOver)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowUI()
    {
        if(playerWon)
        {
            gameOverText.SetText("YOU WON!");
        }
        else
        {
            gameOverText.SetText("YOU FAILED");
        }

        if(mainMenuSettings != null)
        {
            if(mainMenuSettings.endlessMode)
            {
                if (scoreText == null)
                    scoreText = mainMenuSettings.scoreText.GetComponent<TextMeshProUGUI>();

                scoreText.gameObject.SetActive(true);
                float timeSurvived = TimeManager.instance.timeSurvived;
                int minutesSurvived = Mathf.FloorToInt(timeSurvived / 60f);
                int secondsSurvived = Mathf.CeilToInt(timeSurvived % 60f);
                scoreText.SetText("Time Survived: " +"\n"+ + minutesSurvived + "m " + secondsSurvived + "s");
            }
        }

        if (endScreen == null)
            endScreen = mainMenuSettings.endScreen;

        endScreen.SetActive(true);
        TimeManager.instance.OnTimeEnd -= ShowUI;
        Player.OnPlayerDeath -= ShowUI;
    }

}
