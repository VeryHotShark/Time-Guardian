using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public float timeAmount = 60f;
    public TextMeshProUGUI timeText;
    public Image leftBar, rightBar;

    public float timeLeft { get; set; }
    public float timeSurvived { get; set; }
    int timeInSeconds;

    public event System.Action OnTimeEnd;

    static TimeManager _instance;

    public static TimeManager instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        timeLeft = timeAmount; 
        if(timeText)
            SetTimerText();
        UpdateBar();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if((!GameManager.instance.gameIsOver && GameManager.instance.gameStarted) || GameManager.instance.tutorial)
        {
            timeSurvived += Time.deltaTime;
            timeLeft -= Time.deltaTime;
            timeInSeconds = Mathf.CeilToInt(timeLeft);
            if (timeText)
                SetTimerText();
            UpdateBar();
        }

        if (timeLeft < 0f)
        {
            if(GameManager.instance.tutorial)
            {
                CameraShake.isShaking = true;
                timeLeft = timeAmount;
            }
            else
            {
                GameManager.instance.gameIsOver = true;
                if(OnTimeEnd != null)
                {
                    OnTimeEnd();
                }
            }
        }

	}

    void SetTimerText()
    {
        timeText.SetText(timeInSeconds.ToString() + "s");
    }

    void UpdateBar()
    {
        float timeAsPercentage = timeLeft / timeAmount;
        leftBar.fillAmount = timeAsPercentage;
        rightBar.fillAmount = timeAsPercentage;
    }
}
