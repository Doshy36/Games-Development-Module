using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : Tower
{

    public GameObject gunObject;

    protected override void Spawn()
    {
        Vector3 vectorToTarget = GameManager.instance.level.spawn.position - gunObject.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        gunObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    protected override bool HandleFiring()
    {
        Enemy enemy = GameManager.instance.GetClosestEnemy(transform.position, range);
        if (enemy != null) {
            Vector3 vectorToTarget = enemy.transform.position - gunObject.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            gunObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            return true;
        }
        return false;
    }

}
