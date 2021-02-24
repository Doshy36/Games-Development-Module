using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public Level level;
    private List<Enemy> enemies = new List<Enemy>();
    public int health = 100;

    public bool paused = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(int damage) 
    {
        health -= damage;

        if (health <= 0) {
            // End
        }
    }
    public Enemy SpawnEnemy(int level) 
    {
        if (level >= enemyPrefabs.Length) {
            return null;
        }

        GameObject gameObject = Instantiate(enemyPrefabs[level], this.level.spawn.position, new Quaternion());
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.gameManager = this;
        enemy.deathListener = () => enemies.Remove(enemy);

        enemies.Add(enemy);
        return enemy;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Play()
    {
        paused = false;
    }

    public int getEnemyCount() {
        return enemies.Count;
    }

}
