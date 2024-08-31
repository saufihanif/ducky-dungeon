using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBoss : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 1.5f;  //size and distance
    [SerializeField] private float damage = 1f;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 3f;     //Dont want the red box goes way too far
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;     //REQUEST   -   for detect player in range
    private float cooldownTimer = Mathf.Infinity;

    [Header("Flip Enemy")]
    private bool isFlipped = false;

    [Header("Follow Player")]
    [SerializeField] private Transform playerPos;
    private float speed = 2.5f;

    //References
    private Animator anim;
    private Health playerHealth;
    private SpriteRenderer sprite;
    private RaycastHit2D hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        LookAtPlayer();

        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (!PlayerInSight() && !anim.GetCurrentAnimatorStateInfo(0).IsName("boss_attack"))
        {
            FollowPlayer();
        }
        else 
        {
            anim.SetBool("moving", false);
        }
    }

    private bool PlayerInSight()
    {
        //transform.localscale - change box when object change direction on scale x
        //transform.right - box will apperas right when object scale (1,1,1)

        if (isFlipped)   //SPRITE TURN RIGHT
        {
            hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        }
        else                //SPRITE TURN LEEFT
        {
            hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * -1 * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        }

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (isFlipped)   //SPRITE TURN RIGHT
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        }
        else                //SPRITE TURN LEFT
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * -1 * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        }
    }

    private void LookAtPlayer()
    {
        if (transform.position.x > playerPos.position.x && isFlipped)   //IF player on left and ifFlipped = true (object already flipped)
        {
            Debug.Log("Flip Left");
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            sprite.flipX = false;
            isFlipped = false;
        }
        else if (transform.position.x < playerPos.position.x && !isFlipped) //If player on right and ifFlipped = false (object not yet flipped)
        {
            Debug.Log("Flip Right");
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            sprite.flipX = true;
            isFlipped = true;
        }
    }

    private void FollowPlayer()
    {
        anim.SetBool("moving", true);

        Vector2 target = new Vector2(playerPos.position.x, transform.position.y);
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
    }

    private void DamagePlayer()     //EVENT INSIDE ATTACK ANIMATION
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
