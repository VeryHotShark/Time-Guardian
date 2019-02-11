using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour {

    [Header("UI Animations")]
    public GameObject animationBar;
    public float animationLength = 2f;
    TextMeshProUGUI waveText;

    [Header("General")]// General Variables
    public GameObject PickUp;
    public bool waveMode;
    public bool endlessMode;
    float lightHeight = 5f;
    float groundHalfDimension = 9f;

    [Header("Waves Mode")]
    public Wave[] waves;
    public float breakBeforeNextWave = 2f;
    Wave currentWave;

    [Header("Endless Mode")]// Endless Mode
    public float maxSpawnRate = 20f;//spawns per seconds
    public float maxEnemySpeed = 20f;
    public float maxTimeBeforeReachingMaxDifficulty = 180f;
    public AnimationCurve spawnRateCurve;
    public AnimationCurve enemySpeedCurve;
    public Enemy enemy;
    float spawnRate;
    float enemySpeed;

    float timer;
    float timeBetweenSpawns;

    float spawnHeight = 2f;

    int enemiesLeftToSpawn;
    int enemiesAlive;
    int currentWaveIndex = -1;
	// Use this for initialization
	void Start ()
    {
        MainMenu mainMenuSetting = MainMenu.instance;
        if(mainMenuSetting != null)
        {
            waveMode = mainMenuSetting.waveMode;
            endlessMode = !waveMode;
        }

        timeBetweenSpawns = 1f / spawnRate;
        PickUp.SetActive(false);

        waveText = animationBar.GetComponentInChildren<TextMeshProUGUI>();
        animationBar.SetActive(false);

        if (waveMode)
        {
            if (waves.Length > 0)
            {
                StartCoroutine(NextWave());
            }
        }

        if(endlessMode)
        {
            GameManager.instance.gameStarted = true;
            PickUp.SetActive(true);
        }
	}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (endlessMode)
        {
            float timeSinceLevelStarted = Time.timeSinceLevelLoad;
            float difficultyPercent = timeSinceLevelStarted / maxTimeBeforeReachingMaxDifficulty;
            float percent01 = Mathf.Clamp01(difficultyPercent);

            spawnRate = spawnRateCurve.Evaluate(percent01);
            spawnRate *= maxSpawnRate;
            timeBetweenSpawns = 1f / spawnRate;

            enemySpeed = enemySpeedCurve.Evaluate(percent01);
            enemySpeed *= maxEnemySpeed;

            if (timer > timeBetweenSpawns)
            {
                StartCoroutine(SpawnEnemy());
            }
        }

    }

    IEnumerator NextWave()
    {
        if(!GameManager.instance.gameIsOver)
        {
            currentWaveIndex++;

            if (currentWaveIndex >= waves.Length)
            {
                GameManager.instance.playerWon = true;
                yield break;
            }

            yield return new WaitForSeconds(1f);

            animationBar.SetActive(true);
            waveText.SetText("Wave: " + (currentWaveIndex +1 ).ToString());

            yield return new WaitForSeconds(animationLength);

            if(currentWaveIndex == 0)
            {
                GameManager.instance.gameStarted = true;
                PickUp.SetActive(true);
            }

            animationBar.SetActive(false);

            currentWave = waves[currentWaveIndex];
            enemiesLeftToSpawn = currentWave.enemyAmount;
            enemiesAlive = enemiesLeftToSpawn;
            timeBetweenSpawns = 1f / currentWave.spawnRate;

            for(int  i = enemiesLeftToSpawn; i > 0; i--)
            {
                IEnumerator spawnEnemy = SpawnEnemy();

                /*
                if (spawnEnemy != null)
                    StopCoroutine(spawnEnemy);  
                */

                StartCoroutine(spawnEnemy);   
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        if (!GameManager.instance.gameIsOver)
        {

            timer = 0f;

            Vector3 randomSpawnPos = new Vector3(Random.Range(-groundHalfDimension, groundHalfDimension), spawnHeight, Random.Range(-groundHalfDimension, groundHalfDimension));
            GameObject light = ObjectPooler.instance.GetPooledObject();
            light.transform.position = new Vector3(randomSpawnPos.x, lightHeight, randomSpawnPos.z);

            yield return new WaitForSeconds(2f);

            light.gameObject.SetActive(false);

            Enemy spawnedEnemy = Instantiate(enemy, randomSpawnPos, Quaternion.identity) as Enemy;

            if(endlessMode)
            {
                spawnedEnemy.setForce = Mathf.Clamp(enemySpeed + Random.Range(-1f,1f), 1f,maxEnemySpeed);
            }

            if (waveMode)
            {
                enemiesLeftToSpawn--;
                spawnedEnemy.setForce = currentWave.enemySpeed + Random.Range(-1f, 1f);
                spawnedEnemy.OnEnemyDeath += DecreaseEnemiesAliveCount;
            }

        }
    }

    void DecreaseEnemiesAliveCount(Enemy diedEnemy)
    {
        enemiesAlive--;
        //Debug.Log(" enemiesAlive: " + enemiesAlive);
        diedEnemy.OnEnemyDeath -= DecreaseEnemiesAliveCount;
        if (enemiesAlive <= 0)
        {
            StartCoroutine(NextWave());
        }
    }

}

[System.Serializable]
public class Wave
{
    public Enemy enemy;
    public int enemyAmount;
    public float spawnRate;
    public float enemySpeed;
}
