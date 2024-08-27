using UnityEngine;

namespace Npc.Zombie.Weapon.Base
{
    public class Weapon : MonoBehaviour
    {
        public string weaponName;
        public float damage;
        
        public float fireRate = 0.5f;
        public float nextFireTime = 0f; // Time when the weapon can next fire

        public Transform firePoint;
        public GameObject projectilePrefab; 

        public virtual void Fire()
        {
            if (Time.time >= nextFireTime) // Replace with your input method
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate; // Calculate the next allowed fire time
            }
        }

        protected virtual void Shoot()
        {
            
        }

    }
    
}