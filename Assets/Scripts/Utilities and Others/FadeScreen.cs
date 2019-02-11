using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeScreen : MonoBehaviour {

    public enum FadeType { FadeIn, FadeOut }
    public FadeType fade;

    public float fadeDuration;

    public Color startColor;
    Color desiredColor;

    Image fadeImage;

    #region Singleton
    public static FadeScreen instance;

	// Use this for initialization
	void Awake ()
    {
        //DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

	}

    #endregion

    private void Start()
    {
        fadeImage = GetComponent<Image>();
        desiredColor = startColor;


        if (fade == FadeType.FadeIn)
        {
            startColor.a = 1f;
            desiredColor.a = 0f;
        }
        else
        {
            startColor.a = 0f;
            desiredColor.a = 1f;
        }

        fadeImage.color = startColor;
    }

    public void StartFade(float waitTime)
    {
        StartCoroutine(Fade(waitTime));
    }

    // Update is called once per frame
    public IEnumerator Fade(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        float percent = 0f;
        float fadeSpeed = 1f / fadeDuration;

        while(percent < 1f)
        {
            percent += Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = Color.Lerp(startColor, desiredColor, percent);
            yield return null;
        }


	}
}
