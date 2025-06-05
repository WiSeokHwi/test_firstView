using System;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health)
        {
            health.TakeDamage(10);
        }
    }
}
