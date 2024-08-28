using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponBase
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
        usuableBulletAmount = 48;
        magazineCapacity = 24;
        currentBulletAmount = magazineCapacity;
        gameObject.tag = "MachineGun";
    }
}
