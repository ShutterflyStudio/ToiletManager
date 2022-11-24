using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    private Animator animator;
    public GameObject UI_Canvas;


    private void Start()
    {
        UI_Canvas.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void SelectThis()
    {
        animator.SetBool("Activated", true);

        UI_Canvas.SetActive(true);
    }

    public void DeselctThis()
    {
        animator.SetBool("Activated", false);

        UI_Canvas.SetActive(false);
    }
}
