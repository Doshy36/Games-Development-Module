using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public delegate void OnDeath();

    public float speed = 1.0f;
    public int level = 1;
    [HideInInspector]
    public int currentTarget = 0;
    [HideInInspector]
    public float distanceToTarget = 0;
    private int reward;

    public OnDeath deathListener;

    void Awake()
    {
        reward = level;
    }

    public void Damage(int damage) 
    {
        level -= damage;
        if (level <= 0) {
            GameManager.instance.playerManager.AddCoins(reward);

            Die();
        } else {
            GameObject enemyObject = GameManager.instance.enemyPrefabs[level];
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            GetComponent<SpriteRenderer>().sprite = GameManager.instance.enemyPrefabs[level - 1].GetComponent<SpriteRenderer>().sprite;
            speed = enemy.speed;
        }
    }

    public void Die()
    {
        if (deathListener != null) {
            deathListener.Invoke();
        }

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (GameManager.instance.paused) {
            return;
        }

        Transform nextTarget = GameManager.instance.level.route[currentTarget];
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, speed);

        if (transform.position == nextTarget.position) {
            transform.position = nextTarget.position;

            if (++currentTarget >= GameManager.instance.level.route.Length) {
                GameManager.instance.playerManager.Damage(level);

                Die();
                return;
            }
            nextTarget = GameManager.instance.level.route[currentTarget];
        }
        distanceToTarget = Vector3.Distance(transform.position, nextTarget.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null) {
            projectile.Hit(this);
        }
    }
}
