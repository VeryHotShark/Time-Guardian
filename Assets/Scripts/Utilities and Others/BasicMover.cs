using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMover : MonoBehaviour {

    public float moveAmount;
    public float spinSpeed;

	void Update ()
    {
        if(!MainMenu.instance.pauseGame)
            transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * moveAmount, Space.World);
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
	}
}
