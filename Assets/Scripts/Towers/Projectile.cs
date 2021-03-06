﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public int damage;
    public int pierce = 1;

    void FixedUpdate()
    {
        if (GameManager.instance.paused) {
            return;
        }
        
        transform.position += transform.up * Time.deltaTime * speed;

        if (!gameObject.GetComponent<Renderer>().IsVisibleFrom(GameManager.instance.mainCamera)) {
            Destroy(gameObject);
        }
    }

    public void Hit(Enemy enemy)
    {
        enemy.Damage(damage);

        if (--pierce <= 0) {
            Destroy(gameObject);
        }   
    }
}
