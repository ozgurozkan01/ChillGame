using System;
using UnityEngine;

namespace Npc.Zombie.Weapon.Ammo
{
    public class AmmoBase : MonoBehaviour
    {
        public int damage;
        public Rigidbody rb;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Hit");
            }
        }
    }
}