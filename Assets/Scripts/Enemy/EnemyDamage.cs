using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage = 1f;   //**INHERITANCE - use protected

    protected void OnTriggerEnter2D(Collider2D collision)   //USE TRIGGER
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
