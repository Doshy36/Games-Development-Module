using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerUpgrade : ScriptableObject
{

    public string upgradeName;
    public GameObject upgradePrefab;
    public int upgradePrice;
    public int damage;
    public float fireRate;
    public float range;

}