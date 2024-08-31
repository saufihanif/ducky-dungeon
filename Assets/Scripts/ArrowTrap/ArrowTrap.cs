using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform firePoint;       //REQUEST - point where arrow objject get released
    [SerializeField] private GameObject[] arrows;       //REQUEST - an array where arrow ibject stored
    private float cooldownTimer;
    [SerializeField] private AudioSource arrowSound;

    private void Attack()
    {
        cooldownTimer = 0;
        arrowSound.Play();
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<ArrowProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)   //Find any arrow that active in hierarchy
            {
                return i;
            }
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)    //Shoot another arrow
        {
            Attack();   //Find another arrow
        }
    }
}
