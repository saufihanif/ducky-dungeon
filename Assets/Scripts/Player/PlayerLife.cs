using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Health latePlayer;

    [SerializeField] private Text countdownText;

    private float currentTime = 0f;
    private float startingTime = 480f;
    private float minutes;
    private float seconds;


    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        latePlayer = GetComponent<Health>();

        currentTime = startingTime;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("kill_trap"))
        {
            DiePlayer();
        }
    }*/

    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        countdownText.text = niceTime;

        if (currentTime <= 0)
        {
            latePlayer.TakeDamage(1);
            currentTime = startingTime;
        }
    }

    public void DiePlayer()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    //Reload Level
    private void RestartLevel()     //EVENT INSIDE DIE ANIMATION
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
