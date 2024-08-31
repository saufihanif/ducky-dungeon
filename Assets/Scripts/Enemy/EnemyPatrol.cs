using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;       //REQUEST
    [SerializeField] private Transform rightEdge;      //REQUEST

    [Header("Enemy")]
    [SerializeField] private Transform enemy;       //REQUEST - transform component for enemy

    [Header("Movement parameters")]
    [SerializeField] private float speed = 1f;
    private Vector3 initScale;  //initial scale
    private bool movingRight;    //if true enemy will move to left direction

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration = 1f;
    private float idleTimer;    //Stop for a moment when change direction

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;     //REQUEST - animator component for enemy

    private void Awake()
    {
        initScale = enemy.localScale;   //Initial scale
    }

    private void OnDisable()    //What happen when component disabled
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (movingRight)
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(-1);     //move to right
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(1);    //move to left
            else
                DirectionChange();
        }
    }

    private void DirectionChange()  //Handle turning enemy around
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingRight = !movingRight;
    }

    private void MoveInDirection(int _direction)    //FLIP THE ENEMY
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(initScale.x * _direction, initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * -1 * speed, enemy.position.y, enemy.position.z);
    }
}
