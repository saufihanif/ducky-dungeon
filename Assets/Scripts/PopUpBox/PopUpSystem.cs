using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private TMP_Text popUpText;    //REQUEST

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PopUp(string _text)
    {
        gameObject.SetActive(true);
        popUpText.text = _text;
        anim.SetTrigger("pop");
    }
}
