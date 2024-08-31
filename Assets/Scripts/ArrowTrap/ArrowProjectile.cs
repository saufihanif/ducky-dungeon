using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : EnemyDamage  //INHERIT FROM ENEMYDAMAGE SCRIPT
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;
    private float lifetime;     //How long before disable
    private bool hit;

    private Animator anim;
    private BoxCollider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    //When arrows get activated
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    //Move the arrow
    private void Update()
    {
        //If hit dont stop moving arrow
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent INHERIT script first - EnemyDamage script

        gameObject.SetActive(false); //When this hits any object deactivate arrow
    }
}
