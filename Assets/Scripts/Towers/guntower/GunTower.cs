using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTower : Tower
{

    public GameObject gunObject;
    public float gunSpeed;
    public float projectileSpeed;
    public int pierce;

    protected override void Spawn()
    {
        Vector3 vectorToTarget = GameManager.instance.level.spawn.position - gunObject.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        gunObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    protected override Projectile Fire()
    {
        Projectile projectile = base.Fire();
        projectile.speed = projectileSpeed;
        projectile.pierce = pierce;
        return projectile;
    }

    protected override bool HandleFiring()
    {
        Enemy enemy = GameManager.instance.GetClosestEnemy(transform.position, range);
        if (enemy != null) {
            Vector3 vectorToTarget = enemy.transform.position - gunObject.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            float difference = Quaternion.Angle(gunObject.transform.rotation, target);
            gunObject.transform.rotation = Quaternion.Lerp(gunObject.transform.rotation, target, Time.deltaTime * gunSpeed);
            return difference <= 3f;
        }
        return false;
    }

    public override void Upgrade(TowerUpgrade upgradeInfo) 
    {
        base.Upgrade(upgradeInfo);

        GunTowerUpgrade upgrade = (GunTowerUpgrade) upgradeInfo;

        gunSpeed = upgrade.gunSpeed;
        projectileSpeed = upgrade.projectileSpeed;
        pierce = upgrade.pierce;
    }

}
