using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : WeaponBase
{
    void Start()
    {
        damage = 50;
        usuableBulletAmount = 6;
        magazineCapacity = 3;
        currentBulletAmount = magazineCapacity;
        gameObject.tag = "Bazooka";
    }
}
