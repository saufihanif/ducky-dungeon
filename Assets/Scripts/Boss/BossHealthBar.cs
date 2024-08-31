using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    //This script is for Health bar UI Canvas which contain totalhealthbar image and currenthealthbar image

    [SerializeField] private BossHealth bossHealth;   //REQUEST   - Drag player object which automatically implement Health script
    [SerializeField] private Image totalhealthBar;  //REQUEST - canvas image for black health
    [SerializeField] private Image currenthealthBar;    //REQUEST - canvas image for red health

    private void Start()
    {
        totalhealthBar.fillAmount = bossHealth.currentHealth / 10;
    }

    private void Update()
    {
        currenthealthBar.fillAmount = bossHealth.currentHealth / 10;
    }
}
