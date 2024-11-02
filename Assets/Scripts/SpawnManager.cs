using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] SpawnPrefabs;
    public GameManager gameManager;
    public float spawnRate = 1.0f;
    private GameObject Enemy;
    public int enemyCount;
    public int powerupCount;
    public float xRange = 6;
    public float xNegRange = -6;
    public float zRange = 23;
    private int waveNumber = 1;
    public GameObject[] powerupPrefab;
    public bool gameStart;
    public float totalTime = 15;
    public bool timeUp = false;
    public bool etimeUp = false;
    public float etotalTime = 4;
    public float increment;
    public bool isPaused = false;


    // Update is called once per frame
    void Update()
    {   //value to generate a condition to add extra enemy as game progresses
        increment = (etotalTime) % (waveNumber);

        if (gameStart)
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;
            powerupCount = FindObjectsOfType<Spin>().Length;
            int index = Random.Range(0, 4);
        }
        if (gameManager.isGameActive && isPaused == false)
        {
            Timer();
            EnemySpawnTimer();
        }
    }

    //powerup timer
    public void Timer()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            timeUp = false;
        }

        else if (totalTime <= 0)
        {
            timeUp = true;
            Spawn();
        }
    }
    //countdown to spawn enemies
    void EnemySpawnTimer()
    {
        if (etotalTime > 0)
        {
            etotalTime -= Time.deltaTime;
            etimeUp = false;
        }
        else if (totalTime <= 0)
        {
            etimeUp = true;
            SpawnEnemy();
        }
    }
    //random position generator
    Vector3 GenerateRandomPosition()
    {
        float spawnPosX = Random.Range(xNegRange, xRange);
        float spawnPosZ = Random.Range(-zRange, zRange);
        Vector3 randomPos = new Vector3(spawnPosX, 1.5f, spawnPosZ);

        return randomPos;
    }

    //powerup generator over 15 seconds
    void Spawn()
    {
        if (powerupCount == 0 && timeUp)
        {
            int index = Random.Range(0, 4);
            Instantiate(powerupPrefab[index], GenerateRandomPosition(), powerupPrefab[index].transform.rotation);
            totalTime = 15 + waveNumber;
            SingleEnemySpawn();
            waveNumber++;
        }
    }

    //enemy generator over 4 seconds
    void SpawnEnemy()
    {
        if (enemyCount == 0 && etimeUp && !isPaused && gameManager.isGameActive)
        {
            Instantiate(SpawnPrefabs[0], GenerateRandomPosition(), SpawnPrefabs[0].transform.rotation);
            etotalTime = 4;
            etotalTime = etotalTime - Mathf.Abs(Mathf.Sqrt(increment));
        }
    }
    //enemy spawn
    void SingleEnemySpawn()
    {
        Instantiate(SpawnPrefabs[0], GenerateRandomPosition(), SpawnPrefabs[0].transform.rotation);
    }
    public void Pause()
    {
        isPaused = true;
    }

    //resume state checker
    public void Resume()
    {
        isPaused = false;
    }
    //a start game function for when game starts from button tap
    public void StartGame()
    {
        gameStart = true;
        gameManager.isGameActive = true;
        //   SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab[0], GenerateRandomPosition(), SpawnPrefabs[0].transform.localRotation);
    }
}










