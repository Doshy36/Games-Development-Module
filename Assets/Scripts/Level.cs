using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameManager gameManager;
    public Transform spawn;
    [Range(1, 100)]
    public int rounds;
    public Transform[] route;

    private int currentRound;
    private int entitiesToSpawn;
    private float spawnSpeed;

    private float lastSpawn;

    void Start()
    {
        lastSpawn = Time.fixedTime;

        NextRound();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.paused) {
            return;
        }

        if (entitiesToSpawn == 0 && gameManager.getEnemyCount() == 0) {
            NextRound();
            return;
        }

        if (Time.fixedTime - lastSpawn >= spawnSpeed) {
            lastSpawn = Time.fixedTime;

            Enemy enemy = gameManager.SpawnEnemy(0);   
            entitiesToSpawn--;
        }
    }

    private void NextRound()
    {
        currentRound++;
        entitiesToSpawn = currentRound * 10;
        spawnSpeed = (1 / currentRound) * 2;
    }
}
