using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //This script is for Player object

    [Header("Health")]
    [SerializeField] private float startingHealth = 3f; 
    public float currentHealth { get; private set; }    //Get from anywhere but cannot set from anywhere
    private bool dead;

    [Header("Components")]
    private Animator anim;  //Change animation state to hurt
    private PlayerLife killPlayer;
    private bool invulnerable;      //I DONT KNOW WHAT FOR - MAYBE DONT WANT DOUBLE DISABLE COMPONENT WHILE IFRAME

    [Header("IFrames")]
    [SerializeField] private float iFramesDuration = 1f;    //How long player invulnerable
    [SerializeField] private int numberOffFlashes = 3;      //How many flashed before turn into normal state
    private SpriteRenderer spriteRend;  //Change colour when invulnerable

    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource addHealthSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        killPlayer = GetComponent<PlayerLife>();    //Get PlayerLife script
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
            hurtSound.Play();
        }
        else
        {
            if (!dead)
            {
                killPlayer.DiePlayer();

                deathSound.Play();
                dead = true;
            }
        }
    }

    public void AddHealth(float _value) //Add Health from collectable
    {
        addHealthSound.Play();
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()    //COROUTINE enable user to pause a code and give access to unity to do something and continue
    {
        invulnerable = true;

        Physics2D.IgnoreLayerCollision(9, 10, true);    //Player-damageEntity
        Physics2D.IgnoreLayerCollision(9, 11, true);    //Player-Monster
        Physics2D.IgnoreLayerCollision(9, 12, true);    //Player-Boss
        //invulnerable
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(9, 10, false);
        Physics2D.IgnoreLayerCollision(9, 11, false);
        Physics2D.IgnoreLayerCollision(9, 12, false);    //Player-Boss
        invulnerable = false;
    }
}
