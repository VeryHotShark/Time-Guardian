    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadLevel : MonoBehaviour
{

    public TextMeshProUGUI loadPercent;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    // Update is called once per frame
    IEnumerator LoadLevelAsync()
    {
        string scene;

        if (MainMenu.instance.tutorialDone)
            scene = "MainScene";
        else
            scene = "TutorialScene";

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            int percent = Mathf.CeilToInt(asyncOperation.progress * 100f);
            loadPercent.SetText("Loading... " + percent.ToString() + "%");

            /*
            if(asyncOperation.isDone)
                SceneManager.LoadScene("Main");
            */

            yield return null;
        }
    }
}
