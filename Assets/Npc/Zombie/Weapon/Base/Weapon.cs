using System;
using UnityEngine;

namespace Npc.Zombie.Weapon.Base
{
    public class Weapon : MonoBehaviour
    {
        public float damage;
        
        public float fireRate = 0.5f;
        public float speed;
        private float _nextFireTime = 0f; // Time when the weapon can next fire

        public Transform firePoint;
        public GameObject projectilePrefab;

        public Transform playerPoint;

        public virtual void Fire()
        {
            if (Time.time >= _nextFireTime) // Replace with your input method
            {
                Shoot();
                _nextFireTime = Time.time + 1f / fireRate; // Calculate the next allowed fire time
            }
        }

        protected virtual void Shoot()
        {
            
        }
        
        

    }
    
}