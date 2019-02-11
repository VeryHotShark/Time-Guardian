using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public float bonusTime = 3f;
    float halfGroundWidth = 9f;

    public event System.Action OnPickUp;

    private void Start()
    {
        Vector3 newPos = new Vector3(Random.Range(-halfGroundWidth, halfGroundWidth), 0f, Random.Range(-halfGroundWidth, halfGroundWidth));
        transform.position = newPos;
    }

    // Update is called once per frame
    void OnTriggerEnter (Collider other)
    {
		if(other.CompareTag("Player"))
        {

            if (AudioManager.instance)
                AudioManager.instance.PlayClip("PickUp");

            if (OnPickUp != null)
                OnPickUp();

            Vector3 newPos = new Vector3(Random.Range(-halfGroundWidth, halfGroundWidth), 0f, Random.Range(-halfGroundWidth, halfGroundWidth));
            transform.position = newPos;
            if(!GameManager.instance.gameIsOver || GameManager.instance.tutorial)
                TimeManager.instance.timeLeft += bonusTime;
        }
	}
}
