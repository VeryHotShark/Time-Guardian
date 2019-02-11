using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    public bool expand;
    public Transform lightsParent;
    public GameObject lightIndicator;
    public int poolSize = 10;

    List<GameObject> poolList = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        CreatePool();
	}
	
	// Update is called once per frame
	void CreatePool ()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject light = Instantiate(lightIndicator,Vector3.zero, lightIndicator.transform.rotation, lightsParent) as GameObject;
            light.SetActive(false);
            poolList.Add(light);
        }
	}

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if(!poolList[i].activeSelf)
            {
                poolList[i].SetActive(true);
                return poolList[i].gameObject;
            }
        }

        if(expand)
        {
            GameObject light = Instantiate(lightIndicator, lightsParent) as GameObject;
            poolList.Add(light);
            return light;
        }

        return null;
    }
}
