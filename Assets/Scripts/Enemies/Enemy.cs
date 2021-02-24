using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public delegate void OnDeath();

    public GameManager gameManager;
    public float health = 1.0f;
    public float speed = 1.0f;
    public int level = 1;
    private int currentTarget = 0;

    public OnDeath deathListener;

    void Start()
    {
        
    }

    public void Damage(float damage) 
    {
        health -= damage;
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
        if (gameManager.paused) {
            return;
        }

        Transform nextTarget = gameManager.level.route[currentTarget];
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, speed);

        if (transform.position == nextTarget.position) {
            transform.position = nextTarget.position;

            if (++currentTarget >= gameManager.level.route.Length) {
                gameManager.Damage(level);

                Die();
            }
        } 
    }
}
