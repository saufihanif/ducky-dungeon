using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;     
    [SerializeField] private float range = 3f;  //size and distance
    [SerializeField] private int damage = 1;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 0.15f;     //Dont want the red box goes way too far
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;     //REQUEST
    private float cooldownTimer = Mathf.Infinity;
    private EnemyPatrol enemyPatrol;
    private Transform playerPos;

    [Header("Flip Enemy")]
    private bool isFlipped = false;

    [SerializeField] private AudioSource meleeEnemySound;

    //References
    private Animator anim;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (enemyPatrol == null)
        {
            LookAtPlayer();
        }

        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                meleeEnemySound.Play();
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight(); //Stop patrol moving when theres player
    }

    private bool PlayerInSight()
    {
        //transform.localscale - change box when object change direction on scale x
        //transform.right - box will apperas right when object scale (1,1,1)

        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * -1 * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * -1 * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void LookAtPlayer()
    {
        if (transform.position.x > playerPos.position.x && isFlipped)   //IF player on left and ifFlipped = true (object already flipped)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            isFlipped = false;
        }
        else if (transform.position.x < playerPos.position.x && !isFlipped) //If player on right and ifFlipped = false (object not yet flipped)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1 , transform.localScale.y, transform.localScale.z);
            isFlipped = true;
        }
    }

    private void DamagePlayer()     //EVENT INSIDE ATTACK ANIMATION
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
