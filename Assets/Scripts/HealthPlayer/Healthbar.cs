using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //This script is for Health bar UI Canvas which contain totalhealthbar image and currenthealthbar image

    [SerializeField] private Health playerHealth;   //REQUEST   - Drag player object which automatically implement Health script
    [SerializeField] private Image totalhealthBar;  //REQUEST - canvas image for black health
    [SerializeField] private Image currenthealthBar;    //REQUEST - canvas image for red health

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
