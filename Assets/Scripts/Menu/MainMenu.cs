using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public GameObject endScreen { get; set; }
    public GameObject scoreText { get; set; }

    public GameObject mainScreen;
    public GameObject pauseScreen;
    public bool pauseGame;
    public bool endlessMode;
    public bool waveMode;

    public bool tutorialDone;

    public float currentSensitivity = 2f;
    [Range(0,2)]public int currentQualityLevel = 2;

    float normalTimeScale;

    public event System.Action OnPausePress;

    public static MainMenu instance;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        pauseScreen.SetActive(false);
        mainScreen.SetActive(true);

        SceneManager.sceneLoaded += OnSceneLoaded;
        //qualityDropdown.value = currentQualityLevel;
        //sensitivitySlider.value = currentSensitivity;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 0)
        {
            mainScreen.SetActive(true);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void SetEndlessMode(bool isEndless)
    {
        endlessMode = isEndless;
        waveMode = !endlessMode;
    }

    // Use this for initialization
    public void StartGame()
    {
        SceneManager.LoadScene(1);	
	}

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Application.Quit();	
	}

    public void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetSensitivity()
    {

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void Pause()
    {
        if (OnPausePress != null)
            OnPausePress();

        pauseGame = !pauseGame;
        pauseScreen.SetActive(pauseGame);

        if(pauseGame)
        {
            normalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = normalTimeScale;
        }
    }

}
