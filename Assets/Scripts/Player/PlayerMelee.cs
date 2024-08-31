using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 2f;  //size and distance
    [SerializeField] private int damage = 1;
    private PlayerMovement playerMovement;

    private bool[] attackState = new bool[3];
    private enum Attack { idle, attack1, attack2, attack3 }
    Attack stateNum;    //type MovementState which is an enum

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 0.5f;     //Dont want the red box goes way too far
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;     //REQUEST
    [SerializeField] private LayerMask bossLayer;     //REQUEST
    [SerializeField] private LayerMask gateOpen;     //REQUEST
    private float cooldownTimer = Mathf.Infinity;

    [Header("Audio Source")]
    [SerializeField] private AudioSource attackSound;

    //References
    private Animator anim;
    private EnemyHealth enemyHealth;
    private BossHealth bossHealth;
    private Gate gate;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //DONT ALLOWED USER TO CLICKED MOUSE WHILE ANIMATION ATTACK IS PLAYING
        if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_attack3"))
        {
            if (cooldownTimer >= attackCooldown && playerMovement.canAttack())
            {
                Debug.Log("ATTACK");
                if (!attackState[0] && !attackState[1] && !attackState[2])  //All three FALSE - change 0 to true
                {
                    attackState[0] = true;
                    //stateNum = Attack.attack1;      //STATE = 1;
                    anim.SetTrigger("attack_1");
                    attackSound.Play();
                }
                else if (attackState[0] && !attackState[1] && !attackState[2])
                {
                    attackState[1] = true;
                    //stateNum = Attack.attack2;      //STATE = 2;
                    anim.SetTrigger("attack_2");
                    attackSound.Play();
                }
                else if (attackState[0] && attackState[1] && !attackState[2])
                {
                    attackState[2] = true;
                    //stateNum = Attack.attack3;      //STATE = 3;
                    anim.SetTrigger("attack_3");
                    attackSound.Play();
                }

                if (attackState[0] && attackState[1] && attackState[2])
                {
                    attackState[0] = false;
                    attackState[1] = false;
                    attackState[2] = false;
                }

               // anim.SetInteger("state_attack", (int)stateNum);
            }
        }
    }

    

    private bool EnemyInSight()
    {
        int x = 1;
        //transform.localscale - change box when object change direction on scale x
        //transform.right - box will apperas right when object scale (1,1,1)

        if (playerMovement.FlipedStatus())  //IF THE SPRITE IS ALREADY FLIPPED IN PLAYERMOVEMENT
        {
            x = -1;
        }
        
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * x * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<EnemyHealth>();

        return hit.collider != null;
    }

    private bool BossInSight()
    {
        int x = 1;
        //transform.localscale - change box when object change direction on scale x
        //transform.right - box will apperas right when object scale (1,1,1)

        if (playerMovement.FlipedStatus())  //IF THE SPRITE IS ALREADY FLIPPED IN PLAYERMOVEMENT
        {
            x = -1;
        }

        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * x * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, bossLayer);

        if (hit.collider != null)
            bossHealth = hit.transform.GetComponent<BossHealth>();

        return hit.collider != null;
    }

    private bool GateInSight()
    {
        int x = 1;
        //transform.localscale - change box when object change direction on scale x
        //transform.right - box will apperas right when object scale (1,1,1)

        if (playerMovement.FlipedStatus())  //IF THE SPRITE IS ALREADY FLIPPED IN PLAYERMOVEMENT
        {
            x = -1;
        }

        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * x * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, gateOpen);

        if (hit.collider != null)
            gate = hit.transform.GetComponent<Gate>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        playerMovement = GetComponent<PlayerMovement>();
        int y = 1;

        if (playerMovement.FlipedStatus())  //IF THE SPRITE IS ALREADY FLIPPED IN PLAYERMOVEMENT
        {
            y = -1;
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * y * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamageEnemy()     //EVENT INSIDE ATTACK ANIMATION
    {
        if (EnemyInSight())
        {
            enemyHealth.TakeDamage(damage);
        }
        else if (BossInSight())
        {
            bossHealth.TakeDamage(damage);
        }
        else if (GateInSight())
        {
            gate.OpenGate();
        }
    }
}
