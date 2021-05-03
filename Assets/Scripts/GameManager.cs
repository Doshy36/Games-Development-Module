using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Main Settings")]
    public Camera mainCamera;
    public PlayerManager playerManager;
    public ShopManager shopManager;
    public TowerManager towerManager;
    public Level level;

    [Header("Game Data")]
    public GameObject[] enemyPrefabs;
    private List<Enemy> enemies = new List<Enemy>();
    public int startingHealth;
    public int startingCoins = 100;

    [Header("Game References")]
    public Text coinText;
    public Text playButton;
    public Text speedButton;
    public AudioSource enemyDeathSource;
    public AudioSource gameOverSource;
    public AudioSource victorySource;

    [Header("Dev Tools")]
    public bool infiniteHealth;
    public bool infiniteMoney;
    public Button infiniteHealthButton;
    public Button infiniteMoneyButton;

    public bool paused = false;
    private bool speed = false;
    private float timer;

    void Awake()
    {
        instance = this;
    }

    public Enemy SpawnEnemy(int level)
    {
        if (level >= enemyPrefabs.Length)
        {
            return null;
        }

        GameObject gameObject = Instantiate(enemyPrefabs[level], this.level.spawn.position, new Quaternion(), this.level.enemyHolder.transform);
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.deathListener = () =>
        {
            enemies.Remove(enemy);
            enemyDeathSource.Play();
        };
        enemy.name = "Enemy " + enemies.Count;

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

    public void TogglePlay()
    {
        paused = !paused;

        Time.timeScale = paused ? 0 : (speed ? 2 : 1);

        playButton.text = paused ? "Play" : "Pause";
    }

    public void ToggleSpeed()
    {
        speed = !speed;
        Time.timeScale = speed ? 2 : 1;

        speedButton.text = speed ? "Normal" : "Fast";
    }

    public Enemy GetClosestEnemy(Vector3 position, float range)
    {
        Enemy closestEnemy = null;
        foreach (Enemy enemy in enemies)
        {
            if (Vector2.Distance(position, enemy.transform.position) <= range)
            {
                if (closestEnemy == null
                    || enemy.currentTarget > closestEnemy.currentTarget
                    || (enemy.currentTarget == closestEnemy.currentTarget && enemy.distanceToTarget < closestEnemy.distanceToTarget))
                {
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }

    public void ActivateInfiniteHealth()
    {
        infiniteHealth = true;

        infiniteHealthButton.gameObject.SetActive(false);
    }

    public void ActivateInfiniteMoney()
    {
        infiniteMoney = true;

        infiniteMoneyButton.gameObject.SetActive(false);

        shopManager.UpdateButtonColors();
    }

    public int getEnemyCount()
    {
        return enemies.Count;
    }

}