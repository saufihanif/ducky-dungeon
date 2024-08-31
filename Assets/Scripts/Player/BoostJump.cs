using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostJump : MonoBehaviour
{
    [SerializeField] private float speedBoost = 7f;
    [SerializeField] private float jumpBoost = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().BoostPlayer(speedBoost, jumpBoost);
            gameObject.SetActive(false);
        }
    }
}
