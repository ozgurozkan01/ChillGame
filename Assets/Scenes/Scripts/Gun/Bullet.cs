using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private float speed = 50;
    public Vector3 targetPosition;
    public float damage;
    void Update()
    {
        MoveTarget();
    }

    public void SetTargetTransform(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }
    
    void MoveTarget()
    {
        if (targetPosition == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = targetPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(targetPosition);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController controller = other.gameObject.GetComponent<EnemyController>();
            if (controller)
            {
                controller.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }
}
