using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    MachineGun,
    Bazooka
}

public class WeaponBase : MonoBehaviour
{
    public Transform barrelTransform;
    protected float usuableBulletAmount;
    protected float magazineCapacity;
    protected float currentBulletAmount;
    protected float damage;
    
    public void Shoot()
    {
        currentBulletAmount--;

        if (currentBulletAmount <= 0)
        {
            Reload();
        }
        
    }

    public void Reload()
    {
        if (usuableBulletAmount >= magazineCapacity)
        {
            usuableBulletAmount -= magazineCapacity - currentBulletAmount;
            currentBulletAmount = magazineCapacity;
        }
        else
        {
            currentBulletAmount = usuableBulletAmount;
            usuableBulletAmount = 0;
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public Transform GetBarreTrasnform()
    {
        return barrelTransform;
    }
}
    