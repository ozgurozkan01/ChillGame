using Npc.Zombie.Weapon.Gun;
using UnityEngine;

namespace Npc.Zombie.Weapon.Rocket
{
    public class RocketShooter : Shooter
    {
        protected override void Shoot()
        {
            transform.LookAt(playerPoint.transform);

            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var rb = projectile.GetComponent<Ammo.Rocket>().rb;
            
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * speed); // Adjust force as needed
            }
        }
        
    }
}