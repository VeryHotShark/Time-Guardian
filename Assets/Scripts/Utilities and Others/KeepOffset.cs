using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOffset : MonoBehaviour {

    public Transform player;
    Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - player.position;	
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = player.position + offset;
	}
}
