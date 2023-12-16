using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameManager gameManager;
    public int enemyCount;
    private float spawnRangeX = 23;
    private float spawnRangeZ = 17;
    public int waveNumber = 1;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<MoveEnemy>().Length;

        if (enemyCount == 0 && !gameManager.stopGame)
        {
            if (gameManager.levelTwo == false)
            {
                waveNumber++;
                SpawnEnemyWave(waveNumber);
            }
            else if (gameManager.levelTwo == true)
            {
                waveNumber++;
                SpawnEnemyWave(waveNumber);
            }
        }
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawmPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 randomPos = new Vector3(spawnPosX, 0f, spawmPosZ);

        return randomPos;
    }
}
