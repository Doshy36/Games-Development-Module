using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortarTower : Tower
{

    public GameObject target;
    private bool movingTarget;

    protected override void Spawn()
    {
        target.transform.position = GameManager.instance.level.route[0].position;
    }

    protected override void OnUpdate()
    {
        if (movingTarget)
        {
            target.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                movingTarget = false;
                target.SetActive(false);
            }
        }
    }

    protected override Projectile Fire()
    {
        Projectile projectile = base.Fire();
        projectile.speed = 1.0F;
        projectile.pierce = 1;
        return projectile;
    }

    protected override bool HandleFiring()
    {
        return true;
    }

    public override void OnOpenUpgradeMenu()
    {
        GameManager.instance.towerManager.mortarTargetButton.gameObject.SetActive(true);
        GameManager.instance.towerManager.mortarTargetButton.onClick.AddListener(OnTargetButtonClick);

        target.SetActive(true);
    }

    public override void OnCloseUpgradeMenu()
    {
        GameManager.instance.towerManager.mortarTargetButton.gameObject.SetActive(false);

        target.SetActive(false);
        movingTarget = false;
    }

    public void OnTargetButtonClick()
    {
        target.SetActive(true);
        movingTarget = true;
    }

}
