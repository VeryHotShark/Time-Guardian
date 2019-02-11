using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float duration = 0.2f;
    public float power = 2f;
    //public float returnSpeed = 3f;

    Vector3 initialPos;

    public static bool isShaking = false;

    float timer;
    float initDuration;

	// Use this for initialization
	void Start ()
    {
        initialPos = transform.localPosition;
        initDuration = duration;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(isShaking)
        {
            if(duration > 0f)
            {
                duration -= Time.unscaledDeltaTime;
                Vector3 randomPoint = Random.insideUnitSphere * power;
                transform.position = initialPos + randomPoint;
            }
            else
            {
                isShaking = false;
                duration = initDuration;
                transform.position = initialPos;
            }
        }
	}
}
