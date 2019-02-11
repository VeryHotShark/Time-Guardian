using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAndShootingTutorial : Tutorial
{
    public int enemyNumber;
    public Enemy enemy;
    public float enemySpeed;

    public GameObject lightIndicator;

    int enemiesAlive;
    float halfMapSize = 9f;

	// Use this for initialization
	public override void Execute()
    {
        if (lightIndicator.GetComponent<Animator>().enabled == false)
            lightIndicator.GetComponent<Animator>().enabled = true;

        enemiesAlive = enemyNumber;
        lightIndicator.SetActive(false);
        StartCoroutine(SpawnEnemy());
	}
	
	// Update is called once per frame
	IEnumerator SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-halfMapSize, halfMapSize), 2f, Random.Range(-halfMapSize, halfMapSize));
        lightIndicator.transform.position = new Vector3(spawnPos.x, 5f, spawnPos.z);
        lightIndicator.SetActive(true);
        if(enemiesAlive == enemyNumber)
        {
            yield return new WaitForSeconds(1f);
            lightIndicator.GetComponent<Animator>().speed = 0f;
            yield return new WaitForSeconds(2f);
			lightIndicator.GetComponent<Animator>().speed = 1f;
            base.ChangeText("use left mouse button to shoot at enemies (kill 3 enemies)");
			yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        lightIndicator.SetActive(false);
        Enemy spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity, transform) as Enemy;
        spawnedEnemy.OnEnemyDeath += OnEnemyDeath;
        spawnedEnemy.setForce = enemySpeed;
	}

    void OnEnemyDeath(Enemy enemy)
    {
        enemiesAlive--;
        enemy.OnEnemyDeath -= OnEnemyDeath;

        if (enemiesAlive == 1)
        {
            base.ChangeText("don't let them touch you or you will lose 1 health");
        }

        if (enemiesAlive <= 0)
        {
            base.TutorialCompleted();
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(SpawnEnemy());
    }
}
