using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public delegate void OnDeath();

    public float health = 1.0f;
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

    public void Damage(float damage) 
    {
        health -= damage;

        if (health <= 0) {
            if (--level <= 0) {
                GameManager.instance.playerManager.AddCoins(reward);

                Die();
            } else {
                GameObject enemyObject = GameManager.instance.enemyPrefabs[level - 1];
                Enemy enemy = enemyObject.GetComponent<Enemy>();

                GetComponent<SpriteRenderer>().sprite = GameManager.instance.enemyPrefabs[level - 1].GetComponent<SpriteRenderer>().sprite;
                health = enemy.health;
                speed = enemy.speed;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);

        if (deathListener != null) {
            deathListener.Invoke();
        }
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
