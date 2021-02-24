using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public float damage;

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;

        if (!gameObject.GetComponent<Renderer>().IsVisibleFrom(GameManager.instance.mainCamera)) {
            Destroy(gameObject);
        }
    }

    public void Hit(Enemy enemy)
    {
        enemy.Damage(damage);
        
        Destroy(gameObject);
    }
}
