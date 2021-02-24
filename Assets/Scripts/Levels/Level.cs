using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{

    public GameManager gameManager;
    public Tilemap tileMap;
    public Transform spawn;
    public Round[] rounds;
    public Transform[] route;

    [HideInInspector]
    public GameObject projectileHolder;
    public GameObject enemyHolder;

    private int currentRound;
    private int roundData;
    private int entitiesToSpawn;
    private float lastSpawn;
    private float pauseUntil;

    public Round round {
        get {
            return rounds[currentRound];
        }
    }

    public SpawnData spawnData {
        get {
            return round.data[roundData];
        }
    }

    void Awake()
    {
        projectileHolder = new GameObject("Projectile Holder");
        enemyHolder = new GameObject("Enemy Holder");

        lastSpawn = Time.fixedTime;
        entitiesToSpawn = spawnData.enemies;
    }

    void FixedUpdate()
    {
        if (gameManager.paused) {
            return;
        }

        if (entitiesToSpawn == 0) {
            if (gameManager.getEnemyCount() == 0) {
                if (++roundData >= round.data.Length) {
                    NextRound();
                } else {
                    pauseUntil = spawnData.pauseTime;
                    entitiesToSpawn = spawnData.enemies;
                }
            }
            return;
        }

        if (pauseUntil > 0 && Time.time < pauseUntil) {
            return;
        }

        if (Time.fixedTime - lastSpawn >= spawnData.spawnRate) {
            lastSpawn = Time.fixedTime;

            Enemy enemy = gameManager.SpawnEnemy(spawnData.levels[Random.Range(0, spawnData.levels.Length)]);
            entitiesToSpawn--;
        }
    }

    private void NextRound()
    {
        if (++currentRound >= rounds.Length) {
            gameManager.Pause();
            return;
        }

        entitiesToSpawn = spawnData.enemies;
        roundData = 0;
    }

}
