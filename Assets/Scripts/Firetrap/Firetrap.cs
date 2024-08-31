using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    [Header("Firetraps Timer")]
    [SerializeField] private float activationDelay = 2f;
    [SerializeField] private float activeTime = 2f;

    //Reference
    private Animator anim;
    private SpriteRenderer spriteRend;
    private Health playerHealth;

    private bool triggered; //when the trap get triggered - turn sprite into red
    private bool active;    //when the trap get activated

    [SerializeField] private AudioSource firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage);
    }

    //WHEN PLAYER STAR TRIGGER FIRETRAP - FIRST THING HAPPEN WHEN TRIGGERED
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }

            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    //PLAYER RAN AWAY FROM TRAP - SO PLAYER HEALTH NOT EFFECTED
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null;
    }

    private IEnumerator ActivateFiretrap()
    {
        //turn trap into red
        triggered = true;
        spriteRend.color = Color.red;

        //wait delay activate trap, turn on animation
        yield return new WaitForSeconds(activationDelay);
        firetrapSound.Play();
        spriteRend.color = Color.white;   //turn sprite back into its colour
        active = true;
        anim.SetBool("activated", true);

        //wait until x second, deactivate the trap
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
