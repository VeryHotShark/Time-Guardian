using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour {

    public Player player; // Reference to our player
    public List<Tutorial> tutorialList = new List<Tutorial>(); // List of our tutorials
    Tutorial currentTutorial; // our current tutorial

    public TextMeshProUGUI tutorialDescription; // variable for our tutorial text

    int currentTutorialIndex = 0;
    bool tutorialCompleted; // bool to check if we finish whole tutorial

    bool slowMotion = false;

	// Use this for initialization
	void Start ()
    {
        player.doSlowMo = slowMotion;

        if(tutorialList != null)
            StartTutorial();
	}

    void StartTutorial()
    {
        foreach (Tutorial tutorial in tutorialList) // for each tutorial in our tutorialList 
        {
            tutorial.OnTutorialFinish += NextTutorial; // we subscribe our NextTutorial method to OnTutorialFinish event
            tutorial.OnTextChange += ShowTutorialDescription; // same but with text method
        }

        currentTutorial = tutorialList[currentTutorialIndex]; // we set current tutorial to first in the tutorial list
        currentTutorial.Execute(); // and we call Execute method in that tutorial class
        ShowTutorialDescription(); // and we show tutorial description
    }

    void NextTutorial()
    {
        currentTutorial.OnTutorialFinish -= NextTutorial; // when we go to next tutorial we have to unsubscribe to events that we subscribe at the beginning of the game
        currentTutorial.OnTextChange -= ShowTutorialDescription; // same

        currentTutorialIndex++; // we increment tutorial index

        if (currentTutorialIndex >= tutorialList.Count) // chech if we went through all tutorials
        {
            FadeScreen.instance.StartFade(2f); // if yes we fade to next level
            StartCoroutine(ChangeScene()); // we change scene
            tutorialCompleted = true; // set tutorialCompleted bool to true
            MainMenu.instance.tutorialDone = tutorialCompleted;
            return;
        }

        currentTutorial = tutorialList[currentTutorialIndex]; // if not we we go to next tutorial in our tutorial List
        currentTutorial.Execute(); // and we call its execute method
        ShowTutorialDescription(); // and show description method
    }

	// Update is called once per frame
	void ShowTutorialDescription ()
    {
        tutorialDescription.SetText(currentTutorial.description);
	}

    IEnumerator ChangeScene()
    {
        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("MainScene");

    }
}
