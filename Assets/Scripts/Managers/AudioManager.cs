using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Clip[] clips;

    public static AudioManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    void Start ()
    {
        foreach(Clip clip in clips)
        {
            clip.audioSource = gameObject.AddComponent<AudioSource>();
            clip.audioSource.clip = clip.clip;
            clip.audioSource.loop = clip.loop;
            clip.audioSource.pitch = clip.pitch;
            clip.audioSource.spatialBlend = clip.blend;
            clip.audioSource.volume = clip.volume;
            clip.audioSource.outputAudioMixerGroup = clip.audioMixerGroup;
        }
	}
	
	// Update is called once per frame
	public void PlayClip (string clipName)
    {
        Clip ourClip = Array.Find(clips, Clip => Clip.name == clipName);

        if (ourClip == null)
            return;

        ourClip.audioSource.Play();
	}
}

[System.Serializable]
public class Clip
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    [Range(0f,1f)] public float blend = 0.5f;
    public bool loop = false;
    [HideInInspector] public AudioSource audioSource;
    public AudioMixerGroup audioMixerGroup;
}
