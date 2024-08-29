using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public bool isDead;

    public abstract void Attack();
    public abstract void Die();
    public abstract void Move();

    public abstract void TakeDamage(float damage);
    // public abstract void Heal();
}
