using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;

    private float secondsUntilNextSpawn = 2f;
    private GameObject Player;
    public float timeUntilWaveSpawn = 60f;
    private float startTimeUntilWaveSpawn;
    private float waveIntervall = 0f;
    public int EnemyCountPerWave = 2;
    private int startEnemyCountPerWave;
    private List<GameObject> enemies = new List<GameObject>();
    private bool stopped = false;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        startTimeUntilWaveSpawn = timeUntilWaveSpawn;
        startEnemyCountPerWave = EnemyCountPerWave;
    }
	
	void Update () {
        if (!stopped)
        {
            secondsUntilNextSpawn -= Time.deltaTime;
            timeUntilWaveSpawn -= Time.deltaTime;
            if (secondsUntilNextSpawn < 0f)
                SpawnEnemy();
            if (timeUntilWaveSpawn < 0f)
            {
                if (waveIntervall < 0f)
                {
                    SpawnWave();
                }
                waveIntervall -= Time.deltaTime;
            }
        }

    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        float height = Random.Range(-6, 9);
        enemy.transform.position = new Vector3(Player.transform.position.x + 30, height, 0);
        enemy.GetComponent<EnemyMovement>().speed = Random.Range(-10, -2);
        secondsUntilNextSpawn = Random.Range(2, 8);
        enemies.Add(enemy);
    }

    void SpawnWave ()
    {
        for (int i = 0; i < EnemyCountPerWave; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            float height = Random.Range(-6, 9);
            enemy.transform.position = new Vector3(Player.transform.position.x - 30, height, 0);
            enemy.GetComponent<EnemyMovement>().speed = Random.Range(2, 10);
            enemies.Add(enemy);
        }
        waveIntervall = 5f;
        EnemyCountPerWave++;
    }

    public void SetBack()
    {
        timeUntilWaveSpawn = startTimeUntilWaveSpawn;

        EnemyCountPerWave = startEnemyCountPerWave;
        waveIntervall = 0f;
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy, 0.1f);
        }
        enemies.Clear();
    }

    public void Stop()
    {
        SetBack();
        stopped = true;
    }
}
