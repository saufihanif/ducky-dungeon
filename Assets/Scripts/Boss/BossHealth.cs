using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 6f;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;    //REQUEST
    [SerializeField] private GameObject portal;
    private Animator anim;  //Change animation state to hurt

    [SerializeField] private AudioSource bossHurtSound;
    [SerializeField] private AudioSource bossDiestSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);    //Change the currentHealth value when receive damage

        if (currentHealth > 0)
        {
            bossHurtSound.Play();
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                bossDiestSound.Play();
                anim.SetTrigger("die");

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
            }
        }
    }

    private void Deactivate()   //DEACTIVATE OBJECT IN ANIMATOR
    {
        gameObject.SetActive(false);
    }

    private void OpenPortal()
    {
        portal.SetActive(true);
    }
}
