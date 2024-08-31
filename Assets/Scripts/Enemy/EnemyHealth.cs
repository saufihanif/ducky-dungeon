using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 3f;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;    //REQUEST
    private Animator anim;  //Change animation state to hurt
    private bool invulnerable;  //I DONT KNOW WHAT FOR - MAYBE DONT WANT DOUBLE DISABLE COMPONENT WHILE IFRAME

    [Header("IFrames")]
    [SerializeField] private float iFramesDuration = 1f;    //How long enemy invulnerable
    [SerializeField] private int numberOffFlashes = 3;      //How many flashed before turn into normal state
    private SpriteRenderer spriteRend;  //Change colour when invulnerable

    [SerializeField] private AudioSource enemyHurtSound;
    [SerializeField] private AudioSource enemyDieSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);    //Change the currentHealth value when receive damage

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());   //CALL IENUMERATOR
            enemyHurtSound.Play();
        }
        else
        {
            if (!dead)
            {
                enemyDieSound.Play();
                anim.SetTrigger("die");

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
            }
        }
    }

    private IEnumerator Invunerability()    //COROUTINE enable user to pause a code and give access to unity to do something and continue
    {
        invulnerable = true;

        Physics2D.IgnoreLayerCollision(9, 11, true);
        //invulnerable
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(9, 11, false);
        invulnerable = false;
    }

    private void Deactivate()   //DEACTIVATE OBJECT IN ANIMATOR
    {
        gameObject.SetActive(false);
    }
}
