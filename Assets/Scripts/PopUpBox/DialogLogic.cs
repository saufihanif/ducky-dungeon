using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogLogic : MonoBehaviour
{
    [SerializeField] private GameObject popUpBox;   //REQUEST
    [Multiline]
    [SerializeField] private string popUpText;      //REQUEST
    private PopUpSystem pop;
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pop = popUpBox.GetComponent<PopUpSystem>();
            pop.PopUp(popUpText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim = popUpBox.GetComponent<Animator>();
            anim.SetTrigger("close");
        }
    }
}
