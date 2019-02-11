using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionTutorial : Tutorial {

    //public GameObject light;
    public Enemy enemy;
    public Transform[] waypoints;

    int enemiesNumber;
    int enemiesAlive;

	// Use this for initialization
	public override void Execute ()
    {
        enemiesNumber = waypoints.Length;
        enemiesAlive = enemiesNumber;
        StartCoroutine(SpawnEnemy());
	}
	
	// Update is called once per frame
	IEnumerator SpawnEnemy ()
    {

        for (int i = 0; i < waypoints.Length; i++)
        {
            GameObject lightIndicator = ObjectPooler.instance.GetPooledObject();
            lightIndicator.transform.position = waypoints[i].position + Vector3.up * 5f;
            Debug.Log(i);

            Destroy(lightIndicator, 2f);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < waypoints.Length; i++)
        {
            //GameObject lightIndicator = ObjectPooler.instance.GetPooledObject();
            //lightIndicator.transform.position = waypoints[i].position + Vector3.up * 5f;


            //lightIndicator.SetActive(false);
            GameObject.FindObjectOfType<Player>().doSlowMo = true;
            Enemy spawnedEnemy = Instantiate(enemy, waypoints[i].position + Vector3.up * 2f, Quaternion.identity, transform) as Enemy;
            spawnedEnemy.setForce = 5f;
            spawnedEnemy.OnEnemyDeath += DecreaseEnemiesAlive;

            yield return null;
        }
	}

    void DecreaseEnemiesAlive(Enemy enemy)
    {
        enemiesAlive--;
        enemy.OnEnemyDeath -= DecreaseEnemiesAlive;

        if(enemiesAlive <= 0)
        {
            base.ChangeText("now you are ready to go. protect the time at all cost!");
            base.TutorialCompleted();
        }
    }
}
