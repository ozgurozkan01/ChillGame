using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        usuableBulletAmount = 26;
        magazineCapacity = 13;
        currentBulletAmount = magazineCapacity;
        gameObject.tag = "Pistol";
    }
}
