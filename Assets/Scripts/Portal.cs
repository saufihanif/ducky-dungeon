using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool levelCompleted = false;
    private bool triggerEntered = false;

    private void Start()
    {
        //finishSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerEntered == true && !levelCompleted)
        {
            Debug.Log("BUTTON PRESSED");
            levelCompleted = true;
            Invoke("CompleteLevel", 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We set this variable to indicate that character is in trigger
        triggerEntered = true;
        Debug.Log("trigger entered");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // We reset this variable since character is no longer in the trigger
        triggerEntered = false;
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
