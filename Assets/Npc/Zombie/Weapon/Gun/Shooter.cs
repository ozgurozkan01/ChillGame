using Npc.Zombie.Weapon.Ammo;
using UnityEngine;

namespace Npc.Zombie.Weapon.Gun
{
    public class Shooter : Base.Weapon
    {
        protected override void Shoot()
        {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var rb = projectile.GetComponent<AmmoBase>().rb;
            
            if (rb != null)
            {
                rb.AddForce(firePoint.up * 10f); // Adjust force as needed
            }
        }
        
    }
}